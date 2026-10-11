using System.Net;
using PestanaDevApi.Constants.Messages;
using PestanaDevApi.Utils;

namespace PestanaDevApi.Tests.Utils
{
    public class ApiLibTests
    {
        [Theory]
        [InlineData("user@email.com", true)]
        [InlineData("user@email", false)]
        [InlineData("useremail.com", false)]
        [InlineData("", false)]
        public void IsEmailValid_ReturnsExpected(string email, bool expected)
        {
            Assert.Equal(expected, ApiLib.IsEmailValid(email));
        }

        [Fact]
        public void GenerateRandomCode_ReturnsCodeWithRequestedLength()
        {
            Assert.Equal(4, ApiLib.GenerateRandomCode().Length);
            Assert.Equal(6, ApiLib.GenerateRandomCode(6).Length);
        }
    }

    public class DateLibTests
    {
        [Theory]
        [InlineData("2024-01-31", true)]
        [InlineData("2024-13-01", false)]
        [InlineData("31/01/2024", false)]
        public void CheckDate_ValidatesFormat(string date, bool expected)
        {
            Assert.Equal(expected, DateLib.CheckDate(date));
        }

        [Fact]
        public void IsFutureDate_ReturnsTrueOnlyForFutureDates()
        {
            string tomorrow = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd");
            string yesterday = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd");

            Assert.True(DateLib.IsFutureDate(tomorrow));
            Assert.False(DateLib.IsFutureDate(yesterday));
        }

        [Fact]
        public void GetNasaDate_ReturnsNasaFormat()
        {
            Assert.Equal("240131", DateLib.GetNasaDate("2024-01-31"));
        }
    }

    public class PasswordTests
    {
        [Fact]
        public void HashedPassword_IsValidOnlyForTheSamePassword()
        {
            string hash = HashFactory.HashPassword("myPassword");

            Assert.False(PasswordVerifier.IsPasswordNotValid("myPassword", hash));
            Assert.True(PasswordVerifier.IsPasswordNotValid("otherPassword", hash));
        }

        [Fact]
        public void IsPasswordNotValid_ReturnsTrueWhenStoredPasswordIsEmpty()
        {
            Assert.True(PasswordVerifier.IsPasswordNotValid("myPassword", ""));
        }
    }

    public class SHA526FactoryTests
    {
        [Fact]
        public void ValidateCode_ReturnsTrueOnlyForTheSameCode()
        {
            string hash = SHA526Factory.GenerateHmac("ab12", "secret");

            Assert.True(SHA526Factory.ValidateCode("ab12", hash, "secret"));
            Assert.False(SHA526Factory.ValidateCode("cd34", hash, "secret"));
        }
    }

    public class ClientSideUrlBuilderTests
    {
        private readonly Guid _boardId = Guid.Parse("3f682819-c4c4-11f1-879f-0242ac110002");

        [Theory]
        [InlineData("http://localhost:3000/", "pt", "http://localhost:3000/pt/authentication/sign-up?redirectTo=scrum-board&boardId=3f682819-c4c4-11f1-879f-0242ac110002")]
        [InlineData("http://localhost:3000", "en", "http://localhost:3000/en/authentication/sign-up?redirectTo=scrum-board&boardId=3f682819-c4c4-11f1-879f-0242ac110002")]
        [InlineData("http://localhost:3000/", "es", "http://localhost:3000/en/authentication/sign-up?redirectTo=scrum-board&boardId=3f682819-c4c4-11f1-879f-0242ac110002")]
        public void GetBoardInvitationSignUpUrl_BuildsTheLocaleSignUpUrlWithRedirectToFirst(string baseUrl, string locale, string expected)
        {
            Assert.Equal(expected, ClientSideUrlBuilder.GetBoardInvitationSignUpUrl(baseUrl, locale, _boardId));
        }
    }

    public class GetHttpMessageTests
    {
        [Fact]
        public void Get_ReturnsMessageByStatusCode()
        {
            Assert.Equal(ErrorMessages.NotFound, GetHttpMessage.Get(404));
            Assert.Equal(ErrorMessages.Forbidden, GetHttpMessage.Get(HttpStatusCode.Forbidden));
            Assert.Equal(ErrorMessages.DefaultMessage, GetHttpMessage.Get(418));
        }
    }
}
