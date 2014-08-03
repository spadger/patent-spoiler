using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PatentSpoiler.App.Domain.Patents;
using PatentSpoiler.App.Domain.Security;
using PatentSpoiler.App.DTOs;
using Raven.Client;

namespace PatentSpoiler.App.Commands
{
    public class SaveNewPatentableEntityCommand
    {
        private readonly IDocumentSession session;

        public SaveNewPatentableEntityCommand(IDocumentSession session)
        {
            this.session = session;
        }

        public void Save(AddItemRequestViewModel viewModel, string userId)
        {
            var validations = new List<ValidationResult>();
            if (!Validator.TryValidateObject(viewModel, new ValidationContext(viewModel), validations, true))
            {
                throw new ArgumentException("Can't save new item");
            }

            var entity = new PatentableEntity
            {
                Categories = viewModel.Categories,
                Name = viewModel.Name,
                Description = viewModel.Description,
                Owner = userId
            };

            session.Store(entity);
            session.SaveChanges();
        }
    }
}