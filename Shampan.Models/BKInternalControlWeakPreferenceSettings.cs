using Shampan.Models.AuditModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class BKInternalControlWeakPreferenceSettings
    {
        public int Id { set; get; }
        public int BranchID { set; get; }
        public string? Code { get; set; }
        public string Operation { get; set; }
        public string OperationStatus { get; set; }
        public string Edit { get; set; }
        [Display(Name = "BK-Audit Office Type")]
        public int BKAuditOfficeTypeId { set; get; }
        [Display(Name = "BK-Audit Office")]
        public int BKAuditOfficeId { set; get; }
        [Display(Name = "Internal Control Flag")]
        public bool InternalControlFlag { set; get; }
        [Display(Name = "Proper Documentation Flag")]
        public bool ProperDocumentationFlag { set; get; }
        [Display(Name = "Proper Reporting Flag")]
        public bool ProperReportingFlag { set; get; }
        public bool Status { set; get; }
        public string AuditYear { get; set; }
        public string AuditFiscalYear { get; set; }
        public string InfoReceiveDate { get; set; }
        public string InfoReceiveId { get; set; }
        public bool InfoReceiveFlag { get; set; }
        public Audit Audit { set; get; }

        public BKInternalControlWeakPreferenceSettings()
        {
            Audit = new Audit();
        }
    }
}
