using System;
using System.Collections.Generic;
using PatentSpoiler.Models;
using Raven.Client;
using Raven.Client.Document;

namespace PatentSpoiler.App.Data
{
    public interface IPatentDatabaseLoader
    {
        void StoreNodes(PatentHierrachyNode root);
    }

    public class RavenDBBasedPatentDatabaseLoader : IPatentDatabaseLoader
    {
        private IDocumentStore documentStore;

        public RavenDBBasedPatentDatabaseLoader(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        public IEnumerable<PatentHierrachyNode> NodesForTerm(string term)
        {
            throw new System.NotImplementedException();
        }

        public void StoreNodes(PatentHierrachyNode root)
        {
            Check(root, new HashSet<string>(), new List<PatentHierrachyNode>());

            using (var bulkInsertOperation = documentStore.BulkInsert())
            {
                StoreNodesInternal(root, bulkInsertOperation);
            }
        }

        private void StoreNodesInternal(PatentHierrachyNode node, BulkInsertOperation bulkInsertOperation)
        {
            var classification = node.ToPatentClassification();

            try
            {
                bulkInsertOperation.Store(classification);
            }
            catch (Exception ex)
            {
                
            }

            foreach (var child in node.Children)
            {
                StoreNodesInternal(child, bulkInsertOperation);
            }
        }

        private void Check(PatentHierrachyNode node, HashSet<string> prev, List<PatentHierrachyNode> nodes)
        {
            if (!prev.Add(node.ClassificationSymbol))
            {
                
            }

            nodes.Add(node);

            foreach (var child in node.Children)
            {
                Check(child, prev, nodes);
            }
        }
    }
}