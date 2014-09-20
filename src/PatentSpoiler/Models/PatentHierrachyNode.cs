using System;
using System.Collections.Generic;
using PatentSpoiler.App.Domain.Patents;

namespace PatentSpoiler.Models
{
    [Serializable]
    public class PatentHierrachyNode
    {
        private readonly List<PatentHierrachyNode> children = new List<PatentHierrachyNode>();
        private readonly HashSet<string> titleParts = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

        public int Level { get; set; }
        public string ClassificationSymbol { get; set; }
        public string Title { get; set; }

        public PatentHierrachyNode Parent { get; private set; }

        public IEnumerable<PatentHierrachyNode> Children
        {
            get
            {
                return children;
            }
        }

        public void AddChild(PatentHierrachyNode child)
        {
            children.Add(child);
            child.Parent = this;
        }

        public void Remove(PatentHierrachyNode child)
        {
            if (!children.Remove(child))
            {
                throw new InvalidOperationException("Child not found in parent");
            }
            child.Parent = null;
        }

        public void AddChildren(IEnumerable<PatentHierrachyNode> children)
        {
            foreach (var child in children)
            {
                AddChild(child);
            }
        }

        public PatentClassification ToPatentClassification()
        {
            return new PatentClassification
            {
                Id = ClassificationSymbol,
                Title = Title
            };
        }

        public override string ToString()
        {
            return ClassificationSymbol;
        }
    }
}