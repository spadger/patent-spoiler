using PatentSpoiler.Models;

namespace PatentSpoiler.App.DTOs
{
    public class SearchResultTree
    {
        public SearchResultTree Parent { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
        
        public static SearchResultTree From(PatentHierrachyNode hierrachyNode)
        {
            var current = new SearchResultTree
            {
                Id = hierrachyNode.ClassificationSymbol,
                Description = string.Join(" ", hierrachyNode.TitleParts),
                Parent = hierrachyNode.Parent == null ? null : From(hierrachyNode.Parent)
            };

            return current;
        }
    }
}