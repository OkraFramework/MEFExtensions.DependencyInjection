using System;
using System.Collections.Generic;
using System.Composition.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace MEFExtensions.DependencyInjection
{
    internal interface IMefServiceCollection : IServiceCollection
    {
        IList<Action<ContainerConfiguration>> MefServiceDescriptors { get; }
        void Add(Action<ContainerConfiguration> item);
    }
}