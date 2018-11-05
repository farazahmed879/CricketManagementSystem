
// using SendGrid's C# Library
// https://github.com/sendgrid/sendgrid-csharp
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace WebApp.Extentions
{
    public static class EmailExtensions
    {
        public static async Task Execute(string email, string userName, string htmlString, string subject)
        {
            //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
            // var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
            var client = new SendGridClient("");
            var from = new EmailAddress("scoreexec@gmail.com", "ScoreExec");
          //  var subject = "Email Confirmation";
            var to = new EmailAddress(email, userName);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlString);
            var response = await client.SendEmailAsync(msg);
        }
    }
}