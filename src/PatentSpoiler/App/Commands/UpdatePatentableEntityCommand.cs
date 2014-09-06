using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Threading.Tasks;
using PatentSpoiler.App.Data;
using PatentSpoiler.App.Domain.Patents;
using PatentSpoiler.App.DTOs.Item;
using Raven.Client;

namespace PatentSpoiler.App.Commands
{
    public interface IUpdatePatentableEntityCommand
    {
        Task UpdateAsync(UpdateItemRequestViewModel viewModel, string userId);
    }

    public class UpdatePatentableEntityCommand : IUpdatePatentableEntityCommand
    {
        private readonly IAsyncDocumentSession session;
        private readonly IPatentStoreHierrachy patentStoreHierrachy;

        public UpdatePatentableEntityCommand(IAsyncDocumentSession session, IPatentStoreHierrachy patentStoreHierrachy)
        {
            this.session = session;
            this.patentStoreHierrachy = patentStoreHierrachy;
        }

        public async Task UpdateAsync(UpdateItemRequestViewModel viewModel, string userId)
        {
            var validations = new List<ValidationResult>();
            if (!Validator.TryValidateObject(viewModel, new ValidationContext(viewModel), validations, true))
            {
                throw new ArgumentException("Can't save new item");
            }

            var explodedCategories = patentStoreHierrachy.GetAllCategoriesFor(viewModel.Categories);

            var entity = await session.LoadAsync<PatentableEntity>(viewModel.Id);

            if (entity == null)
            {
                throw new InvalidOperationException("Not found");
            }

            if (entity.Owner != userId)
            {
                throw new InvalidOperationException("Wrong user");
            }

            entity.Categories = viewModel.Categories;
            entity.ExplodedCategories = explodedCategories;
            entity.Name = viewModel.Name;
            entity.Description = viewModel.Description;

            await session.StoreAsync(entity);
            await session.SaveChangesAsync();
        }
    }
}