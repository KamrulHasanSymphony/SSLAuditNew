using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shampan.Models
{
    public class BKAuditTemlateMaster
    {
        public int Id { get; set; }
        [Display(Name = "Code")]
        public string? Code { get; set; }
        [Display(Name = "BK-Audit Office Type")]
        public int BKAuditOfficeTypeId { get; set; }
        [Display(Name = "BK-Audit Category")]
        public int BKAuditCategoryId { get; set; }
        public string? Operation { get; set; }
        public string? Description { set; get; }
        public bool Status { set; get; }
		public string Edit { get; set; }
        public List<BKAuditTemplateDetails> BKAuditTemplateDetailsList { set; get; }

        public Audit Audit;
		public BKAuditTemlateMaster()
        {
            Audit = new Audit();
            BKAuditTemplateDetailsList = new List<BKAuditTemplateDetails>();
        }
    }
}
