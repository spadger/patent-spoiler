using System.Linq;
using System.Threading.Tasks;
using Nest;
using PatentSpoiler.App.Data.Queries;
using PatentSpoiler.Models;

namespace PatentSpoiler.App.Data.ElasticSearch.Queries
{
    public interface ISearchByEntityContentQuery
    {
        Task<PageOf<PatentableEntityIndexItem>> ExecuteAsync(string searchPhrase, int skip, int pageSize);
    }

    public class SearchByEntityContentQuery : ISearchByEntityContentQuery
    {
        private readonly IElasticClient client;
        private readonly  IPatentStoreHierrachy patentStoreHierrachy;

        public SearchByEntityContentQuery(IElasticClient client, IPatentStoreHierrachy patentStoreHierrachy)
        {
            this.client = client;
            this.patentStoreHierrachy = patentStoreHierrachy;
        }

        public async Task<PageOf<PatentableEntityIndexItem>> ExecuteAsync(string searchPhrase, int skip, int pageSize)
        {
            var searchResults = await client.SearchAsync<PatentableEntityIndexItem>(s => s
                                      .Index("items")                      
                                      .From(skip)
                                      .Size(pageSize)
                                      .Query(q => q.FuzzyLikeThis(fz => fz.OnFields(x => x.Claims, x=>x.Description, x=>x.Name)
                                                                          .LikeText(searchPhrase)
                                                                          .MinimumSimilarity(0.8)
                                                                  )
                                             )
                                      );

            var docs = searchResults.Documents;
            return Page.Of(docs, (int)searchResults.Total);
        }
    }
}