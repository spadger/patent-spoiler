using System.Linq;
using PatentSpoiler.App.Domain.Patents;
using Raven.Client.Indexes;

namespace PatentSpoiler.App.Data.Indexes.PatentableEntities
{
    public class PatentableEntitiesBySetIdIndex : AbstractIndexCreationTask<PatentableEntity>
    {
        public PatentableEntitiesBySetIdIndex()
        {
            Map = patentableEntities => from patentableEntity in patentableEntities
                                        select new { patentableEntity.SetId, patentableEntity.Archived };
        }
    }
}