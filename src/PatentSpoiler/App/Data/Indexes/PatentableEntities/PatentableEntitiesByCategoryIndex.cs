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
                                        where !patentableEntity.Archived
                                        select new { patentableEntity.ExplodedCategories };

            Indexes.Add(x => x.ExplodedCategories, FieldIndexing.NotAnalyzed);
        }
    }

    public class PatentableEntityCountByCategoryIndex : AbstractIndexCreationTask<PatentableEntity, PatentableEntityCountByCategoryIndex.ReduceResult>
    {
        public class ReduceResult
        {
            public string Category { get; set; }
            public int Count { get; set; }
        }

        public PatentableEntityCountByCategoryIndex()
        {
            Map = patentableEntities => from patentableEntity in patentableEntities
                                        where !patentableEntity.Archived
                                        from category in patentableEntity.ExplodedCategories
                                        select new { Category = category, Count = 1 };

            Reduce = results => from result in results
                                group result by result.Category
                                    into g
                                    select new { Category = g.Key, Count = g.Sum(x => x.Count) };
        }
    }
}