using Shampan.Models.AuditModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class BKCommonSelectionSettings
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
        [Display(Name = "Hitorical Preform Flag")]
        public bool HitoricalPreformFlag { get; set; }
        [Display(Name = "Historical Perform Flag Desc")]
        public string HistoricalPerformFlagDesc { get; set; }
        [Display(Name = "Last Year Audit Finding Flag")]
        public bool LastYearAuditFindingFlag { get; set; }
        [Display(Name = "Last Year Audit Finding FlagDesc")]
        public string LastYearAuditFindingFlagDesc { get; set; }
        [Display(Name = "Previous YearExcept LastYearAudit FindingFlag")]
        public bool PreviousYearExceptLastYearAuditFindingFlag { get; set; }
        [Display(Name = "Previous YearExcept LastYearAudit FindingFlagDesc")]
        public string PreviousYearExceptLastYearAuditFindingFlagDesc { get; set; }
        [Display(Name = "Tech Cyber RiskFlag")]
        public bool TechCyberRiskFlag { get; set; }
        [Display(Name = "Office Size Flag")]
        public bool OfficeSizeFlag { get; set; }
        [Display(Name = "Office Significance Flag")]
        public bool OfficeSignificanceFlag { get; set; }
        [Display(Name = "Tech CyberRisk FlagDesc")]
        public string TechCyberRiskFlagDesc { get; set; }
        [Display(Name = "Staff Turnover Flag")]
        public bool StaffTurnoverFlag { get; set; }
        [Display(Name = "Staff Turnover FlagDesc")]
        public string StaffTurnoverFlagDesc { get; set; }
        [Display(Name = "Staff Training GapsFlag")]
        public bool StaffTrainingGapsFlag { get; set; }
        [Display(Name = "Staff Training GapsFlag Desc")]
        public string StaffTrainingGapsFlagDesc { get; set; }
        [Display(Name = "Strategic Initiative FlagveFlag")]
        public bool StrategicInitiativeFlagveFlag { get; set; }
        [Display(Name = "Strategic Initiative FlagDesc")]
        public string StrategicInitiativeFlagDesc { get; set; }
        [Display(Name = "Operational CompFlag")]
        public bool OperationalCompFlag { get; set; }
        [Display(Name = "Operational CompFlagDesc")]
        public string OperationalCompFlagDesc { get; set; }
        public bool Status { get; set; }
        [Display(Name = "Entry Date")]

        public string EntryDate { get; set; }
        [Display(Name = "Audit Year")]

        public string AuditYear { get; set; }
        public string AuditFiscalYear { get; set; }
        public string InfoReceiveDate { get; set; }
        public string InfoReceiveId { get; set; }
        public bool InfoReceiveFlag { get; set; }

        [Display(Name = "Fields1 Flag")]
        public bool Fields1Flag { get; set; }
        [Display(Name = "Fields1 FlagDesc")]

        public string Fields1FlagDesc { get; set; }
        [Display(Name = "Fields2 Flag")]

        public bool Fields2Flag { get; set; }
        [Display(Name = "Fields2 FlagDesc")]

        public string Fields2FlagDesc { get; set; }
        [Display(Name = "Fields3 Flag")]
        public bool Fields3Flag { get; set; }
        [Display(Name = "Fields3 FlagDesc")]

        public string Fields3FlagDesc { get; set; }
        [Display(Name = "Fields4 Flag")]
        public bool Fields4Flag { get; set; }
        [Display(Name = "Fields4 FlagDesc")]

        public string Fields4FlagDesc { get; set; }
        [Display(Name = "Fields5 Flag")]

        public bool Fields5Flag { get; set; }
        [Display(Name = "Fields5 FlagDesc")]

        public string Fields5FlagDesc { get; set; }
        [Display(Name = "Fields6 Flag")]

        public bool Fields6Flag { get; set; }
        [Display(Name = "Fields6 FlagDesc")]

        public string Fields6FlagDesc { get; set; }
        [Display(Name = "Fields7 Flag")]
        
        public bool Fields7Flag { get; set; }
        [Display(Name = "Fields7 FlagDesc")]

        public string Fields7FlagDesc { get; set; }
        [Display(Name = "Fields8 Flag")]

        public bool Fields8Flag { get; set; }
        [Display(Name = "Fields8 FlagDesc")]

        public string Fields8FlagDesc { get; set; }
        [Display(Name = "Fields9 Flag")]

        public bool Fields9Flag { get; set; }
        [Display(Name = "Fields9 FlagDesc")]

        public string Fields9FlagDesc { get; set; }
        [Display(Name = "Fields10 Flag")]

        public bool Fields10Flag { get; set; }
        [Display(Name = "Fields10 FlagDesc")]

        public string Fields10FlagDesc { get; set; }
        [Display(Name = "Fields11 Flag")]

        public bool Fields11Flag { get; set; }
        [Display(Name = "Fields11 FlagDesc")]

        public string Fields11FlagDesc { get; set; }
        [Display(Name = "Fields12 Flag")]

        public bool Fields12Flag { get; set; }
        [Display(Name = "Fields12 FlagDesc")]

        public string Fields12FlagDesc { get; set; }
        [Display(Name = "Fields13 Flag")]       
        public bool Fields13Flag { get; set; }
        [Display(Name = "Fields13 FlagDesc")]
        public string Fields13FlagDesc { get; set; }
        [Display(Name = "Fields14 Flag")]
        public bool Fields14Flag { get; set; }
        [Display(Name = "Fields14 FlagDesc")]
        public string Fields14FlagDesc { get; set; }
        [Display(Name = "Fields15 Flag")]
        public bool Fields15Flag { get; set; }
        [Display(Name = "Fields15 FlagDesc")]
        public string Fields15FlagDesc { get; set; }
        [Display(Name = "Fields16 Flag")]
        public bool Fields16Flag { get; set; }
        [Display(Name = "Fields16 FlagDesc")]
        public string Fields16FlagDesc { get; set; }
        [Display(Name = "Fields17 Flag")]
        public bool Fields17Flag { get; set; }
        [Display(Name = "Fields17 FlagDesc")]
        public string Fields17FlagDesc { get; set; }
        [Display(Name = "Fields18 Flag")]
        public bool Fields18Flag { get; set; }
        [Display(Name = "Fields18 FlagDesc")]
        public string Fields18FlagDesc { get; set; }
        [Display(Name = "Fields19 Flag")]
        public bool Fields19Flag { get; set; }
        [Display(Name = "Fields19 FlagDesc")]
        public string Fields19FlagDesc { get; set; }
        [Display(Name = "Fields20 Flag")]
        public bool Fields20Flag { get; set; }
        [Display(Name = "Fields20 FlagDesc")]
        public string Fields20FlagDesc { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedFrom { get; set; }
        public string LastUpdateBy { get; set; }
        public string LastUpdateOn { get; set; }
        public string LastUpdateFrom { get; set; }

        public Audit Audit { set; get; }

        public BKCommonSelectionSettings()
        {
            Audit = new Audit();
        }
    }
}
