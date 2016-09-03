using Xunit;

using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace MEFExtensions.DependencyInjection.Tests
{
    public class MefServiceCollectionTests
    {
        [Fact]
        public void Constructor_WithNoParameters_CreatesEmptyCollection()
        {
            var serviceCollection = new MefServiceCollection();

            Assert.Empty(serviceCollection);
            Assert.Empty(serviceCollection.MefServiceDescriptors);
        }

        [Fact]
        public void Constructor_WithServices_CopiesServices()
        {
            ServiceDescriptor service1 = new ServiceDescriptor(typeof(int), 42);
            ServiceDescriptor service2 = new ServiceDescriptor(typeof(string), "Hello");
            ServiceDescriptor service3 = new ServiceDescriptor(typeof(bool), false);
            List<ServiceDescriptor> services = new List<ServiceDescriptor>()
                {
                    service1,
                    service2,
                    service3
                };            

            var serviceCollection = new MefServiceCollection(services);

            Assert.Equal(3, serviceCollection.Count);
            Assert.Equal(service1, serviceCollection[0]);
            Assert.Equal(service2, serviceCollection[1]);
            Assert.Equal(service3, serviceCollection[2]);

            Assert.Empty(serviceCollection.MefServiceDescriptors);
        }
    }
}