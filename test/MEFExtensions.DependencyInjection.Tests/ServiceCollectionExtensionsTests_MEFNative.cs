using Xunit;

using Microsoft.Extensions.DependencyInjection;
using MEFExtensions.DependencyInjection;
using MEFExtensions.DependencyInjection.Tests.TestExports;
using MEFExtensions.DependencyInjection.Tests.Mocks;
using System;
using System.Composition.Convention;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace MEFExtensions.DependencyInjection.Tests
{
    public class ServiceCollectionExtensionsTests_MEFNative
    {
        [Fact]
        public void WithProvider_AddsPartToContainer()
        {
            var serviceCollection = new ServiceCollection();

            var exportDescriptorProvider = new MockExportDescriptorProvider();
            serviceCollection.WithProvider(exportDescriptorProvider);

            var provider = serviceCollection.BuildServiceProvider();
            var export = provider.GetService<SimpleService>();

            Assert.NotNull(export);
            Assert.IsType<SimpleService>(export);
        }

        [Fact]
        public void WithProvider_ThrowsExceptionWithNullProvider()
        {
            var serviceCollection = new ServiceCollection();

            Assert.Throws<ArgumentNullException>("exportDescriptorProvider", () => serviceCollection.WithProvider(null));
        }

        [Fact]
        public void WithDefaultConventions_AddsPartToContainer()
        {
            var serviceCollection = new ServiceCollection();

            var conventionBuilder = new ConventionBuilder();
            conventionBuilder.ForTypesMatching(type => type == typeof(ConventionExport))
                             .Export<ConventionExport>()
                             .SelectConstructor(ctorInfos => ctorInfos.First());

            serviceCollection.WithDefaultConventions(conventionBuilder);
            serviceCollection.WithPart(typeof(SimpleService));
            serviceCollection.WithPart(typeof(ConventionExport));

            var provider = serviceCollection.BuildServiceProvider();
            var export = provider.GetService<ConventionExport>();

            Assert.NotNull(export);
            Assert.NotNull(export.ImportedService);
            Assert.IsType<ConventionExport>(export);
            Assert.IsType<SimpleService>(export.ImportedService);
        }

        [Fact]
        public void WithDefaultConventions_ThrowsExceptionWithNullConventions()
        {
            var serviceCollection = new ServiceCollection();

            Assert.Throws<ArgumentNullException>("conventions", () => serviceCollection.WithDefaultConventions(null));
        }

        [Fact]
        public void WithPart_ByType_AddsPartToContainer()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.WithPart(typeof(SimpleService));
            serviceCollection.WithPart(typeof(SimpleExport));

            var provider = serviceCollection.BuildServiceProvider();
            var export = provider.GetService<SimpleExport>();

            Assert.NotNull(export);
            Assert.NotNull(export.ImportedService);
            Assert.IsType<SimpleExport>(export);
            Assert.IsType<SimpleService>(export.ImportedService);
        }

        [Fact]
        public void WithPart_ByType_ThrowsExceptionWithNullPartType()
        {
            var serviceCollection = new ServiceCollection();

            Assert.Throws<ArgumentNullException>("partType", () => serviceCollection.WithPart(null));
        }

        [Fact]
        public void WithPart_ByTypeAndConventions_AddsPartToContainer()
        {
            var serviceCollection = new ServiceCollection();

            var conventionBuilder = new ConventionBuilder();
            conventionBuilder.ForTypesMatching(type => type == typeof(ConventionExport))
                             .Export<ConventionExport>()
                             .SelectConstructor(ctorInfos => ctorInfos.First());

            serviceCollection.WithPart(typeof(SimpleService));
            serviceCollection.WithPart(typeof(ConventionExport), conventionBuilder);

            var provider = serviceCollection.BuildServiceProvider();
            var export = provider.GetService<ConventionExport>();

            Assert.NotNull(export);
            Assert.NotNull(export.ImportedService);
            Assert.IsType<ConventionExport>(export);
            Assert.IsType<SimpleService>(export.ImportedService);
        }

        [Fact]
        public void WithPart_ByTypeAndConventions_ThrowsExceptionWithNullPartType()
        {
            var serviceCollection = new ServiceCollection();
            var conventionBuilder = new ConventionBuilder();

            Assert.Throws<ArgumentNullException>("partType", () => serviceCollection.WithPart(null, conventionBuilder));
        }

        [Fact]
        public void WithPart_ByGenericType_AddsPartToContainer()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.WithPart<SimpleService>();
            serviceCollection.WithPart<SimpleExport>();

            var provider = serviceCollection.BuildServiceProvider();
            var export = provider.GetService<SimpleExport>();

            Assert.NotNull(export);
            Assert.NotNull(export.ImportedService);
            Assert.IsType<SimpleExport>(export);
            Assert.IsType<SimpleService>(export.ImportedService);
        }

        [Fact]
        public void WithPart_ByGenericTypeAndConventions_AddsPartToContainer()
        {
            var serviceCollection = new ServiceCollection();

            var conventionBuilder = new ConventionBuilder();
            conventionBuilder.ForTypesMatching(type => type == typeof(ConventionExport))
                             .Export<ConventionExport>()
                             .SelectConstructor(ctorInfos => ctorInfos.First());

            serviceCollection.WithPart<SimpleService>();
            serviceCollection.WithPart<ConventionExport>(conventionBuilder);

            var provider = serviceCollection.BuildServiceProvider();
            var export = provider.GetService<ConventionExport>();

            Assert.NotNull(export);
            Assert.NotNull(export.ImportedService);
            Assert.IsType<ConventionExport>(export);
            Assert.IsType<SimpleService>(export.ImportedService);
        }

        [Fact]
        public void WithParts_ByParamsArray_AddsPartToContainer()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.WithParts(typeof(SimpleService), typeof(SimpleExport));

            var provider = serviceCollection.BuildServiceProvider();
            var export = provider.GetService<SimpleExport>();

            Assert.NotNull(export);
            Assert.NotNull(export.ImportedService);
            Assert.IsType<SimpleExport>(export);
            Assert.IsType<SimpleService>(export.ImportedService);
        }

        [Fact]
        public void WithParts_ByParamsArray_ThrowsExceptionWithNullPartTypes()
        {
            var serviceCollection = new ServiceCollection();

            Assert.Throws<ArgumentNullException>("partTypes", () => serviceCollection.WithParts((Type[])null));
        }

        [Fact]
        public void WithParts_ByEnumerable_AddsPartToContainer()
        {
            var serviceCollection = new ServiceCollection();

            IEnumerable<Type> parts = new Type[] {typeof(SimpleService), typeof(SimpleExport)};
            serviceCollection.WithParts(parts);

            var provider = serviceCollection.BuildServiceProvider();
            var export = provider.GetService<SimpleExport>();

            Assert.NotNull(export);
            Assert.NotNull(export.ImportedService);
            Assert.IsType<SimpleExport>(export);
            Assert.IsType<SimpleService>(export.ImportedService);
        }

        [Fact]
        public void WithParts_ByEnumerable_ThrowsExceptionWithNullPartTypes()
        {
            var serviceCollection = new ServiceCollection();

            Assert.Throws<ArgumentNullException>("partTypes", () => serviceCollection.WithParts((IEnumerable<Type>)null));
        }

        [Fact]
        public void WithParts_ByEnumerableAndConventions_AddsPartToContainer()
        {
            var serviceCollection = new ServiceCollection();

            var conventionBuilder = new ConventionBuilder();
            conventionBuilder.ForTypesMatching(type => type == typeof(ConventionExport))
                             .Export<ConventionExport>()
                             .SelectConstructor(ctorInfos => ctorInfos.First());

            IEnumerable<Type> parts = new Type[] {typeof(SimpleService), typeof(ConventionExport)};
            serviceCollection.WithParts(parts, conventionBuilder);

            var provider = serviceCollection.BuildServiceProvider();
            var export = provider.GetService<ConventionExport>();

            Assert.NotNull(export);
            Assert.NotNull(export.ImportedService);
            Assert.IsType<ConventionExport>(export);
            Assert.IsType<SimpleService>(export.ImportedService);
        }

        [Fact]
        public void WithParts_ByEnumerableAndConventions_ThrowsExceptionWithNullPartTypes()
        {
            var serviceCollection = new ServiceCollection();
            var conventionBuilder = new ConventionBuilder();

            Assert.Throws<ArgumentNullException>("partTypes", () => serviceCollection.WithParts((IEnumerable<Type>)null, conventionBuilder));
        }

        [Fact]
        public void WithAssembly_ByAssembly_AddsPartToContainer()
        {
            var serviceCollection = new ServiceCollection();

            var assembly = typeof(SimpleService).GetTypeInfo().Assembly;
            serviceCollection.WithAssembly(assembly);

            var provider = serviceCollection.BuildServiceProvider();
            var export = provider.GetService<SimpleExport>();

            Assert.NotNull(export);
            Assert.NotNull(export.ImportedService);
            Assert.IsType<SimpleExport>(export);
            Assert.IsType<SimpleService>(export.ImportedService);
        }

        [Fact]
        public void WithAssembly_ByAssembly_ThrowsExceptionWithNullPartTypes()
        {
            var serviceCollection = new ServiceCollection();

            Assert.Throws<ArgumentNullException>("assembly", () => serviceCollection.WithAssembly(null));
        }

        [Fact]
        public void WithAssembly_ByAssemblyAndConventions_AddsPartToContainer()
        {
            var serviceCollection = new ServiceCollection();

            var conventionBuilder = new ConventionBuilder();
            conventionBuilder.ForTypesMatching(type => type == typeof(ConventionExport))
                             .Export<ConventionExport>()
                             .SelectConstructor(ctorInfos => ctorInfos.First());

            var assembly = typeof(SimpleService).GetTypeInfo().Assembly;
            serviceCollection.WithAssembly(assembly, conventionBuilder);

            var provider = serviceCollection.BuildServiceProvider();
            var export = provider.GetService<ConventionExport>();

            Assert.NotNull(export);
            Assert.NotNull(export.ImportedService);
            Assert.IsType<ConventionExport>(export);
            Assert.IsType<SimpleService>(export.ImportedService);
        }

        [Fact]
        public void WithAssembly_ByAssemblyAndConventions_ThrowsExceptionWithNullPartTypes()
        {
            var serviceCollection = new ServiceCollection();
            var conventionBuilder = new ConventionBuilder();

            Assert.Throws<ArgumentNullException>("assembly", () => serviceCollection.WithAssembly(null, conventionBuilder));
        }

        [Fact]
        public void WithAssemblies_ByAssembly_AddsPartToContainer()
        {
            var serviceCollection = new ServiceCollection();

            var assemblies = new[] { typeof(SimpleService).GetTypeInfo().Assembly };
            serviceCollection.WithAssemblies(assemblies);

            var provider = serviceCollection.BuildServiceProvider();
            var export = provider.GetService<SimpleExport>();

            Assert.NotNull(export);
            Assert.NotNull(export.ImportedService);
            Assert.IsType<SimpleExport>(export);
            Assert.IsType<SimpleService>(export.ImportedService);
        }

        [Fact]
        public void WithAssemblies_ByAssembly_ThrowsExceptionWithNullPartTypes()
        {
            var serviceCollection = new ServiceCollection();

            Assert.Throws<ArgumentNullException>("assemblies", () => serviceCollection.WithAssemblies(null));
        }

        [Fact]
        public void WithAssemblies_ByAssemblyAndConventions_AddsPartToContainer()
        {
            var serviceCollection = new ServiceCollection();

            var conventionBuilder = new ConventionBuilder();
            conventionBuilder.ForTypesMatching(type => type == typeof(ConventionExport))
                             .Export<ConventionExport>()
                             .SelectConstructor(ctorInfos => ctorInfos.First());

            var assemblies = new[] { typeof(SimpleService).GetTypeInfo().Assembly };
            serviceCollection.WithAssemblies(assemblies, conventionBuilder);

            var provider = serviceCollection.BuildServiceProvider();
            var export = provider.GetService<ConventionExport>();

            Assert.NotNull(export);
            Assert.NotNull(export.ImportedService);
            Assert.IsType<ConventionExport>(export);
            Assert.IsType<SimpleService>(export.ImportedService);
        }

        [Fact]
        public void WithAssemblies_ByAssemblyAndConventions_ThrowsExceptionWithNullPartTypes()
        {
            var serviceCollection = new ServiceCollection();
            var conventionBuilder = new ConventionBuilder();

            Assert.Throws<ArgumentNullException>("assemblies", () => serviceCollection.WithAssemblies(null, conventionBuilder));
        }

        // *** ReturnsSameServiceCollection Tests ***

        [Fact]
        public void WithProvider_ReturnsSameServiceCollection()
        {
            var exportDescriptorProvider = new MockExportDescriptorProvider();
            Assert_ReturnsSameServiceCollection(serviceCollection => serviceCollection.WithProvider(exportDescriptorProvider));
        }

        [Fact]
        public void WithDefaultConventions_ReturnsSameServiceCollection()
        {
            var conventionBuilder = new ConventionBuilder();
            Assert_ReturnsSameServiceCollection(serviceCollection => serviceCollection.WithDefaultConventions(conventionBuilder));
        }

        [Fact]
        public void WithPart_ByType_ReturnsSameServiceCollection()
        {
            Assert_ReturnsSameServiceCollection(serviceCollection => serviceCollection.WithPart(typeof(SimpleExport)));
        }

        [Fact]
        public void WithPart_ByTypeAndConventions_ReturnsSameServiceCollection()
        {
            var conventionBuilder = new ConventionBuilder();
            Assert_ReturnsSameServiceCollection(serviceCollection => serviceCollection.WithPart(typeof(SimpleExport), conventionBuilder));
        }

        [Fact]
        public void WithPart_ByGenericType_ReturnsSameServiceCollection()
        {
            Assert_ReturnsSameServiceCollection(serviceCollection => serviceCollection.WithPart<SimpleExport>());
        }

        [Fact]
        public void WithPart_ByGenericTypeAndConventions_ReturnsSameServiceCollection()
        {
            var conventionBuilder = new ConventionBuilder();
            Assert_ReturnsSameServiceCollection(serviceCollection => serviceCollection.WithPart<SimpleExport>(conventionBuilder));
        }

        [Fact]
        public void WithParts_ByParamsArray_ReturnsSameServiceCollection()
        {
            Assert_ReturnsSameServiceCollection(serviceCollection => serviceCollection.WithParts(typeof(SimpleService), typeof(SimpleExport)));
        }

        [Fact]
        public void WithParts_ByEnumerable_ReturnsSameServiceCollection()
        {
            IEnumerable<Type> parts = new Type[] {typeof(SimpleService), typeof(SimpleExport)};
            Assert_ReturnsSameServiceCollection(serviceCollection => serviceCollection.WithParts(parts));
        }

        [Fact]
        public void WithParts_ByEnumerableAndConventions_ReturnsSameServiceCollection()
        {
            var conventionBuilder = new ConventionBuilder();
            IEnumerable<Type> parts = new Type[] {typeof(SimpleService), typeof(SimpleExport)};
            Assert_ReturnsSameServiceCollection(serviceCollection => serviceCollection.WithParts(parts, conventionBuilder));
        }

        [Fact]
        public void WithAssembly_ByAssembly_ReturnsSameServiceCollection()
        {
            var assembly = typeof(SimpleService).GetTypeInfo().Assembly;
            Assert_ReturnsSameServiceCollection(serviceCollection => serviceCollection.WithAssembly(assembly));
        }

        [Fact]
        public void WithAssembly_ByAssemblyAndConventions_ReturnsSameServiceCollection()
        {
            var conventionBuilder = new ConventionBuilder();
            var assembly = typeof(SimpleService).GetTypeInfo().Assembly;
            Assert_ReturnsSameServiceCollection(serviceCollection => serviceCollection.WithAssembly(assembly, conventionBuilder));
        }

        [Fact]
        public void WithAssemblies_ByAssembly_ReturnsSameServiceCollection()
        {
            var assemblies = new[] { typeof(SimpleService).GetTypeInfo().Assembly };
            Assert_ReturnsSameServiceCollection(serviceCollection => serviceCollection.WithAssemblies(assemblies));
        }

        [Fact]
        public void WithAssemblies_ByAssemblyAndConventions_ReturnsSameServiceCollection()
        {
            var conventionBuilder = new ConventionBuilder();
            var assemblies = new[] { typeof(SimpleService).GetTypeInfo().Assembly };
            Assert_ReturnsSameServiceCollection(serviceCollection => serviceCollection.WithAssemblies(assemblies, conventionBuilder));
        }

        // *** ThrowsExceptionIfNotMefServiceCollection Tests ***

        [Fact]
        public void WithProvider_ThrowsExceptionIfNotMefServiceCollection()
        {
            var exportDescriptorProvider = new MockExportDescriptorProvider();
            Assert_NotMefServiceException(serviceCollection => serviceCollection.WithProvider(exportDescriptorProvider));
        }

        [Fact]
        public void WithDefaultConventions_ThrowsExceptionIfNotMefServiceCollection()
        {
            var conventionBuilder = new ConventionBuilder();
            Assert_NotMefServiceException(serviceCollection => serviceCollection.WithDefaultConventions(conventionBuilder));
        }

        [Fact]
        public void WithPart_ByType_ThrowsExceptionIfNotMefServiceCollection()
        {
            Assert_NotMefServiceException(serviceCollection => serviceCollection.WithPart(typeof(SimpleExport)));
        }

        [Fact]
        public void WithPart_ByTypeAndConventions_ThrowsExceptionIfNotMefServiceCollection()
        {
            var conventionBuilder = new ConventionBuilder();
            Assert_NotMefServiceException(serviceCollection => serviceCollection.WithPart(typeof(SimpleExport), conventionBuilder));
        }

        [Fact]
        public void WithPart_ByGenericType_ThrowsExceptionIfNotMefServiceCollection()
        {
            Assert_NotMefServiceException(serviceCollection => serviceCollection.WithPart<SimpleExport>());
        }

        [Fact]
        public void WithPart_ByGenericTypeAndConventions_ThrowsExceptionIfNotMefServiceCollection()
        {
            var conventionBuilder = new ConventionBuilder();
            Assert_NotMefServiceException(serviceCollection => serviceCollection.WithPart<SimpleExport>(conventionBuilder));
        }

        [Fact]
        public void WithParts_ByParamsArray_ThrowsExceptionIfNotMefServiceCollection()
        {
            Assert_NotMefServiceException(serviceCollection => serviceCollection.WithParts(typeof(SimpleService), typeof(SimpleExport)));
        }

        [Fact]
        public void WithParts_ByEnumerable_ThrowsExceptionIfNotMefServiceCollection()
        {
            IEnumerable<Type> parts = new Type[] {typeof(SimpleService), typeof(SimpleExport)};
            Assert_NotMefServiceException(serviceCollection => serviceCollection.WithParts(parts));
        }

        [Fact]
        public void WithParts_ByEnumerableAndConventions_ThrowsExceptionIfNotMefServiceCollection()
        {
            var conventionBuilder = new ConventionBuilder();
            IEnumerable<Type> parts = new Type[] {typeof(SimpleService), typeof(SimpleExport)};
            Assert_NotMefServiceException(serviceCollection => serviceCollection.WithParts(parts, conventionBuilder));
        }

        [Fact]
        public void WithAssembly_ByAssembly_ThrowsExceptionIfNotMefServiceCollection()
        {
            var assembly = typeof(SimpleService).GetTypeInfo().Assembly;
            Assert_NotMefServiceException(serviceCollection => serviceCollection.WithAssembly(assembly));
        }

        [Fact]
        public void WithAssembly_ByAssemblyAndConventions_ThrowsExceptionIfNotMefServiceCollection()
        {
            var conventionBuilder = new ConventionBuilder();
            var assembly = typeof(SimpleService).GetTypeInfo().Assembly;
            Assert_NotMefServiceException(serviceCollection => serviceCollection.WithAssembly(assembly, conventionBuilder));
        }

        [Fact]
        public void WithAssemblies_ByAssembly_ThrowsExceptionIfNotMefServiceCollection()
        {
            var assemblies = new[] { typeof(SimpleService).GetTypeInfo().Assembly };
            Assert_NotMefServiceException(serviceCollection => serviceCollection.WithAssemblies(assemblies));
        }

        [Fact]
        public void WithAssemblies_ByAssemblyAndConventions_ThrowsExceptionIfNotMefServiceCollection()
        {
            var conventionBuilder = new ConventionBuilder();
            var assemblies = new[] { typeof(SimpleService).GetTypeInfo().Assembly };
            Assert_NotMefServiceException(serviceCollection => serviceCollection.WithAssemblies(assemblies, conventionBuilder));
        }

        // *** Private Methods ***

        private void Assert_ReturnsSameServiceCollection(Func<IServiceCollection, IServiceCollection> testCode)
        {
            var serviceCollection = new ServiceCollection();

            var result = testCode(serviceCollection);

            Assert.Equal(serviceCollection, result);
        }

        private void Assert_NotMefServiceException(Func<IServiceCollection, object> testCode)
        {
            var serviceCollection = new MockServiceCollection();

            Assert.Throws<ArgumentNullException>("services", () => testCode(null));

            var e = Assert.Throws<ArgumentException>(() => testCode(serviceCollection));

            Assert.Equal("services", e.ParamName);
            Assert.Equal("You can only add native MEF exports to an instance of MEFExtensions.DependencyInjection.ServiceCollection.\r\nParameter name: services", e.Message);
        }
    }
}