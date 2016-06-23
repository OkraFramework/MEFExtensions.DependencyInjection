using System;
using System.Collections.Generic;
using System.Composition.Convention;
using System.Composition.Hosting.Core;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MEFExtensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection WithProvider(this IServiceCollection services, ExportDescriptorProvider exportDescriptorProvider)
        {
            if (services ==null)
                throw new ArgumentNullException(nameof(services));
            
            if (exportDescriptorProvider == null)
                throw new ArgumentNullException(nameof(exportDescriptorProvider));

            if (!(services is IMefServiceCollection))
                throw new ArgumentException(Resources.CannotAddNativeMefImports, nameof(services));

            ((IMefServiceCollection)services).Add(c => c.WithProvider(exportDescriptorProvider));
            return services;
        }

        public static IServiceCollection WithDefaultConventions(this IServiceCollection services, AttributedModelProvider conventions)
        {
            if (services ==null)
                throw new ArgumentNullException(nameof(services));
            
            if (conventions == null)
                throw new ArgumentNullException(nameof(conventions));

            if (!(services is IMefServiceCollection))
                throw new ArgumentException(Resources.CannotAddNativeMefImports, nameof(services));

            ((IMefServiceCollection)services).Add(c => c.WithDefaultConventions(conventions));
            return services;
        }

        public static IServiceCollection WithPart(this IServiceCollection services, Type partType)
        {
            return WithPart(services, partType, null);
        }

        public static IServiceCollection WithPart(this IServiceCollection services, Type partType, AttributedModelProvider conventions)
        {
            if (services ==null)
                throw new ArgumentNullException(nameof(services));
            
            if (partType == null)
                throw new ArgumentNullException(nameof(partType));
            
            if (!(services is IMefServiceCollection))
                throw new ArgumentException(Resources.CannotAddNativeMefImports, nameof(services));

            ((IMefServiceCollection)services).Add(c => c.WithPart(partType, conventions));
            return services;
        }

        public static IServiceCollection WithPart<TPart>(this IServiceCollection services)
        {
            return WithPart<TPart>(services, null);
        }

        public static IServiceCollection WithPart<TPart>(this IServiceCollection services, AttributedModelProvider conventions)
        {
            return WithPart(services, typeof(TPart), conventions);
        }

        public static IServiceCollection WithParts(this IServiceCollection services, params Type[] partTypes)
        {
            return WithParts(services, (IEnumerable<Type>)partTypes);
        }

        public static IServiceCollection WithParts(this IServiceCollection services, IEnumerable<Type> partTypes)
        {
            return WithParts(services, partTypes, null);
        }

        public static IServiceCollection WithParts(this IServiceCollection services, IEnumerable<Type> partTypes, AttributedModelProvider conventions)
        {
            if (services ==null)
                throw new ArgumentNullException(nameof(services));
            
            if (partTypes == null)
                throw new ArgumentNullException(nameof(partTypes));

            if (!(services is IMefServiceCollection))
                throw new ArgumentException(Resources.CannotAddNativeMefImports, nameof(services));

            ((IMefServiceCollection)services).Add(c => c.WithParts(partTypes, conventions));
            return services;
        }

        public static IServiceCollection WithAssembly(this IServiceCollection services, Assembly assembly)
        {            
            return WithAssembly(services, assembly, null);
        }

        public static IServiceCollection WithAssembly(this IServiceCollection services, Assembly assembly, AttributedModelProvider conventions)
        {
            if (services ==null)
                throw new ArgumentNullException(nameof(services));
            
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            if (!(services is IMefServiceCollection))
                throw new ArgumentException(Resources.CannotAddNativeMefImports, nameof(services));
            
            ((IMefServiceCollection)services).Add(c => c.WithAssembly(assembly, conventions));
            return services;
        }

        public static IServiceCollection WithAssemblies(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            return WithAssemblies(services, assemblies, null);
        }

        public static IServiceCollection WithAssemblies(this IServiceCollection services, IEnumerable<Assembly> assemblies, AttributedModelProvider conventions)
        {
            if (services ==null)
                throw new ArgumentNullException(nameof(services));
            
            if (assemblies == null)
                throw new ArgumentNullException(nameof(assemblies));

            if (!(services is IMefServiceCollection))
                throw new ArgumentException(Resources.CannotAddNativeMefImports, nameof(services));

            ((IMefServiceCollection)services).Add(c => c.WithAssemblies(assemblies, conventions));
            return services;
        }
    }
}