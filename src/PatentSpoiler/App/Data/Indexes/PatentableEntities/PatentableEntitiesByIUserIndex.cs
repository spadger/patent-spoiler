using System.Linq;
using PatentSpoiler.App.Domain.Patents;
using Raven.Client.Indexes;

namespace PatentSpoiler.App.Data.Indexes.PatentableEntities
{
    public class PatentableEntityCountByOwnerIndex : AbstractIndexCreationTask<PatentableEntity, PatentableEntityCountByOwnerIndex.ReduceResult>
    {
        public class ReduceResult
        {
            public string Owner { get; set; }
            public int Count { get; set; }
        }

        public PatentableEntityCountByOwnerIndex()
        {
            Map = patentableEntities => from patentableEntity in patentableEntities
                                        where !patentableEntity.Archived
                                        select new { patentableEntity.Owner, Count = 1 };
    
            Reduce = results => from result in results
                                group result by result.Owner
                                into g
                                select new { Owner = g.Key, Count = g.Sum(x => x.Count) };
        }
    }
}