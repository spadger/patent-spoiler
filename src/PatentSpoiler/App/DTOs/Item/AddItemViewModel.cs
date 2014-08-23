using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PatentSpoiler.ValidationAttributes;

namespace PatentSpoiler.App.DTOs.Item
{
    public class AddItemDisplayViewModel
    {
        public string Category { get; set; }
    }

    public class AddItemRequestViewModel
    {
        [Required, StringLength(250, MinimumLength = 5)]
        public string Name { get; set; }
        [Required, MinLength(50)]
        public string Description { get; set; }

        [CannotBeEmpty]
        public HashSet<string> Categories { get; set; }
    }
}