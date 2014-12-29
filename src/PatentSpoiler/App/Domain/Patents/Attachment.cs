using System;

namespace PatentSpoiler.App.Domain.Patents
{
    public class Attachment
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public long Size { get; set; }
        public string Type { get; set; }

        public string LinkText()
        {
            return string.Format("{0}, {1}", Name, Size.PrettyFileSize());
        }
    }
}