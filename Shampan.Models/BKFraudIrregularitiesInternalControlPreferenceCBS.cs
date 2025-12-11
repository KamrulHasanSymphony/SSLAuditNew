using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class BKFraudIrregularitiesInternalControlPreferenceCBS
    {
        public int Id { get; set; }
        [Display(Name = "Branch Name")]
        public int BranchID { get; set; }
        public int ImportId { get; set; }
        [Display(Name = "Audit Year")]
        public string AuditYear { get; set; }
        [Display(Name = "Audit Fiscal Year")]
        public string AuditFiscalYear { get; set; }
        public string Operation { get; set; }
        public string Edit { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        [Display(Name = "Previously Fraud Incident Flag")]

        public bool PreviouslyFraudIncidentFlg { get; set; }
        [Display(Name = "Emp MisConduct Flag")]

        public bool EmpMisConductFlg { get; set; }
        [Display(Name = "Irregularities Flag")]

        public bool IrregularitiesFlg { get; set; }
        public bool InternalControlFlg { get; set; }
        [Display(Name = "Proper Documentation Flag")]

        public bool ProperDocumentationFlg { get; set; }
        [Display(Name = "Proper Reporting Flag")]

        public bool ProperReportingFlg { get; set; }
        public bool Status { get; set; }
        public List<string>? IDs { get; set; }

        public Audit Audit { get; set; }
        public BKFraudIrregularitiesInternalControlPreferenceCBS()
        {
            Audit = new Audit();
        }
    }
}
