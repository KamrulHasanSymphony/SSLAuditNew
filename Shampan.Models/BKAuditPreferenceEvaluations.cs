using Shampan.Models.AuditModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class BKAuditPreferenceEvaluations
    {
        public int Id { set; get; }
        public int BranchID { set; get; }
        public string? Code { get; set; }
        public string Operation { get; set; }
        public string OperationStatus { get; set; }
        public string EvaluationOperation { get; set; }
        public string Edit { get; set; }
        public string BranchCode { get; set; }

        [Display(Name = "BK-Audit Branch Name")]
        public int BKAuditOfficeId { set; get; }
        [Display(Name = "Common Setting Value Min")]
        public decimal FlagPercentFromCommonSettingSelectedValuesMin { set; get; }
        
        [Display(Name = "Common Setting Value Max")]
        public decimal FlagPercentFromCommonSettingSelectedValuesMax { set; get; }
        [Display(Name = "Risk Assess Value Min")]
        public decimal FlagPercentFromRiskAssessSelectedValuesMin { set; get; }
        [Display(Name = "Risk Assess Value Max")]
        public decimal FlagPercentFromRiskAssessSelectedValuesMax { set; get; }
        [Display(Name = "Regu-Compliance Value Min")]
        public decimal FlagPercentFromReguCompliancesSelectedValuesMin { set; get; }
        [Display(Name = "Regu-Compliance Value Max")]
        public decimal FlagPercentFromReguComliancesSelectedValuesMax { set; get; }
        [Display(Name = "Finance Perform Value Min")]
        public decimal FlagPercentFromFinancePerformSelectedValuesMin { set; get; }
        [Display(Name = "Finance Perform Value Max")]
        public decimal FlagPercentFromFinancePerformSelectedValuesMax { set; get; }
        [Display(Name = "Fraud Irrgularitie Value Min")]
        public decimal FlagPercentFromFraudIrrgularitiesSelectedValuesMin { set; get; }
        [Display(Name = "Fraud Irrgularitie Value Max")]
        public decimal FlagPercentFromFraudIrregularitiesSelectedValuesMax { set; get; }
        [Display(Name = "Internal Control Value Min")]
        public decimal FlagPercentFromInternalControlWeakSelectedValuesMin { set; get; }
        [Display(Name = "Internal Control Value Max")]
        public decimal FlagPercentFromInternalControlWeakSelectedValuesMax { set; get; }
        public string EntryDate { set; get; }
        public bool Status { set; get; }
        public Audit Audit { set; get; }

        public BKAuditPreferenceEvaluations()
        {
            Audit = new Audit();
        }
    }
}
