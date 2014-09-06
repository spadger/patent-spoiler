using System.Threading.Tasks;
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

            var result = new EditItemDisplayViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Owner = item.Owner,
                Description = item.Description,
                Categories = item.Categories,
            };

            return result;
        }
    }
}