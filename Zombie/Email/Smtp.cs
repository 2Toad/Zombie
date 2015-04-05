using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Zombie.Email
{
    /// <summary>
    /// A utility class for working with SMTP.
    /// </summary>
    public static class Smtp
    {
        /// <summary>
        /// A satic instance of the Boilerplate template.
        /// </summary>
        public static string Boilerplate
        {
            get
            {
                return _boilerplate ?? (_boilerplate = GetTemplateFile("_Boilerplate"));
            }
        }
        private static string _boilerplate;

        /// <summary>
        /// Asynchronously sends an email using the specified <paramref name="template"/>.
        /// </summary>
        /// <param name="from">The sender of the email.</param>
        /// <param name="to">The receipient of the email.</param>
        /// <param name="subject">The email subject.</param>
        /// <param name="template">The template to use for the email body.</param>
        /// <param name="handlebars">Handlebar expressions for the template.</param>
        public static async Task Send(string from, string to, string subject, string template, params Handlebars[] handlebars)
        {
            using (var message = new MailMessage(new MailAddress(from), new MailAddress(to)))
            {
                message.Subject = subject;
                message.Body = await GetHtml(template, handlebars);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient()) await smtp.SendMailAsync(message);
            }
        }

        private static async Task<string> GetHtml(string template, Handlebars[] handlebars)
        {
            var html = await Handlebars.Parse(GetTemplateFile(template), handlebars);
            return await Handlebars.Parse(Boilerplate, Handlebars.New("message", html));
        }

        private static string GetTemplateFile(string template)
        {
            var file = HostingEnvironment.MapPath("/Email/Templates/" + template + ".html");
            var html = File.ReadAllText(file);

            // Strip CSS comments
            html = Regex.Replace(html, @"/\*.+?\*/", "", RegexOptions.Singleline);
            // Strip HTML comments
            return Regex.Replace(html, @"<!--(?!\[).*?(?!<\])-->", "", RegexOptions.Singleline);
        }
    }
}
