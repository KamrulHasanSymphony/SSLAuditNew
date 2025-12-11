using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class ReportModel
    {
        public string Id { set; get; }
        public string FromDate { set; get; }
        public string ToDate { set; get; }
        public string BranchId { set; get; }
        public bool Compliance { set; get; }
        public bool Financial { set; get; }
        public bool Operational { set; get; }
        public bool InvestigationOrForensis { set; get; }
        public bool StratigicMeeting { set; get; }
        public bool ManagementReviewMeeting { set; get; }
        public bool OtherMeeting { set; get; }
        public bool Training { set; get; }
    }
}
