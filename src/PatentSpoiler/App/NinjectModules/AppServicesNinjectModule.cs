using Ninject.Modules;
using PatentSpoiler.App.Import;

namespace PatentSpoiler.App.NinjectModules
{
    public class AppServicesNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDefinitionImporter>().To<DefinitionImporter>().InTransientScope();
        }
    }
}