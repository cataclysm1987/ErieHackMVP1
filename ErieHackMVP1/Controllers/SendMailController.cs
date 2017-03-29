using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using ErieHackMVP1.Models;
using Microsoft.AspNet.Identity;
using MailMessage = System.Net.Mail.MailMessage;

namespace ErieHackMVP1.Controllers
{
    public class SendMailController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: /SendMail/  
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ViewResult Index(Mail objModelMail)
        {
            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(objModelMail.To);
                mail.From = new MailAddress(objModelMail.From);
                mail.Subject = objModelMail.Subject;
                string Body = objModelMail.Body;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential
                ("eriehackalerts", "Winter89!");// Enter senders User name and password
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return View("Index", objModelMail);
            }
            return View();
        }

        public void SendAlert(Mail alert)
        {
            
        }

        
    }
}