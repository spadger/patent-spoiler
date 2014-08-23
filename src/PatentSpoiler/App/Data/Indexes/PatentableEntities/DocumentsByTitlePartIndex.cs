using System.Linq;
using PatentSpoiler.App.Domain.Patents;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace PatentSpoiler.App.Data.Indexes.PatentableEntities
{
    public class DocumentsByTitlePartIndex : AbstractIndexCreationTask<PatentClassification>
    {
        public DocumentsByTitlePartIndex()
        {
            Map = classifications => from x in classifications
                                     select new { x.Keywords };

            Indexes.Add(x => x.Keywords, FieldIndexing.Analyzed);
        }
    }
}