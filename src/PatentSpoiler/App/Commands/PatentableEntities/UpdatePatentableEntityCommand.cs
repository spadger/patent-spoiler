using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Threading.Tasks;
using AutoMapper;
using PatentSpoiler.App.Data;
using PatentSpoiler.App.Domain.Patents;
using PatentSpoiler.App.DTOs.Item;
using Raven.Client;

namespace PatentSpoiler.App.Commands.PatentableEntities
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

            var original = await session.LoadAsync<PatentableEntity>(viewModel.Id);

            if (original == null)
            {
                throw new InvalidOperationException("Not found");
            }

            if (original.Owner != userId)
            {
                throw new InvalidOperationException("Wrong user");
            }

            var archvedEntity = original.CreateArchiveVersion();

            Mapper.Map(viewModel, original);
            original.BumpVersion();
            original.ExplodedCategories = patentStoreHierrachy.GetAllCategoriesFor(viewModel.Categories);
            
            await session.StoreAsync(original);
            await session.StoreAsync(archvedEntity);
            await session.SaveChangesAsync();
        }
    }
}