using System;
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

    public class PatentableEntityCountBySetIdIndex : AbstractIndexCreationTask<PatentableEntity, PatentableEntityCountBySetIdIndex.ReduceResult>
    {
        public class ReduceResult
        {
            public Guid SetId { get; set; }
            public int Count { get; set; }
        }

        public PatentableEntityCountBySetIdIndex()
        {
            Map = patentableEntities => from patentableEntity in patentableEntities
                                        select new { patentableEntity.SetId, Count = 1 };

            Reduce = results => from result in results
                                group result by result.SetId
                                into g
                                select new { SetId = g.Key, Count = g.Sum(x => x.Count) };
        }
    }
}