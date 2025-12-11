using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class MappingData
    {
        public int Id { set; get; }
        public int BKAuditOfficeTypeId { set; get; }
        public string BKAuditOfficeTypeName { set; get; }
        public int BKAuditComplianceId { set; get; }
        public string BKAuditComplianceDes { set; get; }
        public int BKCheckListTypeId { set; get; }
        public string BKCheckListTypeDes { set; get; }
        public int BKCheckListSubTypeId { set; get; }
        public string BKCheckListSubTypeDes { set; get; }
        public int BKCheckListItemId { set; get; }
        public string BKCheckListItemDes { set; get; }
        public bool FieldType { set; get; }
        public List<CheckListItem> CheckListItemList { set; get; }
        public Audit Audit;
        public MappingData()
        {
            Audit = new Audit();
            CheckListItemList = new List<CheckListItem>();
        }

    }
}
