using System;
using System.Collections.Generic;

namespace PatentSpoiler.Models
{
    public class Node
    {
        private List<Node> children = new List<Node>();

        public string Title { get; set; }
        public int Level { get; set; }

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

        public Node GetHead()
        {
            if (Parent == null)
            {
                return this;
            }

            return Parent.GetHead();
        }

        public void AssertHierrachy()
        {
            var parent = GetHead();
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