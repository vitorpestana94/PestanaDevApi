namespace PestanaDevApi.Dtos.Requests
{
    public class ContactEmailRequestDto : EmailRequest
    {
        public required string ClientName { get; set; }
        public required string ClientMessage { get; set; }
    }
}
