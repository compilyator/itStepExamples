namespace SmtpExample
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using System.Text;

    class Program
    {
        private static SmtpClient GetClient()
        {
            var smtpClient = new SmtpClient();
            return smtpClient;
        }

        private static MailMessage GetMessage()
        {
            var message = new MailMessage
                              {
                                  From = new MailAddress("bigBill@microsoft.com", "Bill Gates"),
                                  Subject = "Smtp test",
                                  SubjectEncoding = Encoding.UTF8,
                                  Body = "<h3>Привет!</h3><br>Вот девахи которых ты просил",
                                  IsBodyHtml = true
                              };
            var email = ConfigurationManager.AppSettings["ToEmail"];
            var userName = ConfigurationManager.AppSettings["ToUserName"];
            message.To.Add(new MailAddress(email, userName));
            Directory.GetFiles($@"{Directory.GetCurrentDirectory()}\Images")
                .ToList()
                .ForEach(file => message.Attachments.Add(new Attachment(file)));

            return message;
        }

        static void Main()
        {
            var smtpClient = GetClient();
            var message = GetMessage();
            smtpClient.Send(message);
            Console.WriteLine("Success!");
        }
    }
}