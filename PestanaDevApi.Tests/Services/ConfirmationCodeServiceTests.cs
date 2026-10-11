using System.Net;
using Microsoft.Extensions.Configuration;
using Moq;
using PestanaDevApi.Dtos.Responses;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Services;
using PestanaDevApi.Utils;

namespace PestanaDevApi.Tests.Services
{
    public class ConfirmationCodeServiceTests
    {
        private const string Secret = "secret";
        private const string Email = "user@email.com";
        private readonly Mock<IConfirmationCodeGenerationRepository> _repository = new();
        private readonly Mock<IConfirmedEmailsRepository> _confirmedEmails = new();
        private readonly ConfirmationCodeService _service;

        public ConfirmationCodeServiceTests()
        {
            IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { ["sha.secret"] = Secret }).Build();

            _repository.Setup(r => r.SelectOneIfTheresEmail(Email)).ReturnsAsync(true);
            _repository.Setup(r => r.SelectOneIfCodeIsStillFresh(Email)).ReturnsAsync(true);
            _repository.Setup(r => r.SelectCodeByEmail(Email)).ReturnsAsync(SHA526Factory.GenerateHmac("ab12", Secret));
            _service = new(_repository.Object, config, _confirmedEmails.Object);
        }

        [Fact]
        public void Constructor_Throws_WhenSecretIsNotConfigured()
        {
            IConfiguration emptyConfig = new ConfigurationBuilder().Build();

            Assert.Throws<InvalidOperationException>(() => new ConfirmationCodeService(_repository.Object, emptyConfig, _confirmedEmails.Object));
        }

        [Fact]
        public async Task GenerateConfirmationCode_StoresTheHashAndNotTheCode()
        {
            string code = await _service.GenerateConfirmationCode(Email);

            _repository.Verify(r => r.InsertConfirmationCode(Email, SHA526Factory.GenerateHmac(code, Secret)), Times.Once);
        }

        [Fact]
        public async Task GenerateConfirmationCode_UpdatesTheCode_WhenItIsAResend()
        {
            await _service.GenerateConfirmationCode(Email, isResend: true);

            _repository.Verify(r => r.UpdateConfirmationCode(Email, It.IsAny<string>()), Times.Once);
            _repository.Verify(r => r.InsertConfirmationCode(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task IsConfirmationCodeValid_ReturnsBadRequest_WhenCodeIsNotFresh()
        {
            _repository.Setup(r => r.SelectOneIfCodeIsStillFresh(Email)).ReturnsAsync(false);

            CheckConfirmationCodeResponse response = await _service.IsConfirmationCodeValid(new() { ClientEmail = Email, Code = "ab12" });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task IsConfirmationCodeValid_ReturnsUnauthorized_WhenCodeIsWrong()
        {
            CheckConfirmationCodeResponse response = await _service.IsConfirmationCodeValid(new() { ClientEmail = Email, Code = "zz99" });

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task IsConfirmationCodeValid_ConfirmsEmailAndDeletesCode_WhenCodeIsRight()
        {
            CheckConfirmationCodeResponse response = await _service.IsConfirmationCodeValid(new() { ClientEmail = Email, Code = "ab12" });

            Assert.True(response.IsSuccess);
            _confirmedEmails.Verify(c => c.RegisterEmailConfirmation(Email), Times.Once);
            _repository.Verify(r => r.DeleteConfirmationCode(Email), Times.Once);
        }
    }
}
