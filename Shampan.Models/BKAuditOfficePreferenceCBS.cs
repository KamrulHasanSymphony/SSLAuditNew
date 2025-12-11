using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class BKAuditOfficePreferenceCBS
    {
        public int Id { get; set; }
        [Display(Name = "Branch Name")]
        public int BranchID { get; set; }
        public int ImportId { get; set; }
        public string? Operation { get; set; }
        public string? Edit { get; set; }
        [Display(Name = "Audit Year")]
        public string? AuditYear { get; set; }
        [Display(Name = "Audit Fiscal Year")]
        public string? AuditFiscalYear { get; set; }
        [Display(Name = "Historical Perform Flg")]
        public bool HistoricalPerformFlg { get; set; }
        [Display(Name = "LastYear Audit Finding Flg")]
        public bool LastYearAuditFindingsFlg { get; set; }
        [Display(Name = "Previous Finding Flg")]
        public bool PreviousYearsExceptLastYearAuditFindingsFlg { get; set; }
        [Display(Name = "Tech CyberRisk Flag")]

        public bool TechCyberRiskFlg { get; set; }
        [Display(Name = "Office Size  Flag")]

        public bool OfficeSizeFlg { get; set; }
        [Display(Name = "Office Significance  Flag")]

        public bool OfficeSignificanceFlg { get; set; }
        [Display(Name = "Staff Turnover  Flag")]

        public bool StaffTurnoverFlg { get; set; }
        [Display(Name = "Staff Training Gap Flag")]

        public bool StaffTrainingGapsFlg { get; set; }
        [Display(Name = "Strategic Initiative Flag")]

        public bool StrategicInitiativeFlg { get; set; }
        [Display(Name = "Operational Comp Flag")]

        public bool OperationalCompFlg { get; set; }   
        [Display(Name = "Entry Date")]

        public string? EntryDate { get; set; }
        [Display(Name = "Update Date")]

        public string? UpdateDate { get; set; }
        public bool Status { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public List<string>? IDs { get; set; }
        public Audit Audit { get; set; }
        public BKAuditOfficePreferenceCBS()
        {
            Audit = new Audit();
        }

    }
}
