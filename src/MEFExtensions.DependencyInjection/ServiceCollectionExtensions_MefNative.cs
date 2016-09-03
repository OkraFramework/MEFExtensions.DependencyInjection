using System;
using System.Collections.Generic;
using System.Composition.Convention;
using System.Composition.Hosting.Core;
using System.Reflection;

namespace MEFExtensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IMefServiceCollection WithProvider(this IMefServiceCollection services, ExportDescriptorProvider exportDescriptorProvider)
        {
            if (services ==null)
                throw new ArgumentNullException(nameof(services));
            
            if (exportDescriptorProvider == null)
                throw new ArgumentNullException(nameof(exportDescriptorProvider));

            services.Add(c => c.WithProvider(exportDescriptorProvider));
            return services;
        }

        public static IMefServiceCollection WithDefaultConventions(this IMefServiceCollection services, AttributedModelProvider conventions)
        {
            if (services ==null)
                throw new ArgumentNullException(nameof(services));
            
            if (conventions == null)
                throw new ArgumentNullException(nameof(conventions));

            services.Add(c => c.WithDefaultConventions(conventions));
            return services;
        }

        public static IMefServiceCollection WithPart(this IMefServiceCollection services, Type partType)
        {
            return WithPart(services, partType, null);
        }

        public static IMefServiceCollection WithPart(this IMefServiceCollection services, Type partType, AttributedModelProvider conventions)
        {
            if (services ==null)
                throw new ArgumentNullException(nameof(services));
            
            if (partType == null)
                throw new ArgumentNullException(nameof(partType));

            services.Add(c => c.WithPart(partType, conventions));
            return services;
        }

        public static IMefServiceCollection WithPart<TPart>(this IMefServiceCollection services)
        {
            return WithPart<TPart>(services, null);
        }

        public static IMefServiceCollection WithPart<TPart>(this IMefServiceCollection services, AttributedModelProvider conventions)
        {
            return WithPart(services, typeof(TPart), conventions);
        }

        public static IMefServiceCollection WithParts(this IMefServiceCollection services, params Type[] partTypes)
        {
            return WithParts(services, (IEnumerable<Type>)partTypes);
        }

        public static IMefServiceCollection WithParts(this IMefServiceCollection services, IEnumerable<Type> partTypes)
        {
            return WithParts(services, partTypes, null);
        }

        public static IMefServiceCollection WithParts(this IMefServiceCollection services, IEnumerable<Type> partTypes, AttributedModelProvider conventions)
        {
            if (services ==null)
                throw new ArgumentNullException(nameof(services));
            
            if (partTypes == null)
                throw new ArgumentNullException(nameof(partTypes));

            services.Add(c => c.WithParts(partTypes, conventions));
            return services;
        }

        public static IMefServiceCollection WithAssembly(this IMefServiceCollection services, Assembly assembly)
        {            
            return WithAssembly(services, assembly, null);
        }

        public static IMefServiceCollection WithAssembly(this IMefServiceCollection services, Assembly assembly, AttributedModelProvider conventions)
        {
            if (services ==null)
                throw new ArgumentNullException(nameof(services));
            
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));
            
            services.Add(c => c.WithAssembly(assembly, conventions));
            return services;
        }

        public static IMefServiceCollection WithAssemblies(this IMefServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            return WithAssemblies(services, assemblies, null);
        }

        public static IMefServiceCollection WithAssemblies(this IMefServiceCollection services, IEnumerable<Assembly> assemblies, AttributedModelProvider conventions)
        {
            if (services ==null)
                throw new ArgumentNullException(nameof(services));
            
            if (assemblies == null)
                throw new ArgumentNullException(nameof(assemblies));

            services.Add(c => c.WithAssemblies(assemblies, conventions));
            return services;
        }
    }
}