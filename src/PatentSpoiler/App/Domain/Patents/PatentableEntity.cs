using System;
using System.Collections.Generic;
using AutoMapper;

namespace PatentSpoiler.App.Domain.Patents
{
    public class PatentableEntity
    {
        public PatentableEntity()
        {
            Attachments = new List<Attachment>();
            DateCreated = DateTime.Now;
            DateUpdated = DateTime.Now;
            SetId = Guid.NewGuid();
        }

        public int Id { get; set; }
        public Guid SetId { get; set; }
        public HashSet<string> Categories { get; set; }
        public HashSet<string> ExplodedCategories { get; set; }
        public string Owner { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        
        public List<Attachment> Attachments { get; set; }
        
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        
        public int Version { get; set; }
        public bool Archived { get; set; }

        public PatentableEntity CreateArchiveVersion()
        {
            var @new = Mapper.Map<PatentableEntity, PatentableEntity>(this);
            @new.Archived = true;
            @new.ExplodedCategories.Clear();

            return @new;
        }

        public void BumpVersion()
        {
            Version++;
            DateUpdated = DateTime.Now;
        }
    }

    public class Attachment
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}