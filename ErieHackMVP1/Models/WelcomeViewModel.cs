using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErieHackMVP1.Models
{
    public class WelcomeViewModel
    {
        public ApplicationUser User { get; set; }
        public IEnumerable<Report> Reports { get; set; }
        public IEnumerable<Report> ReportsInCounty { get; set; }
        public IEnumerable<Report> ReportsInCountyLast30Days { get; set; }
        public IEnumerable<Report> ReportsByUserLast30Days { get; set; }

        public int ReportsInt { get; set; }
        public int ReportsInCountyInt { get; set; }
        public int ReportsInCountyLast30DaysInt { get; set; }
        public int ReportsByUserLast30DaysInt { get; set; }

    }
}