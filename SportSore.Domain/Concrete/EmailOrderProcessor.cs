using SportSore.Domain.Abstract;
using SportSore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SportSore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailtoAddress = "orders@example.com";
        public string MailFromAddress = "sportsstore@example.com";
        public bool UseSsl = true;
        public string Username = "MySmtpUsername";
        public string Password = "MySmtpPassword";
        public string ServerName = "smtp.example.com";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"~/App_Data/sports_store_emails";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings emailSettings)
        {
            this.emailSettings = emailSettings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            using (var smtpClient = new SmtpClient
            {
                EnableSsl = emailSettings.UseSsl,
                Host = emailSettings.ServerName,
                Port = emailSettings.ServerPort,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password),
            })
            {
                if (emailSettings.WriteAsFile)
                {
                    string fullPathOfFileLocation = HttpContext.Current.Server.MapPath(emailSettings.FileLocation);
                    if (!Directory.Exists(fullPathOfFileLocation))
                        Directory.CreateDirectory(fullPathOfFileLocation);

                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = fullPathOfFileLocation;
                    smtpClient.EnableSsl = false;
                }

                var body = new StringBuilder()
                    .AppendLine("Поступил новый заказ")
                    .AppendLine("---")
                    .AppendLine("Товары:");

                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (Сумма: {2:c}", line.Quantity, line.Product.Name, subtotal);
                }

                body.AppendFormat("Всего на сумму: {0:c}", cart.ComputeTotalValue())
                    .AppendLine("---")
                    .AppendLine("Куда доставить:")
                    .AppendLine(shippingDetails.Name)
                    .AppendLine(shippingDetails.Line1)
                    .AppendLine(shippingDetails.Line2 ?? "")
                    .AppendLine(shippingDetails.Line3 ?? "")
                    .AppendLine(shippingDetails.City)
                    .AppendLine(shippingDetails.State ?? "")
                    .AppendLine(shippingDetails.Country)
                    .AppendLine(shippingDetails.Zip)
                    .AppendLine("---")
                    .AppendFormat("Подарочная упаковка: {0}", shippingDetails.GiftWrap ? "Да" : "Нет");

                var mailMessage = new MailMessage(emailSettings.MailFromAddress, emailSettings.MailtoAddress, "Новый заказ отправлен!", body.ToString());

                if (emailSettings.WriteAsFile)
                    mailMessage.BodyEncoding = Encoding.ASCII;

                smtpClient.Send(mailMessage);
            }
        }

    }
}
