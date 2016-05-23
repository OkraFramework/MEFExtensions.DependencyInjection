﻿using Microsoft.Extensions.DependencyInjection;
using Okra.MEF.DependencyInjection.ExportDescriptorProviders;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Convention;
using System.Composition.Hosting;
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
            var containerConfiguration = new ContainerConfiguration();

            foreach (var descriptor in serviceDescriptors)
            {
                if (descriptor.ImplementationInstance != null)
                    containerConfiguration.WithProvider(new InstanceExportDescriptorProvider(descriptor));
                else if (descriptor.ImplementationFactory != null)
                    containerConfiguration.WithProvider(new FactoryExportDescriptorProvider(descriptor));
                else if (descriptor.ImplementationType != null)
                    containerConfiguration.WithProvider(new TypeExportDescriptorProvider(descriptor));
                else
                    throw new NotImplementedException();
            }

            containerConfiguration.WithPart<MefServiceProvider>();
            containerConfiguration.WithPart<MefServiceScopeFactory>();

            containerConfiguration.WithProvider(new EnumerableExportDescriptorProvider());

            return containerConfiguration.CreateContainer();
        }
    }
}
