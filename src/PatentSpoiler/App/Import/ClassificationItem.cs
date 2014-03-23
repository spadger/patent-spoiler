using System.Linq;
using System.Xml;
using PatentSpoiler.Models;

namespace PatentSpoiler.App.Import
{
    public struct ClassificationItem
    {
        public ClassificationItem(XmlElement line):this()
        {
            var linkFileAttr = line.Attributes["link-file"];
            LinkFile = linkFileAttr==null? null : line.Attributes["link-file"].Value;
            Level = int.Parse(line.Attributes["level"].Value);
            ClassificationSymbol = line.GetElementsByTagName("classification-symbol").Cast<XmlElement>().First().InnerText;
            SortKey = line.Attributes["sort-key"].Value;
        }

        public string LinkFile { get; private set; }
        public int Level { get; private set; }
        public string ClassificationSymbol { get; private set; }
        public string SortKey { get; private set; }

        public bool HasLinkFile { get { return !string.IsNullOrEmpty(LinkFile); } }

        public string FileName
        {
            get { return string.Concat("scheme-", SortKey, ".xml"); }
        }

        public Node AsNode()
        {
            return new Node
            {
                 ClassificationSymbol = ClassificationSymbol,
                 Level = Level,
                 SortKey = SortKey
            };
        }
    }
}