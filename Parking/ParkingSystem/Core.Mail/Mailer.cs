using System.Net.Mail;
using System.Net;
using Core.Mail;

namespace GIVService
{
    class Mailer
    {
        public void SendEmail(string to,string link,string ads)
        {
            var fromAddress = new MailAddress(ConfigHelper.fromAddress, ConfigHelper.fromName);
            var toAddress = new MailAddress(to);
            var body = "Parking request map link:"+link+".Also please check ads from our partner:"+ads;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, ConfigHelper.fromPassword),
                Timeout = 20000
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = ConfigHelper.fromSubject,
                Body = body,
            })

            {
                smtp.Send(message);
            }
        }
    }
}
