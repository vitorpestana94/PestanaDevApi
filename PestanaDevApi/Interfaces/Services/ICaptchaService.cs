namespace PestanaDevApi.Interfaces.Services
{
    public interface ICaptchaService
    {
        Task<bool> ValidateCaptchaV3(string captchaToken);
    }
}
