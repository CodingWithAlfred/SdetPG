namespace RestAPIApp.Models
{
    public class TestDataModel
    {
        public List<PlaceHolderModel> Posts { get; set; } = new();
        public InvalidDataModel InvalidData { get; set; } = new();
    }
}
