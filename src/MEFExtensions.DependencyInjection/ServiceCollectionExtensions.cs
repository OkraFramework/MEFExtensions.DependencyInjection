using Microsoft.Extensions.DependencyInjection;
using MEFExtensions.DependencyInjection.ExportDescriptorProviders;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.Composition.Hosting.Core;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MEFExtensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceProvider BuildServiceProvider(this IServiceCollection services)
        {
            var rootContainer = CreateContainer(services);
            var scopeFactory = rootContainer.GetExport<IServiceScopeFactory>();
            var rootScope = scopeFactory.CreateScope();

            return rootScope.ServiceProvider;
        }

        // *** Private Methods ***

        private static CompositionContext CreateContainer(IEnumerable<ServiceDescriptor> serviceDescriptors)
        {
            List<ExportDescriptorProvider> exportDescriptorProviders = new List<ExportDescriptorProvider>();
            List<ServiceDescriptor> openGenericServiceDescriptors = new List<ServiceDescriptor>();
            
            foreach (var descriptor in serviceDescriptors)
            {
                if (descriptor.ImplementationInstance != null)
                    exportDescriptorProviders.Add(new InstanceExportDescriptorProvider(descriptor));
                else if (descriptor.ImplementationFactory != null)
                    exportDescriptorProviders.Add(new FactoryExportDescriptorProvider(descriptor));
                else if (descriptor.ImplementationType != null)
                {
                    if (descriptor.ServiceType.GetTypeInfo().IsGenericTypeDefinition)
                        openGenericServiceDescriptors.Add(descriptor);
                    else
                        exportDescriptorProviders.Add(new TypeExportDescriptorProvider(descriptor));
                }
                else
                    throw new NotImplementedException();
            }

            var containerConfiguration = new ContainerConfiguration();

            containerConfiguration.WithPart<MefServiceProvider>();
            containerConfiguration.WithPart<MefServiceScopeFactory>();

            // NB: Always include open generic services before closed generic services so the latter is always preferred
            var openGenericExportDescriptorProviders = new[] { new OpenGenericExportDescriptorProvider(openGenericServiceDescriptors) };
            var allProviders = Enumerable.Concat(openGenericExportDescriptorProviders, exportDescriptorProviders);
            containerConfiguration.WithProvider(new AggregateExportDescriptorProvider(allProviders));

            return containerConfiguration.CreateContainer();
        }
    }
}
