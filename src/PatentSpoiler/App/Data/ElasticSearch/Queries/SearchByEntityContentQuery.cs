using System;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using PatentSpoiler.App.Data.Queries;
using PatentSpoiler.App.Domain.Patents;
using PatentSpoiler.App.DTOs;
using Raven.Client;

namespace PatentSpoiler.App.Data.ElasticSearch.Queries
{
    public interface ISearchByEntityContentQuery
    {
        Task<PageOf<PatentableEntityViewModel>> ExecuteAsync(string searchPhrase, int skip, int pageSize);
    }

    public class SearchByEntityContentQuery : ISearchByEntityContentQuery
    {
        private readonly IElasticClient client;
        private readonly IAsyncDocumentSession session;

        public SearchByEntityContentQuery(IElasticClient client, IAsyncDocumentSession session)
        {
            this.client = client;
            this.session = session;
        }

        public async Task<PageOf<PatentableEntityViewModel>> ExecuteAsync(string searchPhrase, int skip, int pageSize)
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
            
            var entities = await session.LoadAsync<PatentableEntity>(docs.Select(x=>x.Id).Cast<ValueType>());

            return Page.Of(entities.Select(PatentableEntityViewModel.FromDomainModel), (int)searchResults.Total);
        }
    }
}