using System;

namespace PatentSpoiler.App.Domain.Patents
{
    public class Attachment
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime DateCreated { get; set; }
        public long FileSize { get; set; }
        public string Type { get; set; }
    }
}