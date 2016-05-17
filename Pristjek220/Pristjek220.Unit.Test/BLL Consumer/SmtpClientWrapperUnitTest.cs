using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Consumer;
using NSubstitute;
using NUnit.Framework.Internal;
using NUnit.Framework;

namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class SmtpClientWrapperUnitTest
    {
        private ISmtpClientWrapper _smtpClient;
        private NetworkCredential _credential;
        [SetUp]
        public void SetUp()
        {
            _credential = new NetworkCredential("pristjek220@gmail.com", "pristjek");

            _smtpClient = new SmtpClientWrapper("Smtp.gmail.com", 587, _credential, true);
        }

        [Test]
        public void Constructor_TestCrendential_isPristjek220gmailcomAndPristjek()
        {
            Assert.That(_smtpClient.SmtpClient.Credentials, Is.EqualTo(_credential));
        }
        [Test]
        public void Constructor_TestEnableSsl_isTrue()
        {
            Assert.That(_smtpClient.SmtpClient.EnableSsl, Is.EqualTo(true));
        }
        [Test]
        public void Constructor_Testhost_isSmtpGmailCom()
        {
            Assert.That(_smtpClient.SmtpClient.Host, Is.EqualTo("Smtp.gmail.com"));
        }
        [Test]
        public void Constructor_port_is587()
        {
            Assert.That(_smtpClient.SmtpClient.Port, Is.EqualTo(587));
        }


        //This is a pretty shitty test, But since its not possible to stub it out, 
        //this is the only way to get 100% coverrage. its bad!!! :(
        [Test]
        public void send()
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("pristjek220@gmail.com");
            mail.Subject = "PrisTjek220 indkøbsliste";
            mail.IsBodyHtml = true;

            mail.To.Add("test@321test.dk");
            mail.Body = "test";

            Assert.Throws<InvalidOperationException>(() => _smtpClient.Send(new MailMessage()));
        }

    }
}
