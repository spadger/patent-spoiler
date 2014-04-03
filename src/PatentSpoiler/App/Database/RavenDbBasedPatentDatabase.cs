using System;
using System.Collections.Generic;
using PatentSpoiler.Models;
using Raven.Client;
using Raven.Client.Document;

namespace PatentSpoiler.App.Database
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

            FlattenHierrachy(root);
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

        private void FlattenHierrachy(PatentHierrachyNode node)
        {
            var toFlatten = new List<Tuple<PatentHierrachyNode, PatentHierrachyNode>>();
            ProduceFlattenList(node, toFlatten);

            foreach (var pair in toFlatten)
            {
                SquashNodes(pair.Item1, pair.Item2);
            }
        }

        private void SquashNodes(PatentHierrachyNode parent, PatentHierrachyNode node)
        {
            foreach (var child in node.Children)
            {
                parent.AddChild(child);
            }

            parent.Remove(node);

            foreach (var nodePart in node.TitleParts)
            {
                parent.AddTitlePart(nodePart);
            }
        }

        private void ProduceFlattenList(PatentHierrachyNode node, List<Tuple<PatentHierrachyNode, PatentHierrachyNode>> removals)
        {
            foreach (var child in node.Children)
            {
                ProduceFlattenList(child, removals);
            }

            var parent = node.Parent;
            if (parent != null && parent.ClassificationSymbol == node.ClassificationSymbol)
            {
                removals.Add(Tuple.Create(parent, node));
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