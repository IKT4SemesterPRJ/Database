using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using Pristjek220Data;

namespace Consumer
{
    /// <summary>
    ///     Class that is used to send a mail
    /// </summary>
    public class Mail : IMail
    {
        private readonly MailMessage _mail;
        private readonly ISmtpClientWrapper _smtpClientWrapper;
        /// <summary>
        ///     Constructor for Mail, that sets static elements
        /// </summary>
        /// <param name="smtpClient"></param>
        public Mail(ISmtpClientWrapper smtpClient)
        {
            _smtpClientWrapper = smtpClient;
            _mail = new MailMessage
            {
                From = new MailAddress("pristjek220@gmail.com"),
                Subject = "Pristjek220 indkøbsliste",
                IsBodyHtml = true
            };
        }
        /// <summary>
        ///     Send a mail with the requested lists of items to the requested email in html format
        /// </summary>
        /// <param name="email"></param>
        /// <param name="productListWithStore"></param>
        /// <param name="productListWithNoStore"></param>
        /// <param name="sum"></param>
        public void SendMail(string email, ObservableCollection<StoreProductAndPrice> productListWithStore, ObservableCollection<ProductInfo> productListWithNoStore, string sum)
        {
            _mail.To.Add(email);
            _mail.Body = generateString(productListWithStore, productListWithNoStore, sum);
            _smtpClientWrapper.Send(_mail);
        }

        private string generateString(ObservableCollection<StoreProductAndPrice> productListWithStore,
            ObservableCollection<ProductInfo> productListWithNoStore, string sum)
        {
            var dateTime = DateTime.Now;
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