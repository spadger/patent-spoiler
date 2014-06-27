using System.Collections.Generic;
using System.Linq;
using PatentSpoiler.App.Data.Indexes;
using PatentSpoiler.App.Domain.Patents;
using PatentSpoiler.Models;
using Raven.Abstractions.Util;
using Raven.Client;

namespace PatentSpoiler.App.Data.Queries
{
    public interface ISearchForClassificationQuery
    {
        IEnumerable<PatentHierrachyNode> Execute(string searchPhrase, uint pageNumber, uint pageSize);
    }

    public class SearchForClassificationQuery : ISearchForClassificationQuery
    {
        private readonly  IDocumentStore documentStore;
        private readonly  IPatentStoreHierrachy patentStoreHierrachy;

        public SearchForClassificationQuery(IDocumentStore documentStore, IPatentStoreHierrachy patentStoreHierrachy)
        {
            this.documentStore = documentStore;
            this.patentStoreHierrachy = patentStoreHierrachy;
        }

        public IEnumerable<PatentHierrachyNode> Execute(string searchPhrase, uint pageNumber, uint pageSize)
        {
            using (var session = documentStore.OpenSession())
            {
                var query = session.Advanced.LuceneQuery<PatentClassification, DocumentsByTitlePartIndex>();

                query = query.Search(x => x.Keywords, RavenQuery.Escape(searchPhrase, true, true) + "~0.8")
                             .Skip((int) pageNumber*(int) pageSize)
                             .Take((int) pageSize);

                var queryResults = query.Take(10).ToList();

                var results = queryResults.Select(x => patentStoreHierrachy.GetDefinitionFor(x.Id));

                return results.ToList();
            }
        }
    }
}