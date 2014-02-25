using System;
using FluentAssertions;
using NUnit.Framework;
using PatentSpoiler.Models;

namespace PatentSpoiler.Tests
{
    [TestFixture]
    public class NodeTests
    {
        [Test]
        public void AssertHierrachy_AnEmptyNodeShouldAssertItsSelf()
        {
            var node = new Node();
            node.AssertHierrachy();
        }

        [Test]
        public void AssertHierrachy_AValidNodeStructureShouldAssertItsSelfWhenCalledFromTheRootNode()
        {
            var parent = new Node { Level = 1 };
            var child = new Node { Level = 2 };
            parent.AddChild(child);
            parent.AssertHierrachy();
        }

        [Test]
        public void AssertHierrachy_AValidNodeStructureShouldAssertItsSelfWhenCalledFromAChildNode()
        {
            var parent = new Node { Level = 1 };
            var child = new Node { Level = 2 };
            parent.AddChild(child);
            child.AssertHierrachy();
        }

        [Test]
        public void AssertHierrachy_ANodeStructureWhereParentAndChildShareALevelValueShouldNotAssert()
        {
            var parent = new Node { Level = 1 };
            var child = new Node { Level = 1 };
            parent.AddChild(child);
            
            parent.Invoking(x=>x.AssertHierrachy()).ShouldThrow<Exception>().WithMessage("Level mismatch");
        }

        
    }
}
