namespace PatentSpoiler.App.Domain.Patents
{
    public class PatentClassification
    {
        public string Id { get; set; }
        
        public string ParentId { get; set; }

        public string[] Keywords { get; set; }
    }
}