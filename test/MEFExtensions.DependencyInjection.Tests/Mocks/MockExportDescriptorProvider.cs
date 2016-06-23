using System.Collections.Generic;
using System.Composition.Hosting.Core;
using MEFExtensions.DependencyInjection.Tests.TestExports;

namespace MEFExtensions.DependencyInjection.Tests.Mocks
{
    public class MockExportDescriptorProvider : ExportDescriptorProvider
    {
        public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors(CompositionContract contract, DependencyAccessor descriptorAccessor)
        {
            if (contract.ContractType != typeof(SimpleService))
                return NoExportDescriptors;

            return new[] { new ExportDescriptorPromise(contract, "MockExportDescriptorProvider", true, NoDependencies, _ =>
                    ExportDescriptor.Create((c, o) => new SimpleService(), NoMetadata)) };
        }
    }
}