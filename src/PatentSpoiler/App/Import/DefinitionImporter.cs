using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using PatentSpoiler.Annotations;
using PatentSpoiler.Models;

namespace PatentSpoiler.App.Import
{
    public interface IDefinitionImporter
    {
        Node Import(string documentsPath, string rootDocumentFileName);
    }

    [UsedImplicitly]
    public class DefinitionImporter : IDefinitionImporter
    {
        private readonly IXmlDocumentLoader documentLoader;
        private string documentsPath;
        private bool run = false;
        private readonly object syncRoot = new object();

        public DefinitionImporter(IXmlDocumentLoader documentLoader)
        {
            this.documentLoader = documentLoader;
        }

        public Node Import(string documentsPath, string rootDocumentFileName)
        {
            var root = new Node();

            lock (syncRoot)
            {
                if (run)
                {
                    throw new InvalidOperationException("Already run");
                }

                run = true;
            }
            this.documentsPath = documentsPath;

            var subNodes = ImportClassificationFile(rootDocumentFileName);

            foreach (var subNode in subNodes)
            {
                root.AddChild(subNode);
            }

            return root;
        }

        public IEnumerable<Node> ImportClassificationFile(string fileName)
        {
            var doc = documentLoader.Load(documentsPath, fileName);
            var subNodes = new List<Node>();

            foreach (XmlElement element in doc.SelectNodes("class-scheme/classification-item"))
            {
                //yield is a PITA for debugging...
                var subNode = ImportClassificationItemNode(element);
                subNodes.AddRange(subNode);
            }

            return subNodes;
        }

        public IEnumerable<Node> ImportClassificationItemNode(XmlElement importScope)
        {
            var results = new List<Node>();

            if (importScope.HasAttribute("link-file"))
            {
                var sortKey = importScope.Attributes["sort-key"].Value;
                var linkedFileName = string.Concat("scheme-", sortKey, ".xml");
                var linkedSubNodes = ImportClassificationFile(linkedFileName);

                foreach (var linkedSubNode in linkedSubNodes)
                {
                    results.Add(linkedSubNode);
                }

                return results;
            }
            else
            {
                var result = CreateNodeFromCurrentElement(importScope);
                foreach (XmlElement subItemNode in importScope.SelectNodes("classification-item"))
                {
                    var subNodes = ImportClassificationItemNode(subItemNode);
                    result.AddChildren(subNodes);
                }

                return new[] {result};
            }
        }

        public Node CreateNodeFromCurrentElement(XmlElement element)
        {
            var result = new Node
            {
                Level = int.Parse(element.Attributes["level"].Value),
                SortKey = element.Attributes["sort-key"].Value,
                ClassificationSymbol = element.SelectSingleNode("classification-symbol").InnerText
            };

            var titleParts = element.SelectNodes("class-title/title-part/text").Cast<XmlElement>().Select(x=>x.InnerXml.Trim());

            foreach (var part in titleParts)
            {
                result.AddTitlePart(part.Trim());
            }

            return result;
        }
    }
}