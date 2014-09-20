using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;
using PatentSpoiler.App.Domain.Patents;
using PatentSpoiler.Models;

namespace PatentSpoiler.App.Data
{
    public interface IPatentDatabaseIndexBuilder
    {
        Task IndexCategoriesAsync(PatentHierrachyNode root);
    }

    public class PatentDatabaseIndexBuilder : IPatentDatabaseIndexBuilder
    {
        private IElasticClient elasticClient;

        public PatentDatabaseIndexBuilder(IElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        public async Task IndexCategoriesAsync(PatentHierrachyNode root)
        {
            Check(root, new HashSet<string>(), new List<PatentHierrachyNode>());

            var descriptor = new BulkDescriptor();

            BuildIndexOperation(root, descriptor);

            var result = await elasticClient.BulkAsync(descriptor);
        }

        private void BuildIndexOperation(PatentHierrachyNode node, BulkDescriptor bulkDescriptor)
        {
            var classification = node.ToPatentClassification();

            bulkDescriptor.Index<PatentClassification>(x => x.Document(classification).Index("patent-classifications"));
            
            foreach (var child in node.Children)
            {
                BuildIndexOperation(child, bulkDescriptor);
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