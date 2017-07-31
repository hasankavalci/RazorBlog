using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using BlogLayer.CF;
using BlogLayer.Models.DataModel;
using System.Web.Mvc;

namespace BlogLayer.Models.Helper
{
    public class Mail
    {
        public bool MailGonder(string GelenMail, string Konu ,string Mesaj)
        {
            
            
            try
            {
                RazerBlogContext _db = new RazerBlogContext();
                string MailFrom = _db.SistemMails.FirstOrDefault(x => x.SistemMailID == 1).Mail;
                string MailPassword = _db.SistemMails.FirstOrDefault(x => x.SistemMailID == 1).Password;
                new SmtpClient
                {                   
                    Host = "Smtp.Gmail.com",
                    Port = 465,
                    EnableSsl = true,
                    Timeout = 10000,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(MailFrom, MailPassword)
                }.Send(new MailMessage { From = new MailAddress(MailFrom, ""), To = { MailFrom }, Subject = Konu, Body = Mesaj, BodyEncoding = Encoding.UTF8 });
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}