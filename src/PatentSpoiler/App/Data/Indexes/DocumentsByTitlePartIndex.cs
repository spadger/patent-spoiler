using System.Linq;
using PatentSpoiler.App.Domain.Patents;
using PatentSpoiler.App.ServiceBinding;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace PatentSpoiler.App.Data.Indexes
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


    public abstract class Base
    {
        private static int seed = 1;
        public int value;
        protected Base()
        {
            value = seed++;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }

    public interface ISingleton { }
    [BindAsASingleton]
    public class Singleton : Base, ISingleton { }

    public interface IWeb { }
    [BindInRequestScope]
    public class Web : Base, IWeb { }

    public interface ITransient { }
    public class Transient : Base, ITransient { }
}