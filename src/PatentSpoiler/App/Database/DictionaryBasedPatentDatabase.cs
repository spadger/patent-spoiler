using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PatentSpoiler.App.Import;
using PatentSpoiler.App.Import.Config;
using PatentSpoiler.Models;

namespace PatentSpoiler.App.Database
{
    public interface IPatentStoreHierrachy
    {
        PatentHierrachyNode GetDefinitionFor(string id);
        PatentHierrachyNode Root { get; }
    }

    public class DictionaryBasedPatentStoreHierrachy : IPatentStoreHierrachy
    {
        private readonly Dictionary<string, PatentHierrachyNode> nodesForCategory = new Dictionary<string, PatentHierrachyNode>(StringComparer.InvariantCultureIgnoreCase);
        private readonly PatentHierrachyNode root;

        public DictionaryBasedPatentStoreHierrachy(IDefinitionImporter importer, ImporterSettings importerSettings, HttpContextBase context)
        {
            var fullPath = context.Server.MapPath(importerSettings.DocumentsPath);
            root = importer.Import(fullPath, importerSettings.RootDocumentFileName);
            SetupInverseIndexes(root);
        }

        public PatentHierrachyNode Root
        {
            get { return root; }
        }

        public PatentHierrachyNode GetDefinitionFor(string category)
        {
            PatentHierrachyNode result;
            if (nodesForCategory.TryGetValue(category.Trim(), out result))
            {
                return result;
            }

            return null;
        }

        public void SetupInverseIndexes(PatentHierrachyNode patentHierrachyNode)
        {
            if (!string.IsNullOrEmpty(patentHierrachyNode.ClassificationSymbol))
            {
                nodesForCategory.Add(patentHierrachyNode.ClassificationSymbol, patentHierrachyNode);
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