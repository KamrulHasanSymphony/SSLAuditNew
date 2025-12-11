using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class CheckListItem
    {
        public int Id { get; set; }
        [Display(Name = "BK-CheckList Types")]
        public int BkCheckListTypesId { get; set; }
        [Display(Name = "BK-CheckList SubTypes")]
        public int BKCheckListSubTypesId { get; set; }
        [Display(Name = "Code")]
        public string? Code { get; set; }
        public string? Operation { get; set; }
        public string? Description { set; get; }
        [Display(Name = "Field Type")]

        public string FieldType { set; get; }

        [Display(Name = "Check ListItem")]
        public bool IsCheckListItem { set; get; }
        [Display(Name = "Field Type")]
        public bool IsFieldType { set; get; }
        public bool Status { set; get; }
        public List<string> IDs { get; set; }
        public string Edit { get; set; }

        public Audit Audit;

        public CheckListItem()
        {
            Audit = new Audit();

        }
    }
}
