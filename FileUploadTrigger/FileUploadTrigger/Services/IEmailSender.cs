using FileUploadTrigger.Models;
using System.Threading.Tasks;

namespace FileUploadTrigger.Services
{
    public interface IEmailSender
    {
        Task<bool> SendAsync(EmailOptions options);
    }
}
