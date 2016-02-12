using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Mail
{
    class Program
    {
        static void Main(string[] args)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtpGmail = new SmtpClient("Smtp.gmail.com");
            mail.From = new MailAddress("Andersmeidahl@gmail.com");
            mail.To.Add("Andersmeidahl@gmail.com");
            mail.To.Add("Nnicklas92@hotmail.com");
            mail.To.Add("Rasmushem93@hotmail.com");
            mail.Subject = "PrisTjek220 indkøbsliste";
            mail.Body = "Banan Netto 2 kr.\ntis Netto 10 kr.";

            

            smtpGmail.Port = 587;
            smtpGmail.Credentials = new NetworkCredential("pristjek220@gmail.com", "pristjek");
            smtpGmail.EnableSsl = true;
            smtpGmail.Send(mail);
        }
    }
}
