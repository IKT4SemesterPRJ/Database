using System.Net.Mail;

namespace Consumer
{
    public interface ISmtpClientWrapper
    {
        SmtpClient SmtpClient { get; set; }

        void Send(MailMessage mailMessage);
    }
}