using Okra.MEF.Util;
using System;
using System.Collections.Generic;
using System.Composition.Hosting.Core;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Okra.MEF.DependencyInjection.ExportDescriptorProviders
{
    internal class AggregateExportDescriptorProvider : ExportDescriptorProvider
    {
        private IEnumerable<ExportDescriptorProvider> _exportDescriptorProviders;
        private HashSet<CompositionContract> _activeElementResolverContracts = new HashSet<CompositionContract>();
        private static readonly MethodInfo s_getEnumerableDescriptorMethod = typeof(AggregateExportDescriptorProvider).GetTypeInfo().GetDeclaredMethod(nameof(GetEnumerableDescriptor));

        public AggregateExportDescriptorProvider(IEnumerable<ExportDescriptorProvider> exportDescriptorProviders)
        {
            this._exportDescriptorProviders = exportDescriptorProviders;
        }

        public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors(CompositionContract contract, DependencyAccessor descriptorAccessor)
        {
            var exportDescriptors = _exportDescriptorProviders.SelectMany(p => p.GetExportDescriptors(contract, descriptorAccessor));

            // NB: We may need to use the HashSet<CompositionContract> trick to turn off export descriptors here
            //     This would allow us to get just those exported as MEF parts
            //     If we are looking for the last export, we can then decide to return NoExportDescriptors if a MEF part exists

            if (exportDescriptors.Count() > 0)
            {
                if (_activeElementResolverContracts.Contains(contract))
                {
                    // If we are currently resolving the elements of an IEnumerable<T> then return all the items

                    return exportDescriptors;
                }
                else
                {
                    // Otherwise return just the last element (TODO : prioritising closed generics over open generics)

                    return new ExportDescriptorPromise[] { exportDescriptors.Last() };
                }
            }
            else if (contract.ContractType.IsConstructedGenericType && contract.ContractType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                // We are looking for an export of the type IEnumerable<T> and this hasn't been added specifically

                var elementType = contract.ContractType.GenericTypeArguments[0];
                var elementContract = contract.ChangeType(elementType);

                var getDescriptorMethod = s_getEnumerableDescriptorMethod.MakeGenericMethod(elementType);
                var getDescriptorDelegate = getDescriptorMethod.CreateStaticDelegate<Func<CompositionContract, CompositionContract, DependencyAccessor, HashSet<CompositionContract>, object>>();

                return new[] { (ExportDescriptorPromise)getDescriptorDelegate(contract, elementContract, descriptorAccessor, _activeElementResolverContracts) };
            }
            else
                return NoExportDescriptors;
        }

        private static ExportDescriptorPromise GetEnumerableDescriptor<TElement>(CompositionContract enumerableContract, CompositionContract elementContract, DependencyAccessor definitionAccessor, HashSet<CompositionContract> activeElementResolverContracts)
        {
            return new ExportDescriptorPromise(
                 enumerableContract,
                 typeof(TElement[]).Name,
                 false,
                 () =>
                 {
                     activeElementResolverContracts.Add(elementContract);
                     var dependencies = definitionAccessor.ResolveDependencies("item", elementContract, true);
                     activeElementResolverContracts.Remove(elementContract);
                     return dependencies;
                 },
                 d =>
                 {
                     var dependentDescriptors = d
                         .Select(el => el.Target.GetDescriptor())
                         .ToArray();

                     return ExportDescriptor.Create((c, o) => dependentDescriptors.Select(e => (TElement)e.Activator(c, o)).ToArray(), NoMetadata);
                 });
        }
    }
}
