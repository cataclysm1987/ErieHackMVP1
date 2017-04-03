using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using ErieHackMVP1.Models;
using Microsoft.AspNet.Identity;
using FluentScheduler;
using MvcPaging;
using PagedList;

namespace ErieHackMVP1
{
    public class ReportsController : Controller
    {
        private const int _defaultpagesize = 10;
        private IList<Report> _reports = new List<Report>();
        private readonly string[] _allcategories = new string[3] { "Shoes", "Electronics", "Food" };

        private ApplicationDbContext db = new ApplicationDbContext();

        public class MyRegistry : Registry
        {
            public MyRegistry()
            {
                Schedule<DailyAlert>().ToRunEvery(1).Days().At(12, 0);
            }
        }



        public ViewResult BrowseReports(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var reports = from s in db.Reports
                          select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                reports = reports.Where(s => s.ReportDescription.Contains(searchString)
                                       || s.ReportName.Contains(searchString)
                                       || s.ReportCounty.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    reports = reports.OrderByDescending(s => s.ReportName);
                    break;
                case "Date":
                    reports = reports.OrderBy(s => s.TimeSubmitted);
                    break;
                case "date_desc":
                    reports = reports.OrderByDescending(s => s.TimeSubmitted);
                    break;
                default:  // Name ascending 
                    reports = reports.OrderBy(s => s.ReportName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(reports.ToPagedList(pageNumber, pageSize));
        }

        // GET: Reports
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var userid = User.Identity.GetUserId();

            return View(await db.Reports.Where(u => u.ApplicationUser.Id == userid).ToListAsync());
        }


        [Authorize]
        public  ActionResult InMyArea(int? page)
        {
            var userid = User.Identity.GetUserId();
            var currentuser = db.Users.FirstOrDefault(u => u.Id == userid);
            var county = currentuser.County;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(db.Reports.Where(u => u.ReportCounty == county).ToPagedList(pageNumber, pageSize));
        }


        // GET: Reports/Details/5
        [Authorize]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = await db.Reports.FindAsync(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            if (report.ApplicationUser.Id != User.Identity.GetUserId())
                return View("Index");
            return View(report);
        }

        public async Task<ActionResult> ViewReport(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = await db.Reports.FindAsync(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // GET: Reports/Create
        [Authorize]
        public ActionResult Create()
        {
            var userid = User.Identity.GetUserId();
            var recent = DateTime.Now.AddDays(-2);
            var recentreport = db.Reports.Where(u => u.TimeSubmitted >= recent);
            var usercount = recentreport.Count(u => u.ApplicationUser.Id == userid);
            if (usercount > 0)
                return RedirectToAction("Wait", "Reports");
            return View();
        }

        // POST: Reports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ReportId,ReportName,ReportDescription,Source,Problem,ReportCounty,TimeSubmitted,TimeObserved")] Report report)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);
                report.TimeSubmitted = DateTime.Now;
                report.ReportCounty = currentUser.County;
                report.ApplicationUser = currentUser;
                db.Reports.Add(report);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(report);
        }

        // GET: Reports/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = await db.Reports.FindAsync(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            var userid = User.Identity.GetUserId();
            var currentuser = db.Users.FirstOrDefault(u => u.Id == userid);
            if (report.ApplicationUser != currentuser)
                return View("Index");
            return View(report);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ReportId,ReportName,ReportDescription,Source,Problem,ReportCounty,TimeSubmitted,TimeObserved")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Entry(report).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(report);
        }

        // GET: Reports/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = await db.Reports.FindAsync(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            var userid = User.Identity.GetUserId();
            var currentuser = db.Users.FirstOrDefault(u => u.Id == userid);
            if (report.ApplicationUser != currentuser)
                return View("Index");
            return View(report);
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Report report = await db.Reports.FindAsync(id);
            db.Reports.Remove(report);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        internal class DailyAlert : IJob
        {
            ApplicationDbContext db = new ApplicationDbContext();
            public void Execute()
            {
                var recent = DateTime.Now.AddDays(-2);
                var reports = new List<Report>(db.Reports.Where(u => u.TimeSubmitted >= recent));
                var counties = db.Reports.Select(u => u.ReportCounty).Distinct();
                foreach (var county in counties)
                {
                    if (reports.Count(u => u.ReportCounty == county) >= 5)
                    {
                        var countyreports = reports.Where(u => u.ReportCounty == county);
                        //Count the number of sources affected. If any source is above 5, begin generating report.
                        var lake = countyreports.Count(u => u.Source == SourceAffected.Lake);
                        var reservoir = countyreports.Count(u => u.Source == SourceAffected.Reservoir);
                        var tapwater = countyreports.Count(u => u.Source == SourceAffected.TapWater);
                        var well = countyreports.Count(u => u.Source == SourceAffected.Well);
                        var river = countyreports.Count(u => u.Source == SourceAffected.River);
                        if (river >= 5)
                        {
                             SubmitAlert(county, "river");
                        }
                        if (well >= 5)
                        {
                            SubmitAlert(county, "well");
                        }
                        if (tapwater >= 5)
                        {
                            SubmitAlert(county, "tap water");
                        }
                        if (reservoir >= 5)
                        {
                            SubmitAlert(county, "reservoir");
                        }
                        if (lake >= 5)
                        {
                            SubmitAlert(county, "lake");
                        }
                    }
                }
            }
        }


        public static void SubmitAlert(string county, string source)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var alertmessage = "Alert. There have been issues affecting your " + source + " in " +
                                county + ". Please check WaterAlerts.com for more details.";
            foreach (var user in db.Users)
            {
                if (user.County == county && user.IsSubscribedToUpdates == YesNo.Yes)
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add(user.SMSRoute);
                    mail.From = new MailAddress("eriehackalerts@gmail.com");
                    mail.Subject = "Water alert in " + county + " County";
                    mail.Body = alertmessage;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential
                    ("eriehackalerts", "Winter89!");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

        public ViewResult Wait()
        {
            return View();
        }

        
    }

}

   

