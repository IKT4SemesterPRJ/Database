using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Pristjek220Data;

namespace Consumer
{
    public class Mail
    {
        public void SendMail(string email, List<StoreProductAndPrice> productList, double sum)
        {
            string bodyText = string.Empty;
            MailMessage mail = new MailMessage();
            SmtpClient smtpGmail = new SmtpClient("Smtp.gmail.com");
            mail.From = new MailAddress("pristjek220@gmail.com");
            mail.To.Add(email);
            mail.Subject = "PrisTjek220 indkøbsliste";
            DateTime dateTime = new DateTime();
            bodyText = $"Kære bruger af Pristjek220\n\nHer er den generede indkøbsseddel fra {dateTime.TimeOfDay}.\n\nButik\tProdukt\tStk. pris\tAntal\tPris\n";
            foreach (var item in productList)
            {
                bodyText = bodyText + "\n" + (item.StoreName + "\t" + item.ProductName + "\t" + item.Price + "\t" + item.Quantity + "\t" + item.Sum);
            }
            bodyText = bodyText + "\n\nMed Venlig Hilsen\nPristjek220";
            mail.Body = bodyText;


            smtpGmail.Port = 587;
            smtpGmail.Credentials = new NetworkCredential("pristjek220@gmail.com", "pristjek");
            smtpGmail.EnableSsl = true;
            smtpGmail.Send(mail);
        }
    }
}