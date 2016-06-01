using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Composition.Hosting.Core;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Okra.MEF.Util;

namespace Okra.MEF.DependencyInjection.ExportDescriptorProviders
{
    public class TypeExportDescriptorProvider : ExportDescriptorProvider
    {
        private ServiceDescriptor _serviceDescriptor;

        private static readonly MethodInfo s_getTypedDescriptorMethod = typeof(TypeExportDescriptorProvider).GetTypeInfo().GetDeclaredMethod(nameof(GetTypedDescriptor));

        public TypeExportDescriptorProvider(ServiceDescriptor serviceDescriptor)
        {
            TypeInfo serviceType = serviceDescriptor.ServiceType.GetTypeInfo();
            TypeInfo implementationType = serviceDescriptor.ImplementationType.GetTypeInfo();

            if (implementationType.IsAbstract ||
                implementationType.IsInterface ||
                implementationType.IsGenericTypeDefinition)
            {
                throw new ArgumentException(string.Format(Resources.TypeCannotBeActivated, serviceDescriptor.ImplementationType, serviceDescriptor.ServiceType));
            }

            this._serviceDescriptor = serviceDescriptor;
        }

        public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors(CompositionContract contract, DependencyAccessor descriptorAccessor)
        {
            if (contract.ContractType != _serviceDescriptor.ServiceType)
                return NoExportDescriptors;

            Type implementationType = _serviceDescriptor.ImplementationType;
            var implementationContract = contract.ChangeType(implementationType);

            var getDescriptorMethod = s_getTypedDescriptorMethod.MakeGenericMethod(contract.ContractType);
            var getDescriptorDelegate = getDescriptorMethod.CreateStaticDelegate<Func<CompositionContract, CompositionContract, ServiceLifetime, DependencyAccessor, object>>();

            return new[] { (ExportDescriptorPromise)getDescriptorDelegate(contract, implementationContract, _serviceDescriptor.Lifetime, descriptorAccessor) };
        }

        public static ExportDescriptorPromise GetTypedDescriptor<TElement>(CompositionContract contract, CompositionContract implementationContract, ServiceLifetime lifetime, DependencyAccessor definitionAccessor)
        {
            ConstructorInfo constructor;
            IEnumerable<CompositionDependency> dependencies;

            if (!TryGetLongestComposableConstructor(implementationContract, definitionAccessor, out constructor, out dependencies))
            {
                return new ExportDescriptorPromise(contract, typeof(TElement).Name, false, NoDependencies, ds =>
                    {
                        CompositeActivator activator = (c, o) =>
                        {
                            throw new InvalidOperationException(string.Format(Resources.NoConstructorMatch, implementationContract.ContractType));
                        };

                        return ExportDescriptor.Create(activator, NoMetadata);
                    });
            }

            return new ExportDescriptorPromise(
                 contract,
                 typeof(TElement).Name,
                 false,
                 () => dependencies,
                 ds =>
                 {
                     var parameterActivators = ds.Select(d => d.Target.GetDescriptor().Activator).ToArray();

                     CompositeActivator activator = (c, o) =>
                         {
                             var parameters = parameterActivators.Select(pa => CompositionOperation.Run(c, pa)).ToArray();

                             object result;

                             try
                             {
                                 result = constructor.Invoke(parameters);
                             }
                             catch (TargetInvocationException e)
                             {
                                 throw e.InnerException;
                             }

                             if (result is IDisposable)
                                 c.AddBoundInstance((IDisposable)result);

                             return result;
                         };

                     return ExportDescriptor.Create(activator.ApplyServiceLifetime(lifetime), NoMetadata);
                 });
        }

        private static bool TryGetLongestComposableConstructor(CompositionContract implementationContract, DependencyAccessor definitionAccessor, out ConstructorInfo constructor, out IEnumerable<CompositionDependency> dependencies)
        {
            var availableConstructors = implementationContract.ContractType.GetTypeInfo().DeclaredConstructors
                                .Where(c => c.IsPublic)
                                .OrderByDescending(c => c.GetParameters().Length);

            foreach (var availableConstructor in availableConstructors)
            {
                IEnumerable<CompositionDependency> constructorDependencies;

                if (TryResolveConstructorDependencies(availableConstructor, definitionAccessor, out constructorDependencies))
                {
                    constructor = availableConstructor;
                    dependencies = constructorDependencies;
                    return true;
                }
            }

            constructor = null;
            dependencies = null;
            return false;
        }

        private static bool TryResolveConstructorDependencies(ConstructorInfo constructor, DependencyAccessor definitionAccessor, out IEnumerable<CompositionDependency> dependencies)
        {
            var parameters = constructor.GetParameters();
            var dependencyArray = new CompositionDependency[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                CompositionDependency dependency;
                var parameterContract = new CompositionContract(parameters[i].ParameterType);

                if (definitionAccessor.TryResolveOptionalDependency("parameter", parameterContract, true, out dependency))
                {
                    dependencyArray[i] = dependency;
                }
                else
                {
                    dependencies = null;
                    return false;
                }
            }

            dependencies = dependencyArray;
            return true;
        }
    }
}
