using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class BKCheckListSubType
    {
        public int Id { get; set; }
        [Display(Name = "Check List Types")]
        public int BkCheckListTypesId { get; set; }
        [Display(Name = "Code")]
        public string? Code { get; set; }
        public string? Operation { get; set; }
        public string? Description { set; get; }
        public bool Status { set; get; }
        public List<string> IDs { get; set; }
        public string Edit { get; set; }

        public Audit Audit;

        public BKCheckListSubType()
        {
            Audit = new Audit();

        }
    }
}
