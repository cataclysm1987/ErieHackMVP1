using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ErieHackMVP1.Models
{
    public enum SourceAffected { TapWater, Lake, Well, River, Reservoir }
    public enum ProblemAffecting { Algae, Pollution, AgriculturalRunoff, Waste, HazardousWaste, BiologicalWaste, Outage, BrokenWaterline, BoilAlert }

    public class Report
    {
        public int ReportId { get; set; }
        [Required]
        [Display(Name = "Report Name")]
        public string ReportName { get; set; }
        [Required]
        [Display(Name = "Report Description")]
        public string ReportDescription { get; set; }
        [Required]
        [Display(Name = "What Source is Affected?")]
        public SourceAffected Source { get; set; }
        [Required]
        [Display(Name = "What is the Problem?")]
        public ProblemAffecting Problem { get; set; }
        [Required]
        [Display(Name = "What County is This In?")]
        public string ReportCounty { get; set; }
        public DateTime TimeSubmitted { get; set; }
        [Required]
        [Display(Name = "When Did You Observe This?")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TimeObserved { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}