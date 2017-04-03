using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using ErieHackMVP1.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using MvcPaging;

namespace ErieHackMVP1.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewBag.Title = "Home";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "About Water Alerts";

            return View();
        }

        public ActionResult FAQ()
        {
            ViewBag.Message = "Frequently Asked Questions";
            return View();
        }


        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MailMessage msz = new MailMessage();
                    msz.From = new MailAddress(vm.Email);//Email which you are getting 
                                                         //from contact us page 
                    msz.To.Add("eriehackalerts@gmail.com");//Where mail will be sent 
                    msz.Subject = vm.Subject;
                    msz.Body = vm.Message;
                    SmtpClient smtp = new SmtpClient();

                    smtp.Host = "smtp.gmail.com";

                    smtp.Port = 587;

                    smtp.Credentials = new System.Net.NetworkCredential
                    ("eriehackalerts", "Winter89!");

                    smtp.EnableSsl = true;

                    smtp.Send(msz);

                    ModelState.Clear();
                    ViewBag.Message = "Thank you for Contacting us ";
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    ViewBag.Message = $" An error occurred. Please try again! {ex.Message}";
                }
            }

            return View();
        }
        public ActionResult Error()
        {
            return View();
        }

        [Authorize]
        public ActionResult Welcome()
        {
            var welcome = new WelcomeViewModel();
            var userid = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(u => u.Id == userid);
            welcome.User = user;
            welcome.Reports =  db.Reports.ToList().Where(u => u.ApplicationUser == user);
            var last30days = DateTime.Now.AddDays(-30);
            welcome.ReportsByUserLast30Days =
                
                db.Reports.ToList().Where(u => u.ApplicationUser == user)
                    .Where(u => u.TimeSubmitted >= last30days);
            var usercounty = user.County;
            welcome.ReportsInCounty = db.Reports.ToList().Where(u => u.ReportCounty == usercounty);
            welcome.ReportsInCountyLast30Days =
                
                db.Reports.ToList().Where(u => u.ReportCounty == usercounty)
                    .Where(u => u.TimeSubmitted >= last30days);
            welcome.ReportsByUserLast30DaysInt = welcome.ReportsByUserLast30Days.Count();
            welcome.ReportsInCountyLast30DaysInt = welcome.ReportsInCountyLast30Days.Count();
            welcome.ReportsInCountyInt = welcome.ReportsInCounty.Count();
            welcome.ReportsInt = welcome.Reports.Count();

            return View(welcome);
        }
    }
}