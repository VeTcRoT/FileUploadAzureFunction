using FileUploadTrigger.Models;
using FileUploadTrigger.Services;
using Moq;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;

namespace FileUploadTrigger.Tests.Services
{
    public class EmailSenderTests
    {
        private readonly Mock<ISendGridClient> _sendGridClientMock;
        private readonly EmailOptions _emailOptions;

        public EmailSenderTests()
        {
            _sendGridClientMock = new();
            _emailOptions = new(
                "test@example.com", "recipient@example.com", "Test Email", "<p>This is a test email</p>");
        }

        [Theory]
        [InlineData(HttpStatusCode.OK, true)]
        [InlineData(HttpStatusCode.InternalServerError, false)]
        public async Task SendAsync_SendsEmail_ResultEqualExpected(HttpStatusCode statusCode, bool expected)
        {
            // Arrange
            _sendGridClientMock
                .Setup(c => c.SendEmailAsync(It.IsAny<SendGridMessage>(), CancellationToken.None))
                .ReturnsAsync(new Response(statusCode, null, null));

            var emailSender = new EmailSender(_sendGridClientMock.Object);

            // Act
            var result = await emailSender.SendAsync(_emailOptions);

            // Assert
            Assert.Equal(expected, result);

            _sendGridClientMock.Verify(
                c => c.SendEmailAsync(It.Is<SendGridMessage>(msg =>
                    msg.From == _emailOptions.From &&
                    msg.Subject == _emailOptions.Subject &&
                    msg.HtmlContent == _emailOptions.Body &&
                    msg.Personalizations[0].Tos[0] == _emailOptions.To),
                    CancellationToken.None),
                Times.Once);
        }
    }
}
