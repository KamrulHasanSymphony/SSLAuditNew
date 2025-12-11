using Shampan.Models;
using Shampan.Models.AuditModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Core.Interfaces.Repository.Deshboard
{
    public interface IDeshboardRepository : IBaseRepository<AuditMaster>
    {
        List<AuditReports> TotalCompletedOngoingRemaing(string UserName);
        List<AuditReports> UnPlan(string UserName);
        PrePaymentMaster PrePaymentInsert(PrePaymentMaster objMaster);
        PrePaymentMaster PrePaymentUpdate(PrePaymentMaster objMaster);
        int TotalAdditionalPaymentCount();
        PrePaymentMaster NonFinancialInsert(PrePaymentMaster objMaster);
        DeshboardSettings DeshboardSettingInsert(DeshboardSettings objMaster);
		PrePaymentMaster NonFinancialUpdate(PrePaymentMaster objMaster);
        DeshboardSettings DeshboardSettingUpdate(DeshboardSettings objMaster);
		List<PrePaymentMaster> PrePaymentGetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        List<PrePaymentMaster> PrePaymentGetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        int PrePaymentGetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        List<PrePaymentMaster> PrePaymentGetGetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
		List<PrePaymentMaster> NonFinancialGetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

		List<DeshboardSettings> DeshboardSettingGetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

		List<PrePaymentMaster> NonFinancialGetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
		int NonFinancialGetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);


        List<DeshboardSettings> DeshboardSettingGetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        int DeshboardSettingGetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);


        List<PrePaymentMaster> NonFinancialGetGetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
		string GenerateCode(string CodeGroup, string CodeName, int branchId = 1);
        List<AuditComponent> GetUnPlanAuditComponents();
        int TotalAudit(string UserName);
        int TotalAuditApproved(string UserName);
        int TotalAuditRejected(string UserName);
        int TotalIssueRejected(string UserName);
        int DeadLineForResponse(string UserName);
        int TotalRisk(string UserName);
        int BeforeDeadLineIssue(string UserName);
        int MissDeadLineIssues(string UserName);
        int TotalFollowUpAudit(string UserName);
        int TotalPendingIssueReview(string UserName);
        int PendingForAuditFeedback(string UserName);
        int FollowUpAuditIssues(string UserName);
        int PendingForApproval(string UserName);
        int PendingAuditResponse(string UserName);
        int PendingAuditApproval(string UserName);
        int FinalAuidtApproved(string UserName);
		string CodeGeneration(string codeGroup, string codeName);
		int PendingForReviewerFeedback(string UserName);
		int PendingForReviewerFeedbackForTeam(string UserName);
		int PendingForIssueApproval(string UserName);
        PrePaymentMaster GetPrePayment();
        PrepaymentReview PrepaymentReview();
        UserBranch GetBranchName(string UserName);
        int NotificationsCount(IndexModel model);
        List<AuditIssueUser> AuditBranchUserGetAll();
        List<TeamMembers> AuditTeamMemberGetAll(string UserId);
        PrepaymentReview PrepaymentReviewInsert(PrepaymentReview objMaster);
        List<AuditMaster> GetTotalBranchWithAuditCount(AuditMaster model);
        List<AuditIssue> GetIssueCategoryData(AuditIssue model);

    }
}
