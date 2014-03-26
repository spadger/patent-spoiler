using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using PatentSpoiler.App.Database.RavenDB;
using Raven.Client;


namespace PatentSpoiler.App.NinjectModules
{
    public class RavenDBNinjectModule : NinjectModule
    {
        public override void Load()
        { 
            Bind<IDocumentStore>().ToMethod(context =>
           {
               var manager = new DocumentStoreManager("PatentSpoilerRavenDB");
               var documentStore = manager.GetDocumentStore();

               return documentStore;
           })
           .InSingletonScope();

            Bind<IDocumentSession>().ToMethod(context => context.Kernel.Get<IDocumentStore>().OpenSession()).InRequestScope();
        }
    }
}