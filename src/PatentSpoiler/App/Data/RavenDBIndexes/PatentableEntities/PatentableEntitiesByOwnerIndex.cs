using System.Linq;
using PatentSpoiler.App.Domain.Patents;
using Raven.Client.Indexes;

namespace PatentSpoiler.App.Data.RavenDBIndexes.PatentableEntities
{
    public class PatentableEntitiesByOwnerIndex : AbstractIndexCreationTask<PatentableEntity>
    {
        public PatentableEntitiesByOwnerIndex()
        {
            Map = patentableEntities => from patentableEntity in patentableEntities
                                        where !patentableEntity.Archived
                                        select new { patentableEntity.Owner };
        }
    }
}