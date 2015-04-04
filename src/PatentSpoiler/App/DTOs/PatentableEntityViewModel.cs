using System.Collections.Generic;
using PatentSpoiler.App.Domain.Patents;

namespace PatentSpoiler.App.DTOs
{
    public class PatentableEntityViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Claims { get; set; }
        public string Owner { get; set; }
        public int AttachmentCount { get; set; }

        public static PatentableEntityViewModel FromDomainModel(PatentableEntity domainEntity)
        {
            return new PatentableEntityViewModel
            {
                Id = domainEntity.Id,
                Name = domainEntity.Name,
                Description = domainEntity.Description,
                Claims = new List<string>(domainEntity.Claims),
                Owner = domainEntity.Owner,
                AttachmentCount = domainEntity.Attachments.Count,
            };
        }
    }
}