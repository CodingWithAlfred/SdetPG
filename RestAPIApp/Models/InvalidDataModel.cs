namespace RestAPIApp.Models
{
    public class InvalidDataModel
    {
        public PlaceHolderModel EmptyTitle { get; set; } = new();
        public PlaceHolderModel NegativeId { get; set; } = new();
    }
}
