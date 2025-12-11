using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class BKRiskAssessRegulationPreferenceCBS
    {
        public int Id { get; set; }
        [Display(Name = "Branch Name")]
        public int BranchID { get; set; }
        public int ImportId { get; set; }
        [Display(Name = "Audit Year")]
        public string? AuditYear { get; set; }
        public string? Operation { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public string? Edit { get; set; }
        [Display(Name = "Audit Fiscal Year")]
        public string? AuditFiscalYear { get; set; }
        [Display(Name = "Amount")]
        public decimal RiskTxnAmount { get; set; }
        [Display(Name = "Product Flag")]

        public bool CompFinProductsFlg { get; set; }
        [Display(Name = "Risk Flag")]

        public bool GeoLocRiskFlg { get; set; }
        [Display(Name = "Internation Flag")]

        public bool InternationTxnFlg { get; set; }
        [Display(Name = "Forex Flag")]

        public bool ForexFlg { get; set; }
        [Display(Name = "High Profile Client Flag")]

        public bool HighProfileClientsFlg { get; set; }
        [Display(Name = "Corporate Client Flag")]

        public bool CorporateClientsFlg { get; set; }
        [Display(Name = "Aml Flag")]

        public bool AmlFlg { get; set; }
        [Display(Name = "Kyc Guideline Flag")]
        public List<string>? IDs { get; set; }
        public bool KycGuidelinesFlg { get; set; }
        public bool Status { get; set; }
        public Audit Audit { get; set; }
        public BKRiskAssessRegulationPreferenceCBS()
        {
            Audit = new Audit();
        }
    }
}
