using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class AuditPoint
    {
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string PId { get; set; }
        //public int PId { get; set; }
        public int AuditTypeId { get; set; }
        public int AuditPointId { get; set; }
        [Display(Name = "Name")]
        public int PIdData { get; set; }
        [Display(Name = "Code")]
        public string? Code { get; set; }
        public string? Operation { get; set; }
        public string AuditType { set; get; }
        [Display(Name = "Weight Persent")]
        public int WeightPersent { set; get; }
        public int P_Mark { set; get; }
        [Display(Name = "Level")]
        public int P_Level { set; get; }
        [Display(Name = "Level")]
        public int P_LevelData { set; get; }
        public string Edit { get; set; }

        public Audit Audit;

        public AuditPoint()
        {
            Audit = new Audit();

        }
    }
}
