using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Security.Cryptography;
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
            DateTime dateTime = DateTime.Now;
            mail.IsBodyHtml = true;
            bodyText = $"Kære bruger af Pristjek220<br><br>Her er den generede indkøbsseddel fra {dateTime}.";

            bodyText += "<table>";
            bodyText += "<tr>\r\n    <td ><b>Butik</b> &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td><b>Produkt</b> &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td align=\"right\"><b>Stk. pris</b> &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td align=\"right\"><b>Antal</b> &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td align=\"right\"><b>Pris</b></td>\r\n</tr>";
            foreach (var item in productList)
            {
                bodyText += $"<tr>\r\n    <td>{item.StoreName} &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td>{item.ProductName} &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td align=\"right\">{item.Price} kr. &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td align=\"right\">{item.Quantity} &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td align=\"right\">{item.Sum} kr.</td>\r\n</tr>";
            }
            bodyText += "</table>";
            bodyText += $"<br>Den samlede pris for alle varene på indkøbsseddlen er: {sum} kr.<br><br>";

            bodyText += "<p style=\"color: black\">Du ønskes en billig handletur<br><br>Med Venlig Hilsen<br>PrisTjek220</p>";

            mail.Body = bodyText;

            smtpGmail.Port = 587;
            smtpGmail.Credentials = new NetworkCredential("pristjek220@gmail.com", "pristjek");
            smtpGmail.EnableSsl = true;
            smtpGmail.Send(mail);
        }
    }
}