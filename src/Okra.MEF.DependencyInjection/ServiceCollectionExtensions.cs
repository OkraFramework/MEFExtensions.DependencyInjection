using Microsoft.Extensions.DependencyInjection;
using Okra.MEF.DependencyInjection.ExportDescriptorProviders;
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

namespace Okra.MEF.DependencyInjection
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
            
            foreach (var descriptor in serviceDescriptors)
            {
                if (descriptor.ImplementationInstance != null)
                    exportDescriptorProviders.Add(new InstanceExportDescriptorProvider(descriptor));
                else if (descriptor.ImplementationFactory != null)
                    exportDescriptorProviders.Add(new FactoryExportDescriptorProvider(descriptor));
                else if (descriptor.ImplementationType != null)
                    exportDescriptorProviders.Add(new TypeExportDescriptorProvider(descriptor));
                else
                    throw new NotImplementedException();
            }

            var containerConfiguration = new ContainerConfiguration();

            containerConfiguration.WithPart<MefServiceProvider>();
            containerConfiguration.WithPart<MefServiceScopeFactory>();

            containerConfiguration.WithProvider(new AggregateExportDescriptorProvider(exportDescriptorProviders));

            return containerConfiguration.CreateContainer();
        }
    }
}
