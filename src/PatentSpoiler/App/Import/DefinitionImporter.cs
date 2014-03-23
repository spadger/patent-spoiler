using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using PatentSpoiler.Annotations;
using PatentSpoiler.Models;
using Raven.Abstractions.Data;

namespace PatentSpoiler.App.Import
{
    [UsedImplicitly]
    public class DefinitionImporter : IDefinitionImporter
    {
        static readonly HashSet<string> StopWords = new HashSet<string>(new[]{"a", "about", "above", "above", "across", "after", "afterwards", "again", "against", "all", "almost", "alone", "along", "already", "also","although","always","am","among", "amongst", "amoungst", "amount",  "an", "and", "another", "any","anyhow","anyone","anything","anyway", "anywhere", "are", "around", "as",  "at", "back","be","became", "because","become","becomes", "becoming", "been", "before", "beforehand", "behind", "being", "below", "beside", "besides", "between", "beyond", "bill", "both", "bottom","but", "by", "call", "can", "cannot", "cant", "co", "con", "could", "couldnt", "cry", "de", "describe", "detail", "do", "done", "down", "due", "during", "each", "eg", "eight", "either", "eleven","else", "elsewhere", "empty", "enough", "etc", "even", "ever", "every", "everyone", "everything", "everywhere", "except", "few", "fifteen", "fify", "fill", "find", "fire", "first", "five", "for", "former", "formerly", "forty", "found", "four", "from", "front", "full", "further", "get", "give", "go", "had", "has", "hasnt", "have", "he", "hence", "her", "here", "hereafter", "hereby", "herein", "hereupon", "hers", "herself", "him", "himself", "his", "how", "however", "hundred", "ie", "if", "in", "inc", "indeed", "interest", "into", "is", "it", "its", "itself", "keep", "last", "latter", "latterly", "least", "less", "ltd", "made", "many", "may", "me", "meanwhile", "might", "mill", "mine", "more", "moreover", "most", "mostly", "move", "much", "must", "my", "myself", "name", "namely", "neither", "never", "nevertheless", "next", "nine", "no", "nobody", "none", "noone", "nor", "not", "nothing", "now", "nowhere", "of", "off", "often", "on", "once", "one", "only", "onto", "or", "other", "others", "otherwise", "our", "ours", "ourselves", "out", "over", "own","part", "per", "perhaps", "please", "put", "rather", "re", "same", "see", "seem", "seemed", "seeming", "seems", "serious", "several", "she", "should", "show", "side", "since", "sincere", "six", "sixty", "so", "some", "somehow", "someone", "something", "sometime", "sometimes", "somewhere", "still", "such", "system", "take", "ten", "than", "that", "the", "their", "them", "themselves", "then", "thence", "there", "thereafter", "thereby", "therefore", "therein", "thereupon", "these", "they", "thickv", "thin", "third", "this", "those", "though", "three", "through", "throughout", "thru", "thus", "to", "together", "too", "top", "toward", "towards", "twelve", "twenty", "two", "un", "under", "until", "up", "upon", "us", "very", "via", "was", "we", "well", "were", "what", "whatever", "when", "whence", "whenever", "where", "whereafter", "whereas", "whereby", "wherein", "whereupon", "wherever", "whether", "which", "while", "whither", "who", "whoever", "whole", "whom", "whose", "why", "will", "with", "within", "without", "would", "yet", "you", "your", "yours", "yourself", "yourselves", "the"});

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

            var fullPath = Path.Combine(documentsPath, rootDocumentFileName);

            var subNodes = ImportClassificationFile(fullPath);

            foreach (var subNode in subNodes)
            {
                root.AddChild(subNode);
            }

            return root;
        }

        public IEnumerable<Node> ImportClassificationFile(string fullPath)
        {
            var doc = new XmlDocument();
            doc.Load(fullPath);

            var subNodes = new List<Node>();

            foreach (XmlElement element in doc.SelectNodes("class-scheme/classification-item"))
            {
                //yield is a PITA for debugging...
                var subNode = ImportClassificationItemNode(element);
                subNodes.Add(subNode);
            }

            return subNodes;
        }

        public Node ImportClassificationItemNode(XmlElement importScope)
        {
            var result = CreateNodeFromCurrentElement(importScope);

            if (importScope.HasAttribute("link-file"))
            {
                var sortKey = importScope.Attributes["sort-key"].Value;
                var linkedFileName = Path.Combine(documentsPath, string.Concat("scheme-", sortKey, ".xml"));
                var linkedSubNodes = ImportClassificationFile(linkedFileName);

                foreach (var linkedSubNode in linkedSubNodes)
                {
                    result.AddChild(linkedSubNode);
                }
            }
            else
            {
                foreach (XmlElement subItemNode in importScope.SelectNodes("classification-item"))
                {
                    var subNode = ImportClassificationItemNode(subItemNode);
                    result.AddChild(subNode);
                }
            }
            
            return result;
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
                result.AddTitlePart(part);
            }

            return result;
        }
    }
}