using System.Collections.Generic;

namespace PatentSpoiler.App.Domain
{
    public class PatentClassification
    {
        public PatentClassification()
        {
            PatentableEntities = new HashSet<PatentableEntity>();
        }

        public string Id { get; set; }
        
        public string ParentId { get; set; }

        public string[] Keywords { get; set; }

        public ICollection<PatentableEntity> PatentableEntities { get; set; }
    }

    public class PatentableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}