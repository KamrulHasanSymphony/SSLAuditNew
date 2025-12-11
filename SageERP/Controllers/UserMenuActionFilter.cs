using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shampan.Core.Interfaces.Services.Audit;
using Shampan.Core.Interfaces.Services.AuditIssues;
using Shampan.Core.Interfaces.Services.Deshboard;
using Shampan.Core.Interfaces.Services.Notification;
using Shampan.Core.Interfaces.Services.UserRoll;
using Shampan.Core.Interfaces.Services.UsersPermission;
using Shampan.Models;
using System.Data.SqlClient;

namespace SSLAudit.Controllers
{

    [ServiceFilter(typeof(UserMenuActionFilter))]
    [Authorize]
    public class UserMenuActionFilter : IActionFilter
    {
        private readonly IUserRollsService _userRollsService;
        private readonly IAuditMasterService _auditMasterService;
        private readonly IDeshboardService _deshboardService;
        private readonly IUsersPermissionService _usersPermissionService;
        private readonly IConfiguration _configuration;
        private readonly INotificationService _notificationService;

        public UserMenuActionFilter(IUserRollsService userRollsService, IAuditMasterService auditMasterService,
           IConfiguration configuration,
           IDeshboardService deshboardService,
           IUsersPermissionService usersPermissionService,
           INotificationService notificationService
         )
        {

            _userRollsService = userRollsService;
            _auditMasterService = auditMasterService;
            _deshboardService = deshboardService;
            _usersPermissionService = usersPermissionService;
            _configuration = configuration;
            _notificationService = notificationService;
            AuthDbConfig.AuthDB = AuthDbName();

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {

                if(YearChange.BeginingYear == null)
                {
                    YearChange.BeginingYear =  "1999-01-01";
                    YearChange.EndYear =  "1999-12-30";
                }
                string userName = context.HttpContext.User.Identity.Name;
                string controllerName = context.RouteData.Values["controller"].ToString();
                string actionName = context.RouteData.Values["action"].ToString();

                ResultModel<List<UserBranch?>> rolls = _auditMasterService.GetUserIdbyUserName(userName);
                UserBranch ur = rolls.Data.FirstOrDefault();
                List<UserManuInfo> userMenu = _userRollsService.GetUserManu(userName);
                List<SubmanuList> usereSubManu = _userRollsService.GetUserSubManu(userName);
                context.HttpContext.Items["UserMenu"] = userMenu;
                context.HttpContext.Items["usereSubManu"] = usereSubManu;
                context.HttpContext.Items["AuditComponentList"] = _userRollsService.GetAuditComponents();
                context.HttpContext.Items["GetUnPlanAuditComponents"] = _deshboardService.GetUnPlanAuditComponents();
                context.HttpContext.Items["branchCoverages"] = _userRollsService.GetBranchCoverages();
                context.HttpContext.Items["basicData"] = _userRollsService.GetBasicData();
                context.HttpContext.Items["SpecialEngagements"] = _userRollsService.GetSpecialEngagements();
                context.HttpContext.Items["AddHocEngagements"] = _userRollsService.GetAddHocEngagements();
                
                context.HttpContext.Items["AuditBranchUserGetAll"] = _deshboardService.AuditBranchUserGetAll(userName);
              
                context.HttpContext.Items["GetBranchName"] = _deshboardService.GetBranchName(ur.UserId);
                context.HttpContext.Items["NotificationsCount"] = _deshboardService.NotificationsCount(new IndexModel() { UserId = ur.UserId });
                context.HttpContext.Items["Notifications"] = _notificationService.GetIndexData(new IndexModel() { UserId = ur.UserId },new[] { "IsAudit", "IsIssue", "IsFeedback", "IsBranchFeedBack", "IsAuditApprove" }, new[] { 1.ToString(),1.ToString(),1.ToString(),1.ToString(),1.ToString() });
          
            }

        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
        public string AuthDbName()
        {
            string connectionString = _configuration.GetConnectionString("AuthContext");
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            string databaseName = builder.InitialCatalog;
            return databaseName;
        }


    }

    [ServiceFilter(typeof(DeshboardActionFilter))]
    [Authorize]
    public class DeshboardActionFilter : IActionFilter
    {

        private readonly IUserRollsService _userRollsService;
        private readonly IAuditMasterService _auditMasterService;
        private readonly IDeshboardService _deshboardService;
        private readonly IUsersPermissionService _usersPermissionService;
        private readonly IConfiguration _configuration;
        private readonly INotificationService _notificationService;
        private readonly IAuditIssueService _auditIssueService;


