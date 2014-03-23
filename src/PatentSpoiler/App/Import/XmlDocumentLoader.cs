using System.IO;
using System.Xml;
using PatentSpoiler.Annotations;

namespace PatentSpoiler.App.Import
{
    public interface IXmlDocumentLoader
    {
        XmlDocument Load(string documentsPath, string rootDocumentFileName);
    }

    [UsedImplicitly]
    public class XmlDocumentLoader : IXmlDocumentLoader
    {
        public XmlDocument Load(string documentsPath, string rootDocumentFileName)
        {
            return Load(Path.Combine(documentsPath, rootDocumentFileName));
        }

        public XmlDocument Load(string path)
        {
            var result = new XmlDocument();
            result.Load(path);
            return result;
        }
    }
}