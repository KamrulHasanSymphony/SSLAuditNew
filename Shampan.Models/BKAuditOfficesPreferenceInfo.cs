using Shampan.Models.AuditModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class BKAuditOfficesPreferenceInfo
    {
        public int Id { set; get; }
        public int BranchID { set; get; }
        public string? Code { get; set; }
        public string Operation { get; set; }
        public string OperationStatus { get; set; }
        public string Edit { get; set; }
        [Display(Name = "BK-Audit Office Type")]
        public int BKAuditOfficeTypeId { set; get; }
        [Display(Name = "BK-Audit Branch Name")]
        public int BKAuditOfficeId { set; get; }
        [Display(Name = "Historical Perform Flag")]
        public bool HistoricalPerformFlag { set; get; }
        [Display(Name = "Audit Year")]
        public string AuditYear { set; get; }
        [Display(Name = "Audit Fiscal Year")]
        public string AuditFiscalYear { set; get; }
        [Display(Name = "Audit Perference Value")]
        public string AuditPerferenceValue { set; get; }
        public string EntryDate { set; get; }
        public bool Status { set; get; }
        public Audit Audit { set; get; }

        public BKAuditOfficesPreferenceInfo()
        {
            Audit = new Audit();
        }
    }
}
