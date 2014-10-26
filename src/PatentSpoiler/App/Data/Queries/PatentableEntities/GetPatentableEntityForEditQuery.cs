using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PatentSpoiler.App.Data.Indexes.PatentableEntities;
using PatentSpoiler.App.Domain.Patents;
using PatentSpoiler.App.DTOs.Item;
using Raven.Client;

namespace PatentSpoiler.App.Data.Queries.PatentableEntities
{
    public interface IGetPatentableEntityForEditQuery
    {
        Task<EditItemDisplayViewModel> ExecuteAsync(int id);
    }

    public class GetPatentableEntityForEditQuery : IGetPatentableEntityForEditQuery
    {
        private readonly IAsyncDocumentSession session;

        public GetPatentableEntityForEditQuery(IAsyncDocumentSession session, IPatentStoreHierrachy patentStoreHierrachy)
        {
            this.session = session;
        }

        public async Task<EditItemDisplayViewModel> ExecuteAsync(int id)
        {
            var item = await session.LoadAsync<PatentableEntity>(id);

            if (item == null)
            {
                return null;
            }

            if (item.Archived)
            {
                item = await session.Query<PatentableEntity, PatentableEntitiesBySetIdIndex>()
                                    .FirstOrDefaultAsync(x => x.SetId == item.SetId && !x.Archived);

                if (item == null)
                {
                    return null;
                }
            }

            var result = Mapper.Map<PatentableEntity, EditItemDisplayViewModel>(item);
            return result;
        }
    }
}