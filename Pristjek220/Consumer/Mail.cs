using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class Mail : IMail
    {
        public void SendMail(string email, ObservableCollection<StoreProductAndPrice> productListWithStore, ObservableCollection<ProductInfo> productListWithNoStore, string sum)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtpGmail = new SmtpClient("Smtp.gmail.com");
            mail.From = new MailAddress("pristjek220@gmail.com");
            mail.To.Add(email);
            mail.Subject = "PrisTjek220 indkøbsliste";
            mail.IsBodyHtml = true;

            mail.Body = generateString(productListWithStore, productListWithNoStore, sum);

            smtpGmail.Port = 587;
            smtpGmail.Credentials = new NetworkCredential("pristjek220@gmail.com", "pristjek");
            smtpGmail.EnableSsl = true;
            smtpGmail.Send(mail);
        }

        private string generateString(ObservableCollection<StoreProductAndPrice> productListWithStore,
            ObservableCollection<ProductInfo> productListWithNoStore, string sum)
        {
            DateTime dateTime = DateTime.Now;
            string bodyText = $"Kære bruger af Pristjek220<br><br>Her er den generede indkøbsseddel fra {dateTime}.";

            bodyText += "<table>";
            bodyText += "<tr>\r\n    <td ><b>Butik</b> &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td><b>Produkt</b> &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td align=\"right\"><b>Stk. pris</b> &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td align=\"right\"><b>Antal</b> &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td align=\"right\"><b>Pris</b></td>\r\n</tr>";
            bodyText = productListWithStore.Aggregate(bodyText, (current, item) => current + $"<tr>\r\n    <td>{item.StoreName} &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td>{item.ProductName} &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td align=\"right\">{item.Price} kr. &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td align=\"right\">{item.Quantity} &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td align=\"right\">{item.Sum} kr.</td>\r\n</tr>");
            bodyText += "</table>";
            bodyText += "<br>";
            bodyText += $"<p style =\"color: black\"><br>Den samlede pris for alle varene fundet i butikker er: {sum}.<br><br><p>";
            bodyText += "<br><br>";

            bodyText += "<p style =\"color: black\">Produkter der ikke findes i en forretning:</p>";
            bodyText += "<table>";
            bodyText += "<tr>\r\n    <td><b>Produkt</b> &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td align=\"right\"><b>Antal</b></td>\r\n</tr>";
            bodyText = productListWithNoStore.Aggregate(bodyText, (current, item) => current + $"<tr>\r\n    <td>{item.Name} &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td>{item.Quantity}</td>\r\n</tr>");
            bodyText += "</table>";

            bodyText += "<p style=\"color: black\">Du ønskes en billig handletur<br><br>Med Venlig Hilsen<br>PrisTjek220</p>";
            return bodyText;
        }
    }
}