using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using Raven.Client;
using Raven.Client.Document;

namespace PatentSpoiler.App.NinjectModules
{
    public class RavenDBNinjectModule : NinjectModule
    {
        public override void Load()
        { 
            Bind<IDocumentStore>()
           .ToMethod(context =>
           {
               var documentStore = new DocumentStore { ConnectionStringName = "PatentSpoiler", };
               return documentStore.Initialize();
           })
           .InSingletonScope();

            Bind<IDocumentSession>().ToMethod(context => context.Kernel.Get<IDocumentStore>().OpenSession()).InRequestScope();
        }
    }
}