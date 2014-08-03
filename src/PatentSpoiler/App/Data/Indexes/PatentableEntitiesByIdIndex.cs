using System.Linq;
using PatentSpoiler.App.Domain.Patents;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace PatentSpoiler.App.Data.Indexes
{
    public class PatentableEntitiesByCategoryIndex : AbstractIndexCreationTask<PatentableEntity>
    {
        public PatentableEntitiesByCategoryIndex()
        {
            Map = patentableEntities => from patentableEntity in patentableEntities
                                        select new { patentableEntity.Categories };

            Indexes.Add(x => x.Categories, FieldIndexing.NotAnalyzed);
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
                                        from category in patentableEntity.Categories
                                        select new { Category=category, Count = 1 };
    
            Reduce = results => from result in results
                                group result by result.Category
                                into g
                                select new { Category = g.Key, Count = g.Sum(x => x.Count) };
        }
    }
}