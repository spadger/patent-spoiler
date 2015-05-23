using System.Threading.Tasks;
using AutoMapper;
using PatentSpoiler.App.Data.RavenDBIndexes.PatentableEntities;
using PatentSpoiler.App.Domain.Patents;
using PatentSpoiler.App.Domain.Security;
using PatentSpoiler.App.DTOs.Item;
using Raven.Client;

namespace PatentSpoiler.App.Data.Queries.PatentableEntities
{
    public interface IGetPatentableEntityForDisplayQuery
    {
        Task<DisplayItemViewModel> ExecuteAsync(int id);
    }

    public class GetPatentableEntityForDisplayQuery : IGetPatentableEntityForDisplayQuery
    {
        private readonly IAsyncDocumentSession session;
        private readonly PatentSpoilerUser user;

        public GetPatentableEntityForDisplayQuery(IAsyncDocumentSession session, PatentSpoilerUser user)
        {
            this.session = session;
            this.user = user;
        }

        public async Task<DisplayItemViewModel> ExecuteAsync(int id)
        {
            var item = await session.LoadAsync<PatentableEntity>(id);
            if (item == null)
            {
                return null;
            }

            var result = Mapper.Map<PatentableEntity, DisplayItemViewModel>(item);

            if (item.Archived)
            {
                var latestItem = await session.Query<PatentableEntity, PatentableEntitiesBySetIdIndex>()
                                    .FirstOrDefaultAsync(x => x.SetId == item.SetId && !x.Archived);

                if (latestItem != null)
                {
                    result.LatestId = latestItem.Id;
                }
            }

            return result;
        }
    }
}