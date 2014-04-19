using System;

using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace PatentSpoiler.App.Data
{
    public class RavenDBDocumentStoreManager : IDisposable
    {
        bool disposed;
        private readonly string connectionStringName;
        private IDocumentStore documentStore;

        public RavenDBDocumentStoreManager(string connectionStringName)
        {
            this.connectionStringName = connectionStringName;
        }

        private IDocumentStore CreateDefaultDocumentStore()
        {
            return new DocumentStore { ConnectionStringName = connectionStringName }
                    .Initialize();
        }

        public IDocumentStore GetDocumentStore()
        {
            if (documentStore == null || documentStore.WasDisposed)
            {
                documentStore = CreateDefaultDocumentStore();

                new RavenDocumentsByEntityName().Execute(documentStore);
                IndexCreation.CreateIndexes(GetType().Assembly, documentStore);
            }

            return documentStore;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~RavenDBDocumentStoreManager()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (documentStore != null)
                    documentStore.Dispose();
            }

            disposed = true;
        }
    }
}