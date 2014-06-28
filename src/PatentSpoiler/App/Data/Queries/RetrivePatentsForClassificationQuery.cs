using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatentSpoiler.App.Data.Indexes;
using PatentSpoiler.App.Domain.Patents;
using PatentSpoiler.App.DTOs;
using Raven.Client;

namespace PatentSpoiler.App.Data.Queries
{
    public interface IRetrivePatentsForClassificationQuery
    {
        Task<PageOf<PatentableEntityViewModel>> ExecuteAsync(string category, int page, int pageSize);
    }

    public class PageOf<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int Count { get; private set; }

        public PageOf(IEnumerable<T> items, int count)
        {
            Items = items;
            Count = count;
        }
    }

    public static class Page
    {
        public static PageOf<T> Of<T>(IEnumerable<T> items, int count)
        {
            return new PageOf<T>(items, count);
        }
    }

    public class RetrivePatentsForClassificationQuery : IRetrivePatentsForClassificationQuery
    {
        private readonly  IDocumentStore documentStore;

        public RetrivePatentsForClassificationQuery(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        public async Task<PageOf<PatentableEntityViewModel>> ExecuteAsync(string category, int page, int pageSize)
        {
            using (var session = documentStore.OpenAsyncSession())
            {
                RavenQueryStatistics stats;
                var items = await session.Query<PatentableEntity, PatentableEntitiesByCategoryIndex>()
                    .Statistics(out stats)
                    .Where(x=> x.Category == category)
                    .Skip((page-1)*pageSize)
                    .Take(pageSize)
                    .OrderByDescending(x => x.Id)
                    .ToListAsync();

                return Page.Of(items.Select(PatentableEntityViewModel.FromDomainModel), stats.TotalResults);
            }
        }
    }
}