using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shampan.Models
{
    public class BKAuditTemplateDetails
    {
        public int Id { get; set; }
        public int AuditId { get; set; }
        public int BKAuditOfficeTypeId { get; set; }
        public int BKAuditTemlateMasterId { get; set; }
        public int BKCheckListItemId { get; set; }
        public int BKCheckListSubTypeId { get; set; }
        public int BKAuditComplianceId { get; set; }
        public int BKCheckListTypeId { get; set; }
        public bool IsFieldType { get; set; }
        public bool Status { get; set; }
        public string? Operation { get; set; }
        public string? Description { set; get; }
		public string Edit { get; set; }

		public Audit Audit;
        public List<MappingData> MappingData { set; get; }
        public List<CheckListItem> CheckListItemList { set; get; }
        public BKAuditTemplateDetails()
        {
            Audit = new Audit();
            MappingData = new List<MappingData>();
            CheckListItemList = new List<CheckListItem>();
        }
    }
}
