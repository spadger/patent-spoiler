using System.Collections.Generic;
using System.Linq;
using System.Web;
using PatentSpoiler.App.Import;
using PatentSpoiler.App.Import.Config;
using PatentSpoiler.Models;

namespace PatentSpoiler.App.Database
{
    public interface IPatentDatabase
    {
        IEnumerable<Node> NodesForCategory(string category);
        IEnumerable<Node> NodesForTerm(string term);
    }

    public class DictionaryBasedPatentDatabase : IPatentDatabase
    {
        private readonly Dictionary<string, List<Node>> nodesForCategory = new Dictionary<string, List<Node>>();
        private readonly Dictionary<string, List<Node>> nodesForTerm = new Dictionary<string, List<Node>>();
        private readonly Node root;

        public DictionaryBasedPatentDatabase(IDefinitionImporter importer, ImporterSettings importerSettings, HttpContextBase context)
        {
            var fullPath = context.Server.MapPath(importerSettings.DocumentsPath);
            root = importer.Import(fullPath, importerSettings.RootDocumentFileName);
            SetupInverseIndexes(root);
        }

        public IEnumerable<Node> NodesForCategory(string category)
        {
            List<Node> result;

            if (nodesForCategory.TryGetValue(category.Trim().ToLower(), out result))
            {
                return result;
            }

            return Enumerable.Empty<Node>();
        }

        public IEnumerable<Node> NodesForTerm(string term)
        {
            List<Node> result;

            if (nodesForTerm.TryGetValue(term.Trim().ToLower(), out result))
            {
                return result;
            }

            return Enumerable.Empty<Node>();
        }

        public void SetupInverseIndexes(Node node)
        {
            foreach (var term in node.TitlePartTerms)
            {
                nodesForTerm.AddNodeForKey(term, node);
            }

            if (!string.IsNullOrEmpty(node.ClassificationSymbol))
            {
                nodesForCategory.AddNodeForKey(node.ClassificationSymbol, node);
            }

            foreach (var child in node.Children)
            {
                SetupInverseIndexes(child);
            }
        }
    }

    public static class DictionaryExtensions
    {
        public static void AddNodeForKey<K, V>(this IDictionary<K, List<V>> dict, K key, V value)
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, new List<V> { value });
            }
            else
            {
                dict[key].Add(value);
            }
        }
    }
}