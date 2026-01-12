namespace RestAPIApp.Models
{
    public class ApiSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public int Timeout { get; set; }
        public Dictionary<string, string> Headers { get; set; } = new();
    }
}
