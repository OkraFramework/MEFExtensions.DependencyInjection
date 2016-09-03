using System.Composition;

namespace MEFExtensions.DependencyInjection.Tests.TestExports
{
    [Export(typeof(ConventionExport))]
    public class ConventionExport
    {
        public SimpleService ImportedService;

        public ConventionExport(SimpleService importedService)
        {
            ImportedService = importedService;
        }
    }
}