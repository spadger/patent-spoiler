namespace PatentSpoiler.App.Attachments
{
    public interface IAttachmentStagingSettings
    {
        string Path { get; }
        int MaxStagingAgeInDays { get; }
    }

    public interface IAttachmentPermenantSettings
    {
        string Path { get; }
        int MaxStagingAgeInDays { get; }
    }
}