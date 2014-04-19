using System.Configuration;
using Ninject.Modules;
using PatentSpoiler.Annotations;
using PatentSpoiler.App.Data;
using PatentSpoiler.App.Import;
using PatentSpoiler.App.Import.Config;

namespace PatentSpoiler.App.NinjectModules
{
    [UsedImplicitly]
    public class AppServicesNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDefinitionImporter>().To<DefinitionImporter>().InTransientScope();
            Bind<IXmlDocumentLoader>().To<XmlDocumentLoader>().InSingletonScope();
            Bind<ImporterSettings>().ToMethod(x => (ImporterSettings) (dynamic) ConfigurationManager.GetSection("importerSettings")).InTransientScope();
            Bind<IPatentStoreHierrachy>().To<DictionaryBasedPatentStoreHierrachy>().InSingletonScope();
            Bind<IPatentDatabaseLoader>().To<RavenDBBasedPatentDatabaseLoader>().InSingletonScope();
        }
    }
}