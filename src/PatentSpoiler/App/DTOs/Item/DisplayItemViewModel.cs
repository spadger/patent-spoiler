using System.Collections.Generic;
using PatentSpoiler.App.Domain.Patents;

namespace PatentSpoiler.App.DTOs.Item
{
    public class DisplayItemViewModel
    {
        public int? LatestId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public HashSet<string> Categories { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}