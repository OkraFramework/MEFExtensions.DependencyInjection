using System.Composition;

namespace MEFExtensions.DependencyInjection.Tests.TestExports
{
    [Export(typeof(SimpleExport))]
    public class SimpleExport
    {
        public SimpleService ImportedService;

        [ImportingConstructor]
        public SimpleExport(SimpleService importedService)
        {
            ImportedService = importedService;
        }
    }
}