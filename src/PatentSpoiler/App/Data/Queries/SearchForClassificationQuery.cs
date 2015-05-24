using System.Collections.Generic;
using System.Linq;
using Nest;
using PatentSpoiler.App.Data.ElasticSearch;
using PatentSpoiler.Models;

namespace PatentSpoiler.App.Data.Queries
{
    public interface ISearchForClassificationQuery
    {
        IEnumerable<PatentHierrachyNode> Execute(string searchPhrase, uint pageNumber, uint pageSize);
    }

    public class SearchForClassificationQuery : ISearchForClassificationQuery
    {
        private readonly IElasticClient client;
        private readonly  IPatentStoreHierrachy patentStoreHierrachy;

        public SearchForClassificationQuery(IElasticClient client, IPatentStoreHierrachy patentStoreHierrachy)
        {
            this.client = client;
            this.patentStoreHierrachy = patentStoreHierrachy;
        }

        public IEnumerable<PatentHierrachyNode> Execute(string searchPhrase, uint pageNumber, uint pageSize)
        {
            var searchResults = client.Search<PatentClassificationIndexItem>(s => s
                                      .Index("patent-classifications")                      
                                      .From(0)
                                      .Size(10)
                                      .Query(q => q.FuzzyLikeThis(fz => fz.OnFields(cat => cat.Title, cat=>cat.Id)
                                                                          .LikeText(searchPhrase)
                                                                          .MinimumSimilarity(0.8)
                                                                  )
                                             )
                                      )
                                      .Documents;

            var results = searchResults.Select(x => patentStoreHierrachy.GetDefinitionFor(x.Id))
                                       .ToList();
            return results;
        }
    }
}