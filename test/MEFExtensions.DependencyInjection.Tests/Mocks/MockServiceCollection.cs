using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace MEFExtensions.DependencyInjection.Tests.Mocks
{
    public class MockServiceCollection : List<ServiceDescriptor>, IServiceCollection
    {
    }
}