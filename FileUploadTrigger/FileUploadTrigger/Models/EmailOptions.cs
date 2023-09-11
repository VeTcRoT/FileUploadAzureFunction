using SendGrid.Helpers.Mail;

namespace FileUploadTrigger.Models
{
    public class EmailOptions
    {
        public EmailAddress From { get; set; }
        public EmailAddress To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailOptions(string from, string to, string subject, string body)
        {
            From = new(from);
            To = new(to);
            Subject = subject;
            Body = body;
        }
    }
}
