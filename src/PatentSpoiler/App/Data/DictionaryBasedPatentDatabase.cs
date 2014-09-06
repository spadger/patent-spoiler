using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PatentSpoiler.App.Import;
using PatentSpoiler.App.Import.Config;
using PatentSpoiler.Models;

namespace PatentSpoiler.App.Data
{
    public interface IPatentStoreHierrachy
    {
        PatentHierrachyNode GetDefinitionFor(string id);
        PatentHierrachyNode Root { get; }
        bool ContainsCategory(string id);
        HashSet<string> GetAllCategoriesFor(IEnumerable<string> categories);
        IEnumerable<string> GetParentCategoriesFor(string category);
    }

    public class DictionaryBasedPatentStoreHierrachy : IPatentStoreHierrachy
    {
        private readonly Dictionary<string, PatentHierrachyNode> nodesForCategory = new Dictionary<string, PatentHierrachyNode>(StringComparer.InvariantCultureIgnoreCase);
        private readonly PatentHierrachyNode root;

        public DictionaryBasedPatentStoreHierrachy(IDefinitionImporter importer, ImporterSettings importerSettings, HttpContextBase context)
        {
            var fullPath = context.Server.MapPath(importerSettings.DocumentsPath);
            root = importer.Import(fullPath, importerSettings.RootDocumentFileName);

            FlattenHierrachy(root);
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

        private void FlattenHierrachy(PatentHierrachyNode node)
        {
            var toFlatten = new List<Tuple<PatentHierrachyNode, PatentHierrachyNode>>();
            ProduceFlattenList(node, toFlatten);

            foreach (var pair in toFlatten)
            {
                SquashNodes(pair.Item1, pair.Item2);
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

        public void SetupInverseIndexes(PatentHierrachyNode node)
        {
            
            if (!string.IsNullOrEmpty(node.ClassificationSymbol))
            {
                nodesForCategory.Add(node.ClassificationSymbol, node);
            }

            foreach (var childNode in node.Children)
            {
                SetupInverseIndexes(childNode);
            }
        }

        public bool ContainsCategory(string id)
        {
            return nodesForCategory.ContainsKey((id??"").Trim());
        }

        public HashSet<string> GetAllCategoriesFor(IEnumerable<string> categories)
        {
            var results = new HashSet<string>();

            foreach (var category in categories)
            {
                var relatedCategories = GetParentCategoriesFor(category);
                foreach (var relatedCategory in relatedCategories)
                {
                    results.Add(relatedCategory);
                }
            }

            return results;
        }
        
        public IEnumerable<string> GetParentCategoriesFor(string category)
        {
            var categoryHierrachy = GetDefinitionFor(category);

            do
            {
                if (categoryHierrachy.ClassificationSymbol != null)
                {
                    yield return categoryHierrachy.ClassificationSymbol;
                }
                categoryHierrachy = categoryHierrachy.Parent;
            } while (categoryHierrachy != null);
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