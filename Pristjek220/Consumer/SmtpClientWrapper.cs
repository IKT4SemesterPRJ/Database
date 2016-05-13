using System.Net;
using System.Net.Mail;

namespace Consumer
{
    /// <summary>
    ///     Wrapper made to be able to test the SmtpClient, since it got no interface
    /// </summary>
    public class SmtpClientWrapper : ISmtpClientWrapper
    {
        /// <summary>
        ///     set and get method for SmtpClient
        /// </summary>
        public SmtpClient SmtpClient { get; set; }


        /// <summary>
        ///     Sets up the SmtpClient with the host, Credential, and SSL
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="networkCredential"></param>
        /// <param name="enableSsl"></param>
        public SmtpClientWrapper(string host, int port, NetworkCredential networkCredential, bool enableSsl)
        {
            SmtpClient = new SmtpClient(host, port);
            SmtpClient.Credentials = networkCredential;
            SmtpClient.EnableSsl = enableSsl;
        }

        /// <summary>
        ///     Send function for SmtpClient
        /// </summary>
        /// <param name="mailMessage"></param>
        public void Send(MailMessage mailMessage)
        {
            SmtpClient.Send(mailMessage);
        }
    }
}
