using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PatentSpoiler.App.Data;
using PatentSpoiler.App.Domain.Patents;
using PatentSpoiler.App.DTOs.Item;
using Raven.Client;

namespace PatentSpoiler.App.Commands
{
    public class SaveNewPatentableEntityCommand
    {
        private readonly IDocumentSession session;
        private readonly IPatentStoreHierrachy patentStoreHierrachy;

        public SaveNewPatentableEntityCommand(IDocumentSession session, IPatentStoreHierrachy patentStoreHierrachy)
        {
            this.session = session;
            this.patentStoreHierrachy = patentStoreHierrachy;
        }

        public void Save(AddItemRequestViewModel viewModel, string userId)
        {
            var validations = new List<ValidationResult>();
            if (!Validator.TryValidateObject(viewModel, new ValidationContext(viewModel), validations, true))
            {
                throw new ArgumentException("Can't save new item");
            }

            var explodedCategories = GetAllCategoriesFor(viewModel.Categories);

            var entity = new PatentableEntity
            {
                Categories = viewModel.Categories,
                ExplodedCategories = explodedCategories,
                Name = viewModel.Name,
                Description = viewModel.Description,
                Owner = userId
            };

            session.Store(entity);
            session.SaveChanges();
        }

        public HashSet<string> GetAllCategoriesFor(HashSet<string> categories)
        {
            var results = new HashSet<string>();

            foreach (var category in categories)
            {
                GetAllCategoriesFor(category, results);
            }

            return results;
        }
        
        public void GetAllCategoriesFor(string category, HashSet<string> results)
        {
            var categoryHierrachy = patentStoreHierrachy.GetDefinitionFor(category);

            do
            {
                if (categoryHierrachy.ClassificationSymbol != null)
                {
                    results.Add(categoryHierrachy.ClassificationSymbol);
                }
                categoryHierrachy = categoryHierrachy.Parent;
            } while (categoryHierrachy != null);
        }

    }
}