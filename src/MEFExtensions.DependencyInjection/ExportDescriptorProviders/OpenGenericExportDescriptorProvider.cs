using Microsoft.Extensions.DependencyInjection;
using MEFExtensions.DependencyInjection.Util;
using System;
using System.Collections.Generic;
using System.Composition.Hosting.Core;
using System.Linq;
using System.Reflection;

namespace MEFExtensions.DependencyInjection.ExportDescriptorProviders
{
    public class OpenGenericExportDescriptorProvider : ExportDescriptorProvider
    {
        private ServiceDescriptor[] _serviceDescriptors;

        private static readonly MethodInfo s_getTypedDescriptorMethod = typeof(TypeExportDescriptorProvider).GetTypeInfo().GetDeclaredMethod(nameof(TypeExportDescriptorProvider.GetTypedDescriptor));

        public OpenGenericExportDescriptorProvider(IEnumerable<ServiceDescriptor> serviceDescriptors)
        {
            foreach (ServiceDescriptor serviceDescriptor in serviceDescriptors)
            {
                TypeInfo serviceType = serviceDescriptor.ServiceType.GetTypeInfo();
                TypeInfo implementationType = serviceDescriptor.ImplementationType.GetTypeInfo();

                if (implementationType.IsAbstract ||
                    implementationType.IsInterface ||
                    !implementationType.IsGenericTypeDefinition ||
                    !serviceType.IsGenericTypeDefinition)
                {
                    throw new ArgumentException(string.Format(Resources.TypeCannotBeActivated, serviceDescriptor.ImplementationType, serviceDescriptor.ServiceType));
                }
            }

            _serviceDescriptors = serviceDescriptors.ToArray();
        }

        public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors(CompositionContract contract, DependencyAccessor descriptorAccessor)
        {
            return _serviceDescriptors.Select(sd => GetExportDescriptorPromise(sd, contract, descriptorAccessor))
                                      .Where(edp => edp != null);
        }

        private ExportDescriptorPromise GetExportDescriptorPromise(ServiceDescriptor serviceDescriptor, CompositionContract contract, DependencyAccessor descriptorAccessor)
        {
            if (!contract.ContractType.GetTypeInfo().IsGenericType || contract.ContractType.GetGenericTypeDefinition() != serviceDescriptor.ServiceType)
                return null;

            var genericArguments = contract.ContractType.GetTypeInfo().GenericTypeArguments;
            var implementationType = serviceDescriptor.ImplementationType.MakeGenericType(genericArguments);
            var implementationContract = contract.ChangeType(implementationType);

            var getDescriptorMethod = s_getTypedDescriptorMethod.MakeGenericMethod(contract.ContractType);
            var getDescriptorDelegate = getDescriptorMethod.CreateStaticDelegate<Func<CompositionContract, CompositionContract, ServiceLifetime, DependencyAccessor, object>>();

            return (ExportDescriptorPromise)getDescriptorDelegate(contract, implementationContract, serviceDescriptor.Lifetime, descriptorAccessor);
        }
    }
}
