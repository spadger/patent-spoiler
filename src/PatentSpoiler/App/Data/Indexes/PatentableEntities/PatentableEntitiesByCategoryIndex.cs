using System.Linq;
using PatentSpoiler.App.Domain.Patents;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace PatentSpoiler.App.Data.Indexes.PatentableEntities
{
    public class PatentableEntitiesByCategoryIndex : AbstractIndexCreationTask<PatentableEntity>
    {
        public PatentableEntitiesByCategoryIndex()
        {
            Map = patentableEntities => from patentableEntity in patentableEntities
                select new { patentableEntity.ExplodedCategories };

            Indexes.Add(x => x.ExplodedCategories, FieldIndexing.NotAnalyzed);
        }
    }
}