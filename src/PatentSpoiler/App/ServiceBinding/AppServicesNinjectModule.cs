using System.Configuration;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Ninject.Infrastructure.Language;
using Ninject.Modules;
using Ninject.Extensions.Conventions;
using Ninject.Web.Common;
using Ninject.Web.Mvc.FilterBindingSyntax;
using PatentSpoiler.Annotations;
using PatentSpoiler.App.Data;
using PatentSpoiler.App.Filters;
using PatentSpoiler.App.Import.Config;
using PatentSpoiler.App.Security;

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

            Bind<IAuthenticationManager>().ToMethod(c => HttpContext.Current.GetOwinContext().Authentication).InRequestScope();
            Bind<PatentSpoilerUserManager>().ToMethod(c =>
            {
                return HttpContext.Current.GetOwinContext().Get<PatentSpoilerUserManager>();
            }).InRequestScope();

            this.BindFilter<PatentCategoryMustExistFilter>(FilterScope.Action, int.MaxValue)
                .WhenActionMethodHas<PatentCategoryMustExistAttribute>()
                .WithConstructorArgumentFromActionAttribute<PatentCategoryMustExistAttribute>("categoryPath", x => x.CategoryPath);
        }
    }
}