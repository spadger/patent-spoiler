using PatentSpoiler.Models;

namespace PatentSpoiler.App.DTOs
{
    public class SearchResult
    {
        public SearchResult Child { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }

        public static SearchResult From(PatentHierrachyNode currentNode)
        {
            return From(currentNode, null);

        }
        private static SearchResult From(PatentHierrachyNode currentNode, SearchResult child)
        {
            var current = new SearchResult
            {
                Id = currentNode.ClassificationSymbol,
                Description = currentNode.Title,
            };

            if (child != null)
            {
                current.Child = child;
            }

            if (currentNode.Parent.Parent == null)
            {
                return current;
            }

            return From(currentNode.Parent, current);
        }
    }
}