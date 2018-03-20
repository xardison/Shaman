using System.Net.Mail;
using System.Text;
using Shaman.Common.Env;

namespace Shaman.Common.Mail
{
    public interface IMailer
    {
        void SendMail(Letter letter);
    }

    internal class Mailer : IMailer
    {
        private const string HtmlTemplateMail =
            @"<!DOCTYPE html><html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml""><head><meta http-equiv=""content-type"" content=""text/html; charset=UTF-8""></head>" +
            @"<body><tt>{body}</tt></body></html>";

        private const string Host = AppConfig.MailHost;
        private const int Port = AppConfig.MailPort;

        public void SendMail(Letter letter)
        {
            if (letter.To == null || letter.To.Count == 0)
            {
                return;
            }

            using (letter)
            {
                using (var mail = LetterToMailMessage(letter))
                {
                    using (var client = new SmtpClient(Host, Port))
                    {
                        client.Send(mail);
                    }
                }
            }
        }

        private MailMessage LetterToMailMessage(Letter letter)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(letter.From.Email),
                Body = HtmlTemplateMail.Replace("{body}", letter.Body),
                BodyEncoding = Encoding.UTF8,
                Subject = letter.Subject.Replace("\r\n", " ").Replace('\r', ' ').Replace('\n', ' '),
                SubjectEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };

            foreach (var receiver in letter.To)
            {
                if (!string.IsNullOrEmpty(receiver.Email))
                {
                    mailMessage.To.Add(new MailAddress(receiver.Email, receiver.Name, Encoding.UTF8));
                }
            }

            foreach (var attachment in letter.Attachments)
            {
                mailMessage.Attachments.Add(attachment);
            }

            return mailMessage;
        }
    }
}