using System.Configuration;
using Ninject.Modules;
using PatentSpoiler.App.Import;
using PatentSpoiler.App.Import.Config;
using SimpleConfig;

namespace PatentSpoiler.App.NinjectModules
{
    public class AppServicesNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDefinitionImporter>().To<DefinitionImporter>().InTransientScope();
            Bind<IXmlDocumentLoader>().To<XmlDocumentLoader>().InSingletonScope();
            Bind<ImporterSettings>().ToMethod(x =>
            {
                var config = (ConfigMapper) ConfigurationManager.GetSection("importerSettings");
               return (ImporterSettings) (dynamic) ConfigurationManager.GetSection("importerSettings");
            }).
                InTransientScope();
            
        }
    }
}