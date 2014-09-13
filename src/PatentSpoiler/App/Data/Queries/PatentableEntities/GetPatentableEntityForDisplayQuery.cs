using System.Threading.Tasks;
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

            var result = new DisplayItemViewModel
            {
                Name = item.Name,
                Description = item.Description,
                Owner = item.Owner,
                Categories = item.Categories,
                Attachments = item.Attachments
            };

            return result;
        }
    }
}