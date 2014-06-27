using System.Linq;
using PatentSpoiler.App.Domain.Patents;
using Raven.Client.Indexes;

namespace PatentSpoiler.App.Data.Indexes
{
    public class PatentableEntitiesByCategoryIndex : AbstractIndexCreationTask<PatentableEntity>
    {
        public PatentableEntitiesByCategoryIndex()
        {
            Map = classifications => from x in classifications
                select new { x.Category };
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
            Map = classifications => from x in classifications
                                     select new { x.Category, Count=1 };

            Reduce = results =>
                from result in results
                group result by result.Category
                    into g
                    select new { Category = g.Key, Count = g.Sum(x => x.Count) };
        }
    }
}