        public DeshboardActionFilter(IUserRollsService userRollsService, IAuditMasterService auditMasterService,
           IConfiguration configuration,
           IDeshboardService deshboardService,
           IUsersPermissionService usersPermissionService,
           INotificationService notificationService,
           IAuditIssueService auditIssueService
         )
        {
            _userRollsService = userRollsService;
            _auditMasterService = auditMasterService;
            _deshboardService = deshboardService;
            _usersPermissionService = usersPermissionService;
            _configuration = configuration;
            _notificationService = notificationService;
            _auditIssueService = auditIssueService;

        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                string userName = context.HttpContext.User.Identity.Name;
                string controllerName = context.RouteData.Values["controller"].ToString();
                string actionName = context.RouteData.Values["action"].ToString();


                ResultModel<List<UserBranch?>> rolls = _auditMasterService.GetUserIdbyUserName(userName);
                UserBranch ur = rolls.Data.FirstOrDefault();


                context.HttpContext.Items["OngoinAudit"] = _auditMasterService.GetIndexDataCount(new IndexModel() { Status ="Audit"}, new[] { "ad.AuditStatus" } , new[] { "Ongoing" } , null).Data;

                context.HttpContext.Items["IssueProrityHigh"] = _auditIssueService.GetIndexDataCount(null,new[] { "A_AuditIssues.IssuePriority" }, new[] { "1029" }).Data;
                context.HttpContext.Items["IssueProrityMedium"] = _auditIssueService.GetIndexDataCount(null,new[] { "A_AuditIssues.IssuePriority" }, new[] { "1030" }).Data;
                context.HttpContext.Items["IssueProrityLow"] = _auditIssueService.GetIndexDataCount(null,new[] { "A_AuditIssues.IssuePriority" }, new[] { "1031" }).Data;
                context.HttpContext.Items["IssueApprove"] = _auditIssueService.GetIndexDataCount(null,new[] { "A_AuditIssues.IsIssueApprove" }, new[] { "1" }).Data;
                context.HttpContext.Items["IssueCreatedOnAudit"] = _auditMasterService.GetIndexDataCount(new IndexModel() { Status = "Audit" }, null, null).Data;

                context.HttpContext.Items["Issues"] = _userRollsService.GetIssues(userName);
                context.HttpContext.Items["TotalAudit"] = _deshboardService.TotalAudit(userName);
                context.HttpContext.Items["TotalCompletedOngoingRemaing"] = _deshboardService.TotalCompletedOngoingRemaing(userName);
                context.HttpContext.Items["UnPlan"] = _deshboardService.UnPlan(userName);
                context.HttpContext.Items["TotalAuditApproved"] = _deshboardService.TotalAuditApproved(userName);
                context.HttpContext.Items["TotalAuditRejected"] = _deshboardService.TotalAuditRejected(userName);
                context.HttpContext.Items["TotalIssueRejected"] = _deshboardService.TotalIssueRejected(userName);
                context.HttpContext.Items["DeadLineForResponse"] = _deshboardService.DeadLineForResponse(userName);
                context.HttpContext.Items["TotalRisk"] = _deshboardService.TotalRisk(userName);
                context.HttpContext.Items["BeforeDeadLineIssue"] = _deshboardService.BeforeDeadLineIssue(userName);
                context.HttpContext.Items["MissDeadLineIssues"] = _deshboardService.MissDeadLineIssues(userName);
                context.HttpContext.Items["TotalFollowUpAudit"] = _deshboardService.TotalFollowUpAudit(userName);
                context.HttpContext.Items["TotalPendingIssueReview"] = _deshboardService.TotalPendingIssueReview(userName);
                context.HttpContext.Items["PendingForAuditFeedback"] = _deshboardService.PendingForAuditFeedback(userName);
                context.HttpContext.Items["FollowUpAuditIssues"] = _deshboardService.FollowUpAuditIssues(userName);
                context.HttpContext.Items["PendingForApproval"] = _deshboardService.PendingForApproval(userName);
                context.HttpContext.Items["PendingAuditResponse"] = _deshboardService.PendingAuditResponse(userName);
                context.HttpContext.Items["PendingAuditApproval"] = _deshboardService.PendingAuditApproval(userName);
                context.HttpContext.Items["FinalAuidtApproved"] = _deshboardService.FinalAuidtApproved(userName);
                context.HttpContext.Items["GetPrePayment"] = _deshboardService.GetPrePayment();
                context.HttpContext.Items["PendingForReviewerFeedback"] = _deshboardService.PendingForReviewerFeedback(userName);             

                context.HttpContext.Items["PrepaymentReview"] = _deshboardService.PrepaymentReview();
                context.HttpContext.Items["ProfileImage"] = _usersPermissionService.GetImageByUserName(new[] { "AUA.UserName" }, new[] { userName }).Data.FirstOrDefault();
                context.HttpContext.Items["PendingForIssueApproval"] = _deshboardService.PendingForIssueApproval(userName);

                context.HttpContext.Items["PendingForReviewerFeedbackForTeam"] = _deshboardService.PendingForReviewerFeedbackForTeam(userName);
                ResultModel<List<UserBranch>> Users = _auditMasterService.GetUserIdbyUserName(userName);
                context.HttpContext.Items["AuditTeamMemberGetAll"] = _deshboardService.AuditTeamMemberGetAll(userName, Users.Data.FirstOrDefault().UserId);
                context.HttpContext.Items["GetTotalBranchWithAuditCount"] = _deshboardService.GetTotalBranchWithAuditCount(null).Data;

            }

        }
        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}


