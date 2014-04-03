using System.Collections.Generic;

namespace PatentSpoiler.App.Domain
{
    public class PatentClassification
    {
        public PatentClassification()
        {
            PatentableEntities = new HashSet<PatentableEntity>();
            TitleParts = new HashSet<string>();
        }

        public string Id { get; set; }
        
        public string ParentId { get; set; }

        public ISet<string> TitleParts { get; set; }

        public ICollection<PatentableEntity> PatentableEntities { get; set; }
    }

    public class PatentableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}