using System;

using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace PatentSpoiler.App.Database.RavenDB
{
    public interface IDocumentStoreManager
    {
        IDocumentStore GetDocumentStore();
    }

    public class DocumentStoreManager : IDisposable, IDocumentStoreManager
    {
        bool _disposed;
        private readonly string _connectionStringName;
        private IDocumentStore _documentStore;

        public DocumentStoreManager(string connectionStringName)
        {
            _connectionStringName = connectionStringName;
        }

       
      
        private IDocumentStore CreateDefaultDocumentStore()
        {
            return new DocumentStore { ConnectionStringName = _connectionStringName }
                    .Initialize();
        }

        public IDocumentStore GetDocumentStore()
        {
            if (_documentStore == null || _documentStore.WasDisposed)
            {
                _documentStore = CreateDefaultDocumentStore();

                new RavenDocumentsByEntityName().Execute(_documentStore);
                IndexCreation.CreateIndexes(GetType().Assembly, _documentStore);
            }

            return _documentStore;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DocumentStoreManager()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (_documentStore != null)
                    _documentStore.Dispose();
            }

            _disposed = true;
        }
    }
}