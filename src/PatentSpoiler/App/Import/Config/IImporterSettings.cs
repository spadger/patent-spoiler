namespace PatentSpoiler.App.Import.Config
{
    public interface IImporterSettings
    {
        string DocumentsPath { get; }
        string RootDocumentFileName { get; }
    }
}