using System.Linq;
using System.Threading.Tasks;
using PatentSpoiler.App.Data.Indexes.PatentableEntities;
using PatentSpoiler.App.Domain.Patents;
using PatentSpoiler.App.DTOs;
using Raven.Client;

namespace PatentSpoiler.App.Data.Queries.PatentableEntities
{
    public interface IRetrievePatentsByClassificationQuery
    {
        Task<PageOf<PatentableEntityViewModel>> ExecuteAsync(string category, int page, int pageSize);
    }

    public class RetrievePatentsByClassificationQuery : IRetrievePatentsByClassificationQuery
    {
        private readonly  IAsyncDocumentSession session;

        public RetrievePatentsByClassificationQuery(IAsyncDocumentSession session)
        {
            this.session = session;
        }

        public async Task<PageOf<PatentableEntityViewModel>> ExecuteAsync(string category, int page, int pageSize)
        {
                RavenQueryStatistics stats;
                var items = await session.Query<PatentableEntity, PatentableEntitiesByCategoryIndex>()
                    .Statistics(out stats)
                    .Search(x=>x.ExplodedCategories, category)
                    .Skip((page-1)*pageSize)
                    .Take(pageSize)
                    .OrderByDescending(x => x.Id)
                    .ToListAsync();

                return Page.Of(items.Select(PatentableEntityViewModel.FromDomainModel), stats.TotalResults);
            
        }
    }
}