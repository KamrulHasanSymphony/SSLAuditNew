using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shampan.Models
{
    public class BKAuditOfficeType
    {
        public int Id { get; set; }
        [Display(Name = "Code")]
        public string? Code { get; set; }
        public string? Operation { get; set; }

        public string? Name { set; get; }
        [Display(Name = "Status")]
        public bool Status { set; get; }
		public string Edit { get; set; } = "Audit";
		public Audit Audit;
		public BKAuditOfficeType()
        {
            Audit = new Audit();

        }
    }
}
