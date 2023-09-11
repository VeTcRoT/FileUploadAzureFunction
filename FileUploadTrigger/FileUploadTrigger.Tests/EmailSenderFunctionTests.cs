using Moq;
using FileUploadTrigger.Models;
using Microsoft.Extensions.Configuration;
using FileUploadTrigger.Services;
using FileUploadTrigger.Tests.Loggers;

namespace FileUploadTrigger.Tests
{
    public class EmailSenderFunctionTests
    {
        private readonly Mock<MemoryStream> _blobStreamMock;
        private readonly string _blobName;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IEmailSender> _emailSenderMock;
        private readonly Mock<IBlobService> _blobServiceMock;
        private readonly TestLogger<EmailSenderFunction> _logger;

        public EmailSenderFunctionTests()
        {
            _blobStreamMock = new();
            _blobName = "testfile.txt";
            _configurationMock = new();
            _emailSenderMock = new();
            _blobServiceMock = new();
            _logger = new();
        }

        [Fact]
        public async Task Run_Invoke_SendsEmail()
        {
            // Arrange
            var userEmail = "user@example.com";
            var metadata = new Dictionary<string, string>
            {
                { "UserEmail", userEmail }
            };

            _configurationMock.Setup(x => x["Email:From"]).Returns("sender@example.com");

            _emailSenderMock.Setup(x => x.SendAsync(It.IsAny<EmailOptions>())).ReturnsAsync(true);

            _blobServiceMock.Setup(x => x.GetSasUri(_blobName)).Returns("https://example.com/sasuri");

            var function = new EmailSenderFunction(_blobServiceMock.Object, _emailSenderMock.Object, _configurationMock.Object);

            // Act
            await function.Run(_blobStreamMock.Object, _blobName, _logger, metadata);

            // Assert
            Assert.DoesNotContain("User email not found in blob metadata.", _logger.LogMessages);
            Assert.DoesNotContain("Email was not sent.", _logger.LogMessages);
            _emailSenderMock.Verify(x => x.SendAsync(It.IsAny<EmailOptions>()), Times.Once);
        }

        [Fact]
        public async Task Run_Invoke_LogsWarning()
        {
            // Arrange
            var metadata = new Dictionary<string, string>();

            var function = new EmailSenderFunction(_blobServiceMock.Object, _emailSenderMock.Object, _configurationMock.Object);

            // Act
            await function.Run(_blobStreamMock.Object, _blobName, _logger, metadata);

            // Assert
            Assert.Contains("User email not found in blob metadata.", _logger.LogMessages);
        }
    }
}
