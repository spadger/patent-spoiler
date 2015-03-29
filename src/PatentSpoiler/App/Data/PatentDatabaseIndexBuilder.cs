using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private int count = 0;
        private BulkDescriptor bulkDescriptor;

        public PatentDatabaseIndexBuilder(IElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        public async Task IndexCategoriesAsync(PatentHierrachyNode root)
        {
            Check(root, new HashSet<string>(), new List<PatentHierrachyNode>());

            bulkDescriptor = new BulkDescriptor();

            await BuildIndexOperation(root);
            var result = await elasticClient.BulkAsync(bulkDescriptor);
        }

        private async Task BuildIndexOperation(PatentHierrachyNode node, int level = 0)
        {
            //var classification = new PatentClassification{Id=node.ClassificationSymbol, Title = node.Title};
            var classification = new PatentClassification{Id=node.ClassificationSymbol, Title = GetTotalDescriptionFor(node)};

            if (level > 3)
            {
                count++;
                bulkDescriptor.Index<PatentClassification>(x => x.Document(classification).Index("patent-classifications"));

                if (count == 2500)
                {
                    var result = await elasticClient.BulkAsync(bulkDescriptor);
                    count = 0;
                    bulkDescriptor = new BulkDescriptor();
                }
            }
            
            foreach (var child in node.Children)
            {
                await BuildIndexOperation(child, level+1);
            }
        }

        private string GetTotalDescriptionFor(PatentHierrachyNode node)
        {
            var keywords = new StringBuilder();
            do
            {
                keywords.Append(' ').Append(node.Title);
                node = node.Parent;
            } while (node != null);

            var distinctKeywords = keywords.ToString()
                                           .Split(',',';', ' ')
                                           .Distinct(StringComparer.InvariantCultureIgnoreCase)
                                           .Where(x=>x.Length>2);

            return string.Join(" ", distinctKeywords);
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