using System;
using System.Collections.Generic;

namespace PatentSpoiler.App.Domain.Patents
{
    public class PatentableEntity
    {
        public PatentableEntity()
        {
            Attachments = new List<Attachment>();   
        }

        public int Id { get; set; }
        public string Category { get; set; }
        public string Owner { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        
        public List<Attachment> Attachments { get; set; }
    }

    public class Attachment
    {
        public Attachment()
        {
            Versions = new List<AttachmentVersion>();
        }

        public string Name { get; set; }
        public List<AttachmentVersion> Versions { get; set; } 
    }

    public class AttachmentVersion
    {
        public int Version { get; set; }
        public DateTime DateUploaded { get; set; }
        public string Path { get; set; }
    }
}