namespace PatentSpoiler.App.DTOs
{
    public class AddItemDisplayViewModel
    {
        public string Category { get; set; }
    }

    public class AddItemRequestViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}