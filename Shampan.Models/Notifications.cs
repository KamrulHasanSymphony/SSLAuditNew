using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class Notifications
    {      
        public int Id { set; get; }
        public string Name { set; get; }
        public string AuditName { set; get; }
        public string IssueName { set; get; }
        public string FeedbackHeading { set; get; }
        public string BranchFeedbackHeading { set; get; }
        public bool IsAudit { set; get; }
        public bool IsIssue { set; get; }
        public bool IsFeedback { set; get; }
        public bool IsBranchFeedBack { set; get; }
        public bool IsAuditCreate { set; get; }
        public bool IsAuditUpdate { set; get; }
        public bool IsIssueCreate { set; get; }
        public bool IsIssueUpdate { set; get; }
        public bool IsFeedBackCeate { set; get; }
        public bool IsFeedBackUpdate { set; get; }
        public bool IsBranchFeedBackCreate { set; get; }
        public bool IsBranchFeedBackUpdate { set; get; }
        public int AuditId { set; get; }
        public int CommonId { set; get; }
        public bool IsAuditApprove { set; get; }
        public int IssueId { set; get; }
        public int FeedBackId { set; get; }
        public int BranchFeedbackId { set; get; }
        public int TotalCount { set; get; }
        public bool IsProccess { set; get; }
        public string NotificationStatus { set; get; }
        public string AuditUrl { set; get; }
        public string IssueUrl { set; get; }
        public string FeedbackUrl { set; get; }
        public string BranchFeedbackUrl { set; get; }
        public string AuditApproveUrl { set; get; }
        public string URL { set; get; }
        public string UserId { set; get; }
        public Audit Audit { set; get; }

        public Notifications()
        {
            Audit = new Audit(); ;
        }

    }
}
