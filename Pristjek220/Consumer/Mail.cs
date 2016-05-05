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
        private MailMessage _mail;
        private ISmtpClientWrapper smtpClientWrapper;
        public Mail(ISmtpClientWrapper SmtpClient)
        {
            smtpClientWrapper = SmtpClient;
            _mail = new MailMessage();
            _mail.From = new MailAddress("pristjek220@gmail.com");
            _mail.Subject = "Pristjek220 indkøbsliste";
            _mail.IsBodyHtml = true;
        }
        public void SendMail(string email, ObservableCollection<StoreProductAndPrice> productListWithStore, ObservableCollection<ProductInfo> productListWithNoStore, string sum)
        {
            _mail.To.Add(email);
            _mail.Body = generateString(productListWithStore, productListWithNoStore, sum);
            smtpClientWrapper.Send(_mail);
        }

        private string generateString(ObservableCollection<StoreProductAndPrice> productListWithStore,
            ObservableCollection<ProductInfo> productListWithNoStore, string sum)
        {
            DateTime dateTime = DateTime.Now;
            string bodyText = $"Kære bruger af Pristjek220,<br><br>Her er den generede indkøbsliste fra {dateTime}.<br><br>";

            bodyText += "<table>";
            bodyText += "<tr>\r\n    <td ><b>Butik</b> &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td><b>Produkt</b> &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td align=\"right\"><b>Stk. pris</b> &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td align=\"right\"><b>Antal</b> &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td align=\"right\"><b>Pris</b></td>\r\n</tr>";
            bodyText = productListWithStore.Aggregate(bodyText, (current, item) => current + $"<tr>\r\n    <td>{item.StoreName} &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td>{item.ProductName} &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td align=\"right\">{item.Price} kr. &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td align=\"right\">{item.Quantity} &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td align=\"right\">{item.Sum} kr.</td>\r\n</tr>");
            bodyText += "</table>";
            bodyText += "<br>";
            bodyText += $"<p style =\"color: black\">Den samlede pris for alle produkterne fundet i forretningerne er: <u>{sum}</u>.<br><br><p>";
            bodyText += "<br>";

            bodyText += "<p style =\"color: black\">Produkter der ikke findes i en forretning i Pristjek220:</p>";
            bodyText += "<table>";
            bodyText += "<tr>\r\n    <td><b>Produkt</b> &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td align=\"right\"><b>Antal</b></td>\r\n</tr>";
            bodyText = productListWithNoStore.Aggregate(bodyText, (current, item) => current + $"<tr>\r\n    <td>{item.Name} &nbsp;&nbsp;&nbsp;&nbsp;</td>\r\n    <td>{item.Quantity}</td>\r\n</tr>");
            bodyText += "</table>";

            bodyText += "<p style=\"color: black\"><br>Du ønskes en billig indkøbstur.<br><br>Med venlig hilsen<br>Pristjek220</p>";
            return bodyText;
        }
    }
}