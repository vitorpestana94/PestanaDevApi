namespace PestanaDevApi.Dtos.Requests
{
    public class ContactEmailRequestDto
    {
        public required string ClientEmail { get; set; }
        public required string ClientName { get; set; }
        public required string ClientMessage { get; set; }
    }
}
