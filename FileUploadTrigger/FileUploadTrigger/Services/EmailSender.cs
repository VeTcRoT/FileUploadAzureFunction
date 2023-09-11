using FileUploadTrigger.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace FileUploadTrigger.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ISendGridClient _sendGridClient;

        public EmailSender(ISendGridClient sendGridClient)
        {
            _sendGridClient = sendGridClient;
        }

        public async Task<bool> SendAsync(EmailOptions options)
        {
            var message = new SendGridMessage
            {
                From = options.From,
                Subject = options.Subject,
                HtmlContent = options.Body
            };

            message.AddTo(options.To);

            var response = await _sendGridClient.SendEmailAsync(message);

            return response.IsSuccessStatusCode;
        }
    }
}
