using System.Net.Mail;

namespace Consumer
{
    /// <summary>
    ///     Interface for Business logic layer for SmtpClientWrapper
    /// </summary>
    public interface ISmtpClientWrapper
    {
        /// <summary>
        ///     set and get method for SmtpClient
        /// </summary>
        SmtpClient SmtpClient { get; set; }

        /// <summary>
        ///     Send function for SmtpClient
        /// </summary>
        /// <param name="mailMessage"></param>
        void Send(MailMessage mailMessage);
    }
}