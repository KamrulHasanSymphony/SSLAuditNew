using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shampan.Models
{
    public class BKAuditCompliance
    {
        public int Id { get; set; }
        [Display(Name = "Code")]
        public string? Code { get; set; }
        public string? Operation { get; set; }
		[Display(Name = "BK-Auidt Category")]
		public int BKAuidtCategoryId { get; set; }
        [Display(Name = "BK-Audit Office Types")]

        public int BKAuditOfficeTypesId { get; set; }
		[Display(Name = "Date")]

		public string Date { set; get; }
        public string? Description { set; get; }
        [Display(Name = "Severity")]
        public string Severity { set; get; }

		public string Edit { get; set; } = "Audit";

		public Audit Audit;

		public BKAuditCompliance()
        {
            Audit = new Audit();

        }
    }
}
