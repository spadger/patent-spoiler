using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PatentSpoiler.App.Data.RavenDBIndexes.PatentableEntities;
using PatentSpoiler.App.Domain.Patents;
using PatentSpoiler.App.DTOs;
using Raven.Client;

namespace PatentSpoiler.App.Data.Queries.PatentableEntities
{
    public interface IGetPatentsBySetIdQuery
    {
        Task<PageOf<PreviousEntityVersionsViewModel>> ExecuteAsync(Guid setId, int skip, int take);
    }

    public class GetPatentsBySetIdQuery : IGetPatentsBySetIdQuery
    {
        private readonly  IAsyncDocumentSession session;

        public GetPatentsBySetIdQuery(IAsyncDocumentSession session)
        {
            this.session = session;
        }

        public async Task<PageOf<PreviousEntityVersionsViewModel>> ExecuteAsync(Guid setId, int skip, int take)
        {
                RavenQueryStatistics stats;
                var items = await session.Query<PatentableEntity, PatentableEntitiesBySetIdIndex>()
                    .Statistics(out stats)
                    .Where(x => x.SetId == setId && x.Archived)
                    .Skip(skip)
                    .Take(take)
                    .OrderByDescending(x => x.Id)
                    .ToListAsync();

                return Page.Of(items.Select(Mapper.Map<PatentableEntity, PreviousEntityVersionsViewModel>), stats.TotalResults);
        }
    }
}