using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    public class SmtpClientWrapper : ISmtpClientWrapper
    {
        public SmtpClient SmtpClient { get; set; }
        public SmtpClientWrapper(string host, int port, NetworkCredential networkCredential, bool enableSSL)
        {
            SmtpClient = new SmtpClient(host, port);
            SmtpClient.Credentials = networkCredential;
            SmtpClient.EnableSsl = enableSSL;
        }
        public void Send(MailMessage mailMessage)
        {
            SmtpClient.Send(mailMessage);
        }
    }
}
