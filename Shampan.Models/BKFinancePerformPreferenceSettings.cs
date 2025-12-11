using Shampan.Models.AuditModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class BKFinancePerformPreferenceSettings
    {
        public int Id { set; get; }
        public int BranchID { set; get; }
        public int ImportId { set; get; }
        public string? Code { get; set; }
        public string Operation { get; set; }
        public string OperationStatus { get; set; }
        public string Edit { get; set; }
        [Display(Name = "BK-Audit Office Type")]
        public int BKAuditOfficeTypeId { set; get; }
        [Display(Name = "BK-Audit Branch Name")]
        public int BKAuditOfficeId { set; get; }
        [Display(Name = "Fund Available Flag")]
        public bool FundAvailableFlag { set; get; }
        [Display(Name = "Mis-Management Clinents Flag")]
        public bool MisManagementClinentsFlag { set; get; }
        [Display(Name = "Efficiencty Flag")]
        public bool EfficienctyFlag { set; get; }
        [Display(Name = "NPLS Large Flag")]
        public bool NPLSLargeFlag { set; get; }
        [Display(Name = "Large TxnManage Flag")]
        public bool LargeTxnManageFlag { set; get; }
        [Display(Name = "HighValue Asset MangeFlag")]
        public bool HighValueAssetMangeFlag { set; get; }
        [Display(Name = "Budget MgtFlag")]
        public bool BudgetMgtFlag { set; get; }
        public bool Status { set; get; }

        public string AuditYear { get; set; }
        public string AuditFiscalYear { get; set; }
        public string InfoReceiveDate { get; set; }
        public string InfoReceiveId { get; set; }
        public bool InfoReceiveFlag { get; set; }

        public Audit Audit { set; get; }

        public BKFinancePerformPreferenceSettings()
        {
            Audit = new Audit();
        }
    }
}
