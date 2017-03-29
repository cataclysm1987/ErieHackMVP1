using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ErieHackMVP1.Models;
using Microsoft.AspNet.Identity;
using FluentScheduler;

namespace ErieHackMVP1
{
    public class ReportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public class MyRegistry : Registry
        {
            public MyRegistry()
            {
                Schedule<DailyAlert>().ToRunEvery(1).Days();
                Schedule<TestAlert>().ToRunEvery(20).Seconds();
            }
        }

        

        // GET: Reports
        public async Task<ActionResult> Index()
        {
            return View(await db.Reports.ToListAsync());
        }

        // GET: Reports/Details/5
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
            return View(report);
        }

        // GET: Reports/Create
        public ActionResult Create()
        {
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

        public class TestAlert : IJob
        {
            ApplicationDbContext db = new ApplicationDbContext();
            public void Execute()
            {
                

            }
        }

        internal class DailyAlert : IJob
        {
            ApplicationDbContext db = new ApplicationDbContext();
            public void Execute()
            {
                var reports = new List<Report>(db.Reports.Where(u => u.TimeSubmitted >= DateTime.Now.AddDays(-2)));
                reports.GroupBy(u => u.ReportCounty);
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
            var alertmessage = "Alert. There have been issues affecting your " + source + " in " +
                                county + ". Please check WaterAlerts.com for more details.";
        }
    }

}

   

