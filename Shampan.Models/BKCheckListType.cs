using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shampan.Models
{
    public class BKCheckListType
    {
        public int Id { get; set; }
        [Display(Name = "Code")]
        public string? Code { get; set; }
        public string? Operation { get; set; }
		[Display(Name = "Bk-Audit Compliance")]
		public int BkAuditCompliancesId { get; set; }
        public string? Description { set; get; }
        public bool Status { set; get; }
		public string Edit { get; set; } = "Audit";
		public Audit Audit;
		public BKCheckListType()
        {
            Audit = new Audit();
        }
    }
}
