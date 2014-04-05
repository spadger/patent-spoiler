using System.Linq;
using PatentSpoiler.App.Domain;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace PatentSpoiler.App.Database.Indexes
{
    public class DocumentsByTitlePartIndex : AbstractIndexCreationTask<PatentClassification>
    {
        public DocumentsByTitlePartIndex()
        {
            Map = classifications => from x in classifications
                select new {x.Keywords};

            Indexes.Add(x => x.Keywords, FieldIndexing.Analyzed);
        }
    }
}