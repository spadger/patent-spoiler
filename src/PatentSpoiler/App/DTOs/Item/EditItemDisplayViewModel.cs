using System.Collections.Generic;

namespace PatentSpoiler.App.DTOs.Item
{
    public class EditItemDisplayViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Description { get; set; }
        public HashSet<string> Categories { get; set; }
    }
}