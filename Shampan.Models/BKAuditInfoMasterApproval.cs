using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shampan.Models
{
    public class BKAuditInfoMasterApproval
    {
        public int Id { get; set; }
        [Display(Name = "Code")]
        public string? Code { get; set; }
        public string? Operation { get; set; }
		[Display(Name = "BK-Audit Office Type")]
		public int BKAuditOfficeTypeId { get; set; }
        [Display(Name = "BK-Audit Info Master")]
        public int BKAuditInfoMasterId { get; set; }
		[Display(Name = "Date")]
		public string Date { set; get; }
        public bool AuditApprovalFlag { set; get; }
        public string ApprovalAuthorityDesc { set; get; }
        public string LastApprovedBy { set; get; }
        public string LastApprovedDate { set; get; }
        public bool Status { set; get; }
		public string Edit { get; set; }

		public Audit Audit;

		public BKAuditInfoMasterApproval()
        {
            Audit = new Audit();

        }
    }
}
