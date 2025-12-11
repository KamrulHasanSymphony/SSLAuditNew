using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shampan.Models
{
    public class BKAuditInfoDetails
    {
        public int Id { get; set; }
        [Display(Name = "Code")]
        public string? Code { get; set; }
        public string? Operation { get; set; }
		[Display(Name = "BK-Audit Offece Type")]
		public int BKAuditOffeceTypeId { get; set; }
        [Display(Name = "BK-Audit Info Master")]
        public int BKAuditInfoMasterId { get; set; }
        [Display(Name = "BK-Check list Item")]
        public int BKChecklistItemId { get; set; }
        [Display(Name = "BK-CheckList SubItem")]
        public int BKCheckListSubItemId { get; set; }
		[Display(Name = "Date")]
		public string Date { set; get; }
		public bool IsFieldType { set; get; }
        public bool Status { set; get; }
		public string Edit { get; set; }

		public Audit Audit;

		public BKAuditInfoDetails()
        {
            Audit = new Audit();

        }
    }
}
