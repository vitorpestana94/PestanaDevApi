namespace PestanaDevApi.Dtos.Requests
{
    public class SignUpRequestDto
    {
        public required string Email { get; set; }
        public required string Nome { get; set; }

        public required string Senha { get; set; }
        public string Picture { get; set; } = string.Empty;

    }
}
