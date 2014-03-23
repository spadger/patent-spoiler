using PatentSpoiler.Models;

namespace PatentSpoiler.App.Import
{
    public interface IDefinitionImporter
    {
        Node Import(string documentsPath, string rootDocumentFileName);
    }   
}