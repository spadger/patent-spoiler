using System.Linq;
using System.Threading.Tasks;
using PatentSpoiler.App.Data.Indexes.PatentableEntities;
using PatentSpoiler.App.Domain.Patents;
using PatentSpoiler.App.DTOs;
using Raven.Client;

namespace PatentSpoiler.App.Data.Queries.PatentableEntities
{
    public interface IRetrievePatentsByOwnerQuery
    {
        Task<PageOf<PatentableEntityViewModel>> ExecuteAsync(string category, int skip, int pageSize);
    }

    public class RetrievePatentsByOwnerQuery : IRetrievePatentsByOwnerQuery
    {
        private readonly  IAsyncDocumentSession session;

        public RetrievePatentsByOwnerQuery(IAsyncDocumentSession session)
        {
            this.session = session;
        }

        public async Task<PageOf<PatentableEntityViewModel>> ExecuteAsync(string owner, int skip, int pageSize)
        {
                RavenQueryStatistics stats;
                var items = await session.Query<PatentableEntity, PatentableEntitiesByOwnerIndex>()
                    .Statistics(out stats)
                    .Search(x => x.Owner, owner)
                    .Skip(skip)
                    .Take(pageSize)
                    .OrderByDescending(x => x.Id)
                    .ToListAsync();

                return Page.Of(items.Select(PatentableEntityViewModel.FromDomainModel), stats.TotalResults);
            
        }
    }
}