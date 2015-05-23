using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
    public interface IUpdatePatentableEntityCommand
    {
        Task UpdateAsync(UpdateItemRequestViewModel viewModel, string userId);
    }

    public class UpdatePatentableEntityCommand : IUpdatePatentableEntityCommand
    {
        private readonly IAsyncDocumentSession session;
        private readonly IPatentStoreHierrachy patentStoreHierrachy;
        private readonly IStagingAttachmentAdapter stagingAttachmentAdapter;
        private readonly IStagedAttachmentAdapter stagedAttachmentAdapter;
        private readonly IPatentableEntitySearchIndexMaintainer patentableEntitySearchIndexMaintainer;

        public UpdatePatentableEntityCommand(IAsyncDocumentSession session, IPatentStoreHierrachy patentStoreHierrachy, IStagingAttachmentAdapter stagingAttachmentAdapter, IStagedAttachmentAdapter stagedAttachmentAdapter, IPatentableEntitySearchIndexMaintainer patentableEntitySearchIndexMaintainer)
        {
            this.session = session;
            this.patentStoreHierrachy = patentStoreHierrachy;
            this.stagingAttachmentAdapter = stagingAttachmentAdapter;
            this.stagedAttachmentAdapter = stagedAttachmentAdapter;
            this.patentableEntitySearchIndexMaintainer = patentableEntitySearchIndexMaintainer;
        }

        public async Task UpdateAsync(UpdateItemRequestViewModel viewModel, string userId)
        {
            var validations = new List<ValidationResult>();
            if (!Validator.TryValidateObject(viewModel, new ValidationContext(viewModel), validations, true))
            {
                throw new ArgumentException("Can't save new item");
            }

            var liveEntity = await session.LoadAsync<PatentableEntity>(viewModel.Id);

            if (liveEntity == null)
            {
                throw new InvalidOperationException("Not found");
            }

            if (liveEntity.Owner != userId)
            {
                throw new InvalidOperationException("Wrong user");
            }

            var archvedEntity = liveEntity.CreateArchiveVersion();

            Mapper.Map(viewModel, liveEntity);
            liveEntity.BumpVersion();
            liveEntity.ExplodedCategories = patentStoreHierrachy.GetAllCategoriesFor(viewModel.Categories);

            await MergeAttachmentsAsync(liveEntity, viewModel);

            await session.StoreAsync(liveEntity);
            await session.StoreAsync(archvedEntity);
            await session.SaveChangesAsync();

            Task.Run(() => DeletedStagingAttachments(viewModel.Attachments));
            Task.Run(() => patentableEntitySearchIndexMaintainer.ItemAmendedAsync(liveEntity));
        }

        public async Task MergeAttachmentsAsync(PatentableEntity liveEntity, UpdateItemRequestViewModel viewModel)
        {
            viewModel.Attachments = viewModel.Attachments ?? new List<AttachmentViewModel>();
            var requiredAttachmentIds = new HashSet<Guid>(viewModel.Attachments.Select(x => x.Id));
            liveEntity.Attachments.RemoveAll(x => !requiredAttachmentIds.Contains(x.Id));

            var existingAttachmentIds = new HashSet<Guid>(liveEntity.Attachments.Select(x => x.Id));
            var attachmentsToAdd = viewModel.Attachments.Where(x => !existingAttachmentIds.Contains(x.Id));

            foreach (var newAttachmentDTO in attachmentsToAdd)
            {
                var newAttachment = Mapper.Map<AttachmentViewModel, Attachment>(newAttachmentDTO);
                liveEntity.Attachments.Add(newAttachment);

                using (var stagedStream = stagingAttachmentAdapter.Get(newAttachment.Id))
                {
                    await stagedAttachmentAdapter.SaveAsync(newAttachment.Id, stagedStream, newAttachment.Type, newAttachment.Name);
                }
            }
        }

        private void DeletedStagingAttachments(List<AttachmentViewModel> attachments)
        {
            foreach (var attachment in attachments)
            {
                stagingAttachmentAdapter.Delete(attachment.Id);
            }
        }
    }
}