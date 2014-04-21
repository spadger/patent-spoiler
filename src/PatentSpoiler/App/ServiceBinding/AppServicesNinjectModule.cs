using System.Configuration;
using Ninject.Infrastructure.Language;
using Ninject.Modules;
using Ninject.Extensions.Conventions;
using Ninject.Web.Common;
using PatentSpoiler.Annotations;
using PatentSpoiler.App.Data;
using PatentSpoiler.App.Import.Config;

namespace PatentSpoiler.App.ServiceBinding
{
    [UsedImplicitly]
    public class AppServicesNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ImporterSettings>().ToMethod(x => (ImporterSettings) (dynamic) ConfigurationManager.GetSection("importerSettings")).InTransientScope();
            Bind<IPatentStoreHierrachy>().To<DictionaryBasedPatentStoreHierrachy>().InSingletonScope();
            Bind<IPatentDatabaseLoader>().To<RavenDBBasedPatentDatabaseLoader>().InSingletonScope();

            Kernel.Bind(x => x.FromAssembliesMatching("PatentSpoiler*")
                .SelectAllClasses()
                .BindDefaultInterface()
                .Configure((cfg, serviceType) =>
                {
                    if (serviceType.HasAttribute<BindAsASingletonAttribute>())
                        cfg.InSingletonScope();
                    else if (serviceType.HasAttribute<BindInRequestScopeAttribute>())
                        cfg.InRequestScope();
                    else
                        cfg.InTransientScope();
                }));
        }
    }
}