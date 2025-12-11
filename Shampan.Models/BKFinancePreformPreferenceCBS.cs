using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class BKFinancePreformPreferenceCBS
    {
        public int Id { get; set; }
        [Display(Name = "Branch Name")]
        public int BranchID { get; set; }
        public int ImportId { get; set; }
        [Display(Name = "Audit Year")]
        public string AuditYear { get; set; }
        public string Operation { get; set; }
        public string Edit { get; set; }
        [Display(Name = "Audit Fiscal Year")]
        public string AuditFiscalYear { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        [Display(Name = "Financial Perform Flag")]
        public bool FinancialPerformFlg { get; set; }
        [Display(Name = "Fund Available  Flag")]
        public bool FundAvailableFlg { get; set; }
        [Display(Name = "Mis-Management Client  Flag")]
        public bool MisManagementClientsFlg { get; set; }
        public bool EfficiencyFlg { get; set; }
        [Display(Name = "Npls Large  Flag")]

        public bool NplsLargeFlg { get; set; }
        [Display(Name = "Large Txn Manage  Flag")]

        public bool LargeTxnManageFlg { get; set; }
        public bool HighValueAssetManageFlg { get; set; }
        [Display(Name = "Security Measure Staff Flag")]

        public bool SecurityMeasuresStaffFlg { get; set; }
        [Display(Name = "Budget Flag")]

        public bool BudgetMgtFlg { get; set; }
        [Display(Name = "Significant Loss Flag")]

        public bool SignificantLossesFlg { get; set; }
        public bool Status { get; set; }
        public List<string>? IDs { get; set; }

        public Audit Audit { get; set; }
        public BKFinancePreformPreferenceCBS()
        {
            Audit = new Audit();
        }
    }
}
