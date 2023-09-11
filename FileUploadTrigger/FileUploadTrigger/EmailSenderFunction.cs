using System.Collections.Generic;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using FileUploadTrigger.Services;
using FileUploadTrigger.Models;
using Microsoft.Extensions.Configuration;

namespace FileUploadTrigger
{
    [StorageAccount("BlobConnectionString")]
    public class EmailSenderFunction
    {
        private readonly IBlobService _blobService;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public EmailSenderFunction(IBlobService blobService, IEmailSender emailSender, IConfiguration configuration)
        {
            _blobService = blobService;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        [FunctionName("EmailSenderFunction")]
        public async Task Run(
            [BlobTrigger("fileupload/{name}")] Stream myBlob,
            string name,
            ILogger log,
            IDictionary<string, string> metadata)
        {
            if (metadata.TryGetValue("UserEmail", out var userEmail))
            {
                var uri = _blobService.GetSasUri(name);

                var emailOptions = new EmailOptions(
                    _configuration["Email:From"],
                    userEmail,
                    _configuration["Email:Subject"],
                    $"Your file '{name}' has been uploaded successfully. You can access it here: <a href=\"{uri}\">Link</a>. <strong>This link will be available for 1 hour.</strong>");

                var isSuccessStatusCode = await _emailSender.SendAsync(emailOptions);

                if (!isSuccessStatusCode)
                    log.LogWarning("Email was not sent.");
            }
            else
            {
                log.LogWarning("User email not found in blob metadata.");
            }
        }
    }
}
