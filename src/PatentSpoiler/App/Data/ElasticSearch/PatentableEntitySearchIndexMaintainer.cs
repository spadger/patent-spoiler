using System.Threading.Tasks;
using AutoMapper;
using Nest;
using PatentSpoiler.App.Domain.Patents;

namespace PatentSpoiler.App.Data.ElasticSearch
{
    public interface IPatentableEntitySearchIndexMaintainer
    {
        Task ItemCreatedAsync(PatentableEntity item);
        Task ItemAmendedAsync(PatentableEntity item);
        Task ItemDeletedAsync(PatentableEntity item);
    }

    public class PatentableEntitySearchIndexMaintainer : IPatentableEntitySearchIndexMaintainer
    {
        private IElasticClient elasticClient;
        
        public PatentableEntitySearchIndexMaintainer(IElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        public async Task ItemCreatedAsync(PatentableEntity item)
        {
            var indexItem = Mapper.Map<PatentableEntityIndexItem>(item);
            var result = await elasticClient.IndexAsync(indexItem, x => x.Index("items"));
        }

        public async Task ItemAmendedAsync(PatentableEntity item)
        {
            var indexItem = Mapper.Map<PatentableEntityIndexItem>(item);
            var result = await elasticClient.IndexAsync(indexItem, x => x.Index("items"));
        }

        public async Task ItemDeletedAsync(PatentableEntity item)
        {
            await elasticClient.DeleteAsync(item);
        }
    }
}