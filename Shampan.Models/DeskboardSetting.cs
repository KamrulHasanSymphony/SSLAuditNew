using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shampan.Models
{
    public class DeshboardSettings
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string UserName { get; set; }
        [Display(Name = "User Name")]
        public string UserId { get; set; }
		[Display(Name = "Pie Chart")]
		public bool IsPieChart  { get; set; }
        [Display(Name = "Financial Data")]
        public bool IsFinancialData { get; set; }
        [Display(Name = "Planned Engagement")]
        public bool IsPlannedEngagement  { get; set; }
        [Display(Name = "Unplanned Engagement")]
        public bool IsUnplannedEngagement  { get; set; }
        [Display(Name = "Year Data")]
        public bool IsYearData  { get; set; }
        [Display(Name = "Box Data")]
        public bool IsBoxData  { get; set; }
        public string Operation { set; get; }

        public Audit Audit;
		public DeshboardSettings()
		{
			Audit = new Audit();
		}
	}
}
