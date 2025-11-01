using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MiniHttpServer.Context;

namespace MiniHttpServer.Utils;


// TODO: тоже сделать синглтоном и добавить в конекст
public static class EmailService
{
    public static void SendEmail(string to, string subject, string message)
    {
        MailAddress fromUser = new MailAddress(
            GlobalContext.SettingsManager.Settings.EmailAddressFrom, 
            GlobalContext.SettingsManager.Settings.EmailNameFrom);
        MailAddress toUser = new MailAddress(to);
        MailMessage m = new MailMessage(fromUser, toUser);

        m.Subject = subject;
        m.Body = $"<h2>{message}</h2>";
        m.IsBodyHtml = true;

        SmtpClient smtp = new SmtpClient(
            GlobalContext.SettingsManager.Settings.SmtpHost, 
            GlobalContext.SettingsManager.Settings.SmtpPort);

        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential(
            GlobalContext.SettingsManager.Settings.EmailAddressFrom, 
            GlobalContext.SettingsManager.Settings.SmtpPassword);
        smtp.EnableSsl = true;
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtp.Send(m);
        smtp.Dispose();
    }
}