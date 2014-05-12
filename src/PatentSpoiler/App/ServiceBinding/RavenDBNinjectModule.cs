using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using PatentSpoiler.App.Data;
using Raven.Client;


namespace PatentSpoiler.App.ServiceBinding
{
    public class RavenDBNinjectModule : NinjectModule
    {
        public override void Load()
        { 
            Bind<IDocumentStore>().ToMethod(context =>
           {
               var manager = new RavenDBDocumentStoreManager("PatentSpoilerRavenDB");
               var documentStore = manager.GetDocumentStore();

               return documentStore;
           })
           .InSingletonScope();

            Bind<IDocumentSession>().ToMethod(context => context.Kernel.Get<IDocumentStore>().OpenSession()).InRequestScope();
            Bind<IAsyncDocumentSession>().ToMethod(context => context.Kernel.Get<IDocumentStore>().OpenAsyncSession()).InRequestScope();
        }
    }
}