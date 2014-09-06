﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using PatentSpoiler.App.Data;
using PatentSpoiler.App.Domain.Patents;
using PatentSpoiler.App.DTOs.Item;
using Raven.Client;

namespace PatentSpoiler.App.Commands
{
    public interface ISaveNewPatentableEntityCommand
    {
        Task SaveAsync(AddItemRequestViewModel viewModel, string userId);
    }

    public class SaveNewPatentableEntityCommand : ISaveNewPatentableEntityCommand
    {
        private readonly IAsyncDocumentSession session;
        private readonly IPatentStoreHierrachy patentStoreHierrachy;

        public SaveNewPatentableEntityCommand(IAsyncDocumentSession session, IPatentStoreHierrachy patentStoreHierrachy)
        {
            this.session = session;
            this.patentStoreHierrachy = patentStoreHierrachy;
        }

        public async Task SaveAsync(AddItemRequestViewModel viewModel, string userId)
        {
            var validations = new List<ValidationResult>();
            if (!Validator.TryValidateObject(viewModel, new ValidationContext(viewModel), validations, true))
            {
                throw new ArgumentException("Can't save new item");
            }

            var explodedCategories = patentStoreHierrachy.GetAllCategoriesFor(viewModel.Categories);

            var entity = new PatentableEntity
            {
                Categories = viewModel.Categories,
                ExplodedCategories = explodedCategories,
                Name = viewModel.Name,
                Description = viewModel.Description,
                Owner = userId
            };

            await session.StoreAsync(entity);
            await session.SaveChangesAsync();
        }
    }
}