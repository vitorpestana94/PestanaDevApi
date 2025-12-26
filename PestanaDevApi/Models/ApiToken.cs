namespace PestanaDevApi.Models
{
    public class ApiToken
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;


        public ApiToken() { }

        public ApiToken(string jwtToken, string refreshToken)
        {
            Token = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
