using FileUploadTrigger;
using FileUploadTrigger.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace FileUploadTrigger
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {

            builder.Services.AddSendGrid(options =>
                options.ApiKey = builder.GetContext().Configuration["SendGridApiKey"]);

            builder.Services.AddTransient<IEmailSender, EmailSender>();
        }
    }
}
