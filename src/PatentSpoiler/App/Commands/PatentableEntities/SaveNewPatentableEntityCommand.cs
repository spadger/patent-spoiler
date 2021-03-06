﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using PatentSpoiler.App.Attachments;
using PatentSpoiler.App.Data;
using PatentSpoiler.App.Data.ElasticSearch;
using PatentSpoiler.App.Domain.Patents;
using PatentSpoiler.App.DTOs.Item;
using Raven.Client;

namespace PatentSpoiler.App.Commands.PatentableEntities
{
    public interface ISaveNewPatentableEntityCommand
    {
        Task<int> SaveAsync(AddItemRequestViewModel viewModel, string userId);
    }

    public class SaveNewPatentableEntityCommand : ISaveNewPatentableEntityCommand
    {
        private readonly IAsyncDocumentSession session;
        private readonly IPatentStoreHierrachy patentStoreHierrachy;
        private readonly IStagingAttachmentAdapter stagingAttachmentAdapter;
        private readonly IStagedAttachmentAdapter stagedAttachmentAdapter;
        private readonly IPatentableEntitySearchIndexMaintainer patentableEntitySearchIndexMaintainer;

        public SaveNewPatentableEntityCommand(IAsyncDocumentSession session, IPatentStoreHierrachy patentStoreHierrachy, IStagingAttachmentAdapter stagingAttachmentAdapter, IStagedAttachmentAdapter stagedAttachmentAdapter, IPatentableEntitySearchIndexMaintainer patentableEntitySearchIndexMaintainer)
        {
            this.session = session;
            this.patentStoreHierrachy = patentStoreHierrachy;
            this.stagingAttachmentAdapter = stagingAttachmentAdapter;
            this.stagedAttachmentAdapter = stagedAttachmentAdapter;
            this.patentableEntitySearchIndexMaintainer = patentableEntitySearchIndexMaintainer;
        }

        public async Task<int> SaveAsync(AddItemRequestViewModel viewModel, string userId)
        {
            var validations = new List<ValidationResult>();
            if (!Validator.TryValidateObject(viewModel, new ValidationContext(viewModel), validations, true))
            {
                throw new ArgumentException("Can't save new item");
            }

            var explodedCategories = patentStoreHierrachy.GetAllCategoriesFor(viewModel.Categories);

            var entity = Mapper.Map<AddItemRequestViewModel, PatentableEntity>(viewModel);
            entity.ExplodedCategories = explodedCategories;
            entity.Owner = userId;
            entity.Changes = "Created";

            await StageAttachmentsAsync(viewModel.Attachments);
            
            await session.StoreAsync(entity);
            await session.SaveChangesAsync();

            Task.Run(() => DeleteStagingAttachments(viewModel.Attachments));
            Task.Run(() => patentableEntitySearchIndexMaintainer.ItemCreatedAsync(entity));

            return entity.Id;
        }

        private async Task StageAttachmentsAsync(List<AttachmentViewModel> attachments)
        {
            if (attachments == null)
            {
                return;
            }
            foreach (var attachment in attachments)
            {
                using (var stagedStream = stagingAttachmentAdapter.Get(attachment.Id))
                {
                    await stagedAttachmentAdapter.SaveAsync(attachment.Id, stagedStream, attachment.Type, attachment.Name);
                }
            }
        }

        private void DeleteStagingAttachments(List<AttachmentViewModel> attachments)
        {
            if (attachments == null)
            {
                return;
            }

            foreach (var attachment in attachments)
            {
                stagingAttachmentAdapter.Delete(attachment.Id);
            }
        }
    }
}