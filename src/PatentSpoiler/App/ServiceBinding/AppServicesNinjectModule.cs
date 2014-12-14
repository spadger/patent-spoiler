using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Nest;
using Ninject;
using Ninject.Activation;
using Ninject.Infrastructure.Language;
using Ninject.Modules;
using Ninject.Extensions.Conventions;
using Ninject.Web.Common;
using Ninject.Web.Mvc.FilterBindingSyntax;
using PatentSpoiler.Annotations;
using PatentSpoiler.App.Attachments;
using PatentSpoiler.App.Data;
using PatentSpoiler.App.Domain.Security;
using PatentSpoiler.App.Filters;
using PatentSpoiler.App.Import.Config;
using PatentSpoiler.App.Security;
using Raven.Client;

namespace PatentSpoiler.App.ServiceBinding
{
    [UsedImplicitly]
    public class AppServicesNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IImporterSettings>().ToMethod(x =>
            {
                IImporterSettings result = (dynamic)ConfigurationManager.GetSection("importer");
                return result;
            }).InTransientScope();
            Bind<IAttachmentStagingSettings>().ToMethod(x =>
            {
                IAttachmentStagingSettings result = (dynamic)ConfigurationManager.GetSection("attachmentStaging");
                return result;
            }).InTransientScope();
            Bind<IPatentStoreHierrachy>().To<DictionaryBasedPatentStoreHierrachy>().InSingletonScope();

            var blobStorageConnectionString = ConfigurationManager.ConnectionStrings["BlobStorage"].ConnectionString;
            var storageAccount = CloudStorageAccount.Parse(blobStorageConnectionString);
            Bind<CloudBlobClient>().ToMethod(x => storageAccount.CreateCloudBlobClient()).InTransientScope();
            
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
            
            Bind<PatentSpoilerUser>().ToMethod(GetUser);

            this.BindFilter<PatentCategoryMustExistFilter>(FilterScope.Action, int.MaxValue)
                .WhenActionMethodHas<PatentCategoryMustExistAttribute>()
                .WithConstructorArgumentFromActionAttribute<PatentCategoryMustExistAttribute>("categoryPath", x => x.CategoryPath)
                .WithConstructorArgumentFromActionAttribute<PatentCategoryMustExistAttribute>("isOptional", x => x.IsOptional);

            var node = new Uri(ConfigurationManager.ConnectionStrings["ElasticSearch"].ConnectionString);
            var settings = new ConnectionSettings(node);
            Bind<IElasticClient>().ToMethod(x => new ElasticClient(settings)).InRequestScope();
        }

        private PatentSpoilerUser GetUser(IContext context)
        {
            var identity = HttpContext.Current.User.Identity;
            if (!identity.IsAuthenticated)
            {
                return null;
            }

            return Kernel.Get<IDocumentSession>().Load<PatentSpoilerUser>(identity.Name);
        }
    }
}