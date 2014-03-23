using System;
using System.Collections.Generic;

namespace PatentSpoiler.Models
{
    public class Node
    {
        private List<Node> children = new List<Node>();
        private readonly HashSet<string> titleParts = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

        public int Level { get; set; }
        public string ClassificationSymbol { get; set; }
        public string SortKey { get; set; }
        public IEnumerable<string> TitleParts { get { return titleParts; } } 

        public Node Parent { get; private set; }

        public IEnumerable<Node> Children
        {
            get
            {
                return children;
            }
        }

        public void AddChild(Node child)
        {
            children.Add(child);
            child.Parent = this;
        }

        public Node GetRoot()
        {
            if (Parent == null)
            {
                return this;
            }

            return Parent.GetRoot();
        }

        public void AddTitlePart(string part)
        {
            titleParts.Add(part.ToLower());
        }

        public void AssertHierrachy()
        {
            var parent = GetRoot();
            parent.AssertDown();
        }

        public Node GetAncestorForLevel(int level)
        {
            if (Level == level)
            {
                return this;
            }

            if (level > Level)
            {
                throw new ArgumentException("Level is deeper than current level");
            }

            if (Parent == null)
            {
                throw new ArgumentException("Level is for a more-shallow node, but this node is the root");
            }

            return Parent.GetAncestorForLevel(level);
        }

        private void AssertDown()
        {
            foreach (var child in Children)
            {
                if (child.Level != Level + 1)
                {
                    throw new Exception("Level mismatch");
                }
                child.AssertDown();
            }
        }
    }
}