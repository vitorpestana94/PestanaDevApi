using System.Net;
using Microsoft.Extensions.Configuration;
using Moq;
using PestanaDevApi.Constants.Email;
using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Interfaces.Services.Email;
using PestanaDevApi.Models.Enums;
using PestanaDevApi.Services;
using PestanaDevApi.Services.Email;

namespace PestanaDevApi.Tests.Services
{
    public class TranslationServiceTests
    {
        private readonly TranslationService _service;

        public TranslationServiceTests()
        {
            // The Locales folder is copied to the tests output directory.
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);
            _service = new();
        }

        [Fact]
        public void Translate_ReturnsTheTextOfTheRequestedLocale()
        {
            Assert.Equal("Olá! Aqui está o seu código de confirmação.", _service.Translate("pt", EmailTranslationKeys.ConfirmationCodeTitle));
            Assert.Equal("Hello! Here it's your confirmation code.", _service.Translate("en", EmailTranslationKeys.ConfirmationCodeTitle));
        }

        [Fact]
        public void Translate_ReplacesTheVariables()
        {
            Assert.Equal("Olá, Vitor!", _service.Translate("pt", EmailTranslationKeys.ContactClientTitle, new() { ["name"] = "Vitor" }));
        }

        [Fact]
        public void Translate_FallsBackToEnglish_WhenLocaleOrKeyDoesNotExist()
        {
            Assert.Equal("Hello, Vitor!", _service.Translate("es", EmailTranslationKeys.ContactClientTitle, new() { ["name"] = "Vitor" }));
            Assert.Equal(" Pestana Dev. All rights reserved.", _service.Translate("pt", EmailTranslationKeys.ContactAdminSubcopyright));
        }

        [Fact]
        public void Translate_ReturnsTheKey_WhenItDoesNotExistInAnyLocale()
        {
            Assert.Equal("email:unknown.key", _service.Translate("pt", "email:unknown.key"));
        }
    }

    public class EmailTemplateServiceTests
    {
        private readonly EmailTemplateService _service;

        public EmailTemplateServiceTests()
        {
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);
            _service = new(new TranslationService());
        }

        [Fact]
        public async Task GetEmailTemplate_FillsTheClientContactEmailInTheClientLocale()
        {
            string html = await _service.GetEmailTemplate(new ContactEmailRequestDto { ClientName = "Ana", ClientLocale = "pt" }, true);

            Assert.Contains("Olá, Ana!", html);
            Assert.DoesNotContain("{{", html);
        }

        [Fact]
        public async Task GetEmailTemplate_FillsTheAdminContactEmailInEnglish()
        {
            string html = await _service.GetEmailTemplate(new ContactEmailRequestDto { ClientName = "Ana", ClientEmail = "ana@email.com", ClientMessage = "Hi!", ClientLocale = "pt" });

            Assert.Contains("Name: Ana", html);
            Assert.Contains("Message: Hi!", html);
            Assert.Contains(EmailConstants.ContactEmailSubject, html);
            Assert.DoesNotContain("{{", html);
        }

        [Fact]
        public async Task GetEmailTemplate_FillsTheConfirmationCodeEmailWithEachCodeCharacter()
        {
            ConfirmationCodeEmailRequestDto request = new() { ClientLocale = "en", ConfirmationCodeEmailType = ConfirmationCodeEmailTypeEnum.SignUp };

            string html = await _service.GetEmailTemplate(request, "a1b2");

            Assert.Contains("You received this email because you requested a sign up on my website.", html);
            Assert.Contains($"{DateTime.Now.Year} Pestana Dev.", html);
            Assert.DoesNotContain("{{", html);
        }
    }

    public class EmailServiceTests
    {
        private readonly Mock<IEmailTemplateService> _templateService = new();
        private readonly Mock<IConfirmationCodeService> _codeService = new();
        private readonly Mock<ICaptchaService> _captchaService = new();
        private readonly Mock<ISignUpService> _signUpService = new();
        private readonly Mock<IUserService> _userService = new();
        private readonly EmailService _service;

        public EmailServiceTests()
        {
            IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["email.address"] = "site@email.com",
                ["email.apppassword"] = "password",
                ["email.smtp"] = "smtp.email.com"
            }).Build();

            _captchaService.Setup(c => c.ValidateCaptchaV3(It.IsAny<string>())).ReturnsAsync(true);
            _service = new(config, _templateService.Object, _codeService.Object, _captchaService.Object, _signUpService.Object, _userService.Object);
        }

        private static ConfirmationCodeEmailRequestDto CodeRequest(ConfirmationCodeEmailTypeEnum type) => new() { ClientEmail = "user@email.com", ClientLocale = "en", ConfirmationCodeEmailType = type };

        [Fact]
        public void Constructor_Throws_WhenEmailIsNotConfigured()
        {
            Assert.Throws<InvalidOperationException>(() => new EmailService(new ConfigurationBuilder().Build(), _templateService.Object, _codeService.Object, _captchaService.Object, _signUpService.Object, _userService.Object));
        }

        [Fact]
        public async Task SendContactEmail_ReturnsForbidden_WhenCaptchaIsNotHuman()
        {
            _captchaService.Setup(c => c.ValidateCaptchaV3(It.IsAny<string>())).ReturnsAsync(false);

            EmailResponseDto response = await _service.SendContactEmail(new() { ClientEmail = "user@email.com" });

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task SendContactEmail_ReturnsBadRequest_WhenEmailIsInvalid()
        {
            EmailResponseDto response = await _service.SendContactEmail(new() { ClientEmail = "invalid" });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SendConfirmationCodeEmail_ReturnsBadRequest_WhenCodeWasAlreadySent()
        {
            _codeService.Setup(c => c.CheckIfConfirmationCodeEmailAlreadySent("user@email.com")).ReturnsAsync(true);

            SendConfirmationCodeEmailResponseDto response = await _service.SendConfirmationCodeEmail(CodeRequest(ConfirmationCodeEmailTypeEnum.SignUp));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SendConfirmationCodeEmail_ReturnsOkWithoutSending_WhenSignUpEmailIsAlreadyRegistered()
        {
            _signUpService.Setup(s => s.IsEmailAlreadyRegistered("user@email.com")).ReturnsAsync(new IsEmailAlreadyRegisteredResponseDto(isRegistered: true));

            SendConfirmationCodeEmailResponseDto response = await _service.SendConfirmationCodeEmail(CodeRequest(ConfirmationCodeEmailTypeEnum.SignUp));

            Assert.True(response.IsSuccess);
            _codeService.Verify(c => c.GenerateConfirmationCode(It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public async Task ResendConfirmationCodeEmail_ReturnsBadRequest_WhenRequestedTooSoon()
        {
            _codeService.Setup(c => c.CheckIfConfirmationCodeEmailAlreadySent("user@email.com")).ReturnsAsync(true);

            SendConfirmationCodeEmailResponseDto response = await _service.ResendConfirmationCodeEmail(CodeRequest(ConfirmationCodeEmailTypeEnum.SignUp));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            _codeService.Verify(c => c.GenerateConfirmationCode(It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
        }
    }
}
