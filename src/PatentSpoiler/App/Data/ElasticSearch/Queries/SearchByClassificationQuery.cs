using System.Linq;
using System.Threading.Tasks;
using Nest;
using PatentSpoiler.App.Data.Queries;
using PatentSpoiler.Models;

namespace PatentSpoiler.App.Data.ElasticSearch.Queries
{
    public interface ISearchByClassificationQuery
    {
        Task<PageOf<PatentHierrachyNode>> ExecuteAsync(string searchPhrase, int skip, int pageSize);
    }

    public class SearchByClassificationQuery : ISearchByClassificationQuery
    {
        private readonly IElasticClient client;
        private readonly  IPatentStoreHierrachy patentStoreHierrachy;

        public SearchByClassificationQuery(IElasticClient client, IPatentStoreHierrachy patentStoreHierrachy)
        {
            this.client = client;
            this.patentStoreHierrachy = patentStoreHierrachy;
        }

        public async Task<PageOf<PatentHierrachyNode>> ExecuteAsync(string searchPhrase, int skip, int pageSize)
        {
            var searchResults = await client.SearchAsync<PatentClassificationIndexItem>(s => s
                                      .Index("patent-classifications")                      
                                      .From(skip)
                                      .Size(pageSize)
                                      .Query(q => q.FuzzyLikeThis(fz => fz.OnFields(cat => cat.Title, cat=>cat.Id)
                                                                          .LikeText(searchPhrase)
                                                                          .MinimumSimilarity(0.8)
                                                                  )
                                             )
                                      );
            var docs = searchResults.Documents;

            var results = docs.Select(x => patentStoreHierrachy.GetDefinitionFor(x.Id));
            return Page.Of(results, (int)searchResults.Total);
        }
    }
}