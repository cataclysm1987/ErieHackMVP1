using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErieHackMVP1.Models
{
    public enum SourceAffected { TapWater, Lake, Well, River, Reservoir }
    public enum ProblemAffecting { Algae, Pollution, AgriculturalRunoff, Waste, HazardousWaste, BiologicalWaste, Outage, BrokenWaterline, BoilAlert }

    public class Report
    {
        public int ReportId { get; set; }
        public string ReportName { get; set; }
        public string ReportDescription { get; set; }
        public SourceAffected Source { get; set; }
        public ProblemAffecting Problem { get; set; }
        public string ReportCounty { get; set; }
        public DateTime TimeSubmitted { get; set; }
        public DateTime TimeObserved { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}