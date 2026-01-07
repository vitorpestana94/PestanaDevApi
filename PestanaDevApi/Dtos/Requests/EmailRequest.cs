namespace PestanaDevApi.Dtos.Requests
{
    public class EmailRequest
    {
        public required string ClientEmail { get; set; }
        public required string ClientLocale { get; set; }
    }
}
