using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SageERP.ExtensionMethods;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.Branch;
using Shampan.Core.Interfaces.Services.HighLevelReports;
using Shampan.Core.Interfaces.Services.Settings;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Core.Interfaces.Services.Tour;
using Shampan.Core.Interfaces.Services.UserRoll;
using Shampan.Models;
using Shampan.Models.AuditModule;
using Shampan.Services.Advance;
using Shampan.Services.Tour;
using Shampan.Services.UserRoll;
using ShampanERP.Models;
using ShampanERP.Persistence;
using SixLabors.ImageSharp.ColorSpaces;
using SSLAudit.Models;
using StackExchange.Exceptional;
using System.Globalization;
using System.Security.Claims;
using System.Text;

namespace SSLAudit.Controllers
{

    [ServiceFilter(typeof(UserMenuActionFilter))]
    [Authorize]

    public class HithLevelReportController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IToursService _toursService;
        private readonly IUserRollsService _userRollsService;
        private readonly IMemoryCache _memoryCache;
        private readonly IBranchProfileService _branchProfileService;
        private readonly ISettingsService _settingsService;
        private readonly IHighLevelReportService _highLevelReportService;

        public HithLevelReportController(

            ApplicationDbContext applicationDb,
            ITeamsService teamsService,
            IToursService toursService,
            IMemoryCache memoryCache,
            IUserRollsService userRollsService,
            IBranchProfileService branchProfileService,
            ISettingsService settingsService,
            IHighLevelReportService highLevelReportService

            )
        {

            _applicationDb = applicationDb;
            _toursService = toursService;
            _userRollsService = userRollsService;
            _memoryCache = memoryCache;
            _branchProfileService = branchProfileService;
            _settingsService = settingsService;
            _highLevelReportService = highLevelReportService;
        }


        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult<IList<HighLevelReport>> _index()
        {

            try
            {

                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                string? search = Request.Form["search[value]"].FirstOrDefault();
                string auditCode = Request.Form["auditCode"].ToString();
                string auditName = Request.Form["auditName"].ToString();
                string auditStatus = Request.Form["auditStatus"].ToString();
                string issuename = Request.Form["issuename"].ToString();
                string issueHeading = Request.Form["issueHeading"].ToString();
                string issuepriority = Request.Form["issuepriority"].ToString();
                string issueStatus = Request.Form["issueStatus"].ToString();
                string dateofsubmission = Request.Form["dateofsubmission"].ToString();
                string investigationOrforensis = Request.Form["investigationOrforensis"].ToString();
                string stratigicMeeting = Request.Form["stratigicMeeting"].ToString();
                string managementReviewMeeting = Request.Form["managementReviewMeeting"].ToString();
                string otherMeeting = Request.Form["otherMeeting"].ToString();
                string training = Request.Form["training"].ToString();
                string operational = Request.Form["operational"].ToString();
                string financial = Request.Form["Financial"].ToString();
                string compliance = Request.Form["compliance"].ToString();

                //investigationOrforensis
                if (investigationOrforensis == "")
                {
                    investigationOrforensis = "Select";
                }
                if (investigationOrforensis != "Select")
                {
                    investigationOrforensis = (investigationOrforensis == "True") ? "1" : "0";

                }
                //managementReviewMeeting
                if (managementReviewMeeting == "")
                {
                    managementReviewMeeting = "Select";
                }
                if (managementReviewMeeting != "Select")
                {
                    managementReviewMeeting = (managementReviewMeeting == "True") ? "1" : "0";
                }
                //otherMeeting
                if (otherMeeting == "")
                {
                    otherMeeting = "Select";
                }
                if (otherMeeting != "Select")
                {
                    otherMeeting = (otherMeeting == "True") ? "1" : "0";
                }
                //training
                if (training == "")
                {
                    training = "Select";
                }
                if (training != "Select")
                {
                    training = (training == "True") ? "1" : "0";
                }
                //stratigicMeeting
                if (stratigicMeeting == "")
                {
                    stratigicMeeting = "Select";
                }
                if (stratigicMeeting != "Select")
                {
                    stratigicMeeting = (stratigicMeeting == "True") ? "1" : "0";
                }
                if (operational == "")
                {
                    operational = "Select";
                }
                if (operational != "Select")
                {
                    operational = (operational == "True") ? "1" : "0";

                }
                if (financial == "")
                {
                    financial = "Select";
                }
                if (financial != "Select")
                {
                    financial = (financial == "True") ? "1" : "0";

                }
                if (compliance == "")
                {
                    compliance = "Select";
                }
                if (compliance != "Select")
                {
                    compliance = (compliance == "True") ? "1" : "0";

                }

                string draw = Request.Form["draw"].ToString();
                var startRec = Request.Form["start"].FirstOrDefault();
                var pageSize = Request.Form["length"].FirstOrDefault();
                var orderName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][Name]"].FirstOrDefault();
                var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
                index.SearchValue = Request.Form["search[value]"].FirstOrDefault();
                index.OrderName = "Id";
                index.orderDir = "asc";
                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);
                index.createdBy = userName;
                index.Status = "TotalIssues";
                index.UserName = userName;

                string[] conFields = new string[10];

                string[] conditionalFields = new[]
                {
                    "A.Code like",
                    "A.Name like",
                    "A.AuditStatus like",
                    "AI.IssueName like",
                    "Enums.EnumValue like",
                    "anm.EnumValue like",
                    "AI.DateOfSubmission like",
                    "AI.InvestigationOrForensis like",
                    "AI.StratigicMeeting like",
                    "AI.ManagementReviewMeeting like",
                    "AI.OtherMeeting like",
                    "AI.Training like",
                    "AI.Operational like",
                    "AI.Financial like",
                    "AI.Compliance like"
                };
                string?[] conditionalValue = new[] { auditCode, auditName, auditStatus, issueHeading, issuepriority, issueStatus, dateofsubmission, investigationOrforensis, stratigicMeeting, managementReviewMeeting, otherMeeting, training, operational, financial, compliance };

                //InvestigationOrForensis
                if (investigationOrforensis == "Select")
                {
                    int indexdata = Array.IndexOf(conditionalFields, "AI.InvestigationOrForensis like");

                    if (indexdata != -1)
                    {
                        conditionalFields = conditionalFields.Take(indexdata).Concat(conditionalFields.Skip(indexdata + 1)).ToArray();
                        conditionalValue = conditionalValue.Take(indexdata).Concat(conditionalValue.Skip(indexdata + 1)).ToArray();
                    }
                }
                //StratigicMeeting
                if (stratigicMeeting == "Select")
                {
                    int indexdata = Array.IndexOf(conditionalFields, "AI.StratigicMeeting like");

                    if (indexdata != -1)
                    {
                        conditionalFields = conditionalFields.Take(indexdata).Concat(conditionalFields.Skip(indexdata + 1)).ToArray();
                        conditionalValue = conditionalValue.Take(indexdata).Concat(conditionalValue.Skip(indexdata + 1)).ToArray();
                    }
                }
                //ManagementReviewMeeting
                if (managementReviewMeeting == "Select")
                {
                    int indexdata = Array.IndexOf(conditionalFields, "AI.ManagementReviewMeeting like");

                    if (indexdata != -1)
                    {
                        conditionalFields = conditionalFields.Take(indexdata).Concat(conditionalFields.Skip(indexdata + 1)).ToArray();
                        conditionalValue = conditionalValue.Take(indexdata).Concat(conditionalValue.Skip(indexdata + 1)).ToArray();
                    }
                }

                //OtherMeeting
                if (otherMeeting == "Select")
                {
                    int indexdata = Array.IndexOf(conditionalFields, "AI.OtherMeeting like");

                    if (indexdata != -1)
                    {
                        conditionalFields = conditionalFields.Take(indexdata).Concat(conditionalFields.Skip(indexdata + 1)).ToArray();
                        conditionalValue = conditionalValue.Take(indexdata).Concat(conditionalValue.Skip(indexdata + 1)).ToArray();
                    }
                }
                //Training
                if (training == "Select")
                {
                    int indexdata = Array.IndexOf(conditionalFields, "AI.Training like");

                    if (indexdata != -1)
                    {
                        conditionalFields = conditionalFields.Take(indexdata).Concat(conditionalFields.Skip(indexdata + 1)).ToArray();
                        conditionalValue = conditionalValue.Take(indexdata).Concat(conditionalValue.Skip(indexdata + 1)).ToArray();
                    }
                }
                //Operational
                if (operational == "Select")
                {
                    int indexdata = Array.IndexOf(conditionalFields, "AI.Operational like");

                    if (indexdata != -1)
                    {
                        conditionalFields = conditionalFields.Take(indexdata).Concat(conditionalFields.Skip(indexdata + 1)).ToArray();
                        conditionalValue = conditionalValue.Take(indexdata).Concat(conditionalValue.Skip(indexdata + 1)).ToArray();
                    }
                }
                if (financial == "Select")
                {
                    int indexdata = Array.IndexOf(conditionalFields, "AI.Financial like");

                    if (indexdata != -1)
                    {
                        conditionalFields = conditionalFields.Take(indexdata).Concat(conditionalFields.Skip(indexdata + 1)).ToArray();
                        conditionalValue = conditionalValue.Take(indexdata).Concat(conditionalValue.Skip(indexdata + 1)).ToArray();
                    }
                }
                if (compliance == "Select")
                {
                    int indexdata = Array.IndexOf(conditionalFields, "AI.Compliance like");

                    if (indexdata != -1)
                    {
                        conditionalFields = conditionalFields.Take(indexdata).Concat(conditionalFields.Skip(indexdata + 1)).ToArray();
                        conditionalValue = conditionalValue.Take(indexdata).Concat(conditionalValue.Skip(indexdata + 1)).ToArray();
                    }
                }

                ResultModel<List<HighLevelReport>> indexData = _highLevelReportService.GetIndexData(index, conditionalFields, conditionalValue);

                #region AddintAllDetailsFromEncryptTextToNormalText 
                foreach (var item in indexData.Data)
                {
                    if (item.IssueDetails != null)
                    {
                        item.IssueDetails = Encoding.UTF8.GetString(Convert.FromBase64String(item.IssueDetails));

                    }
                }
                foreach (var item in indexData.Data)
                {
                    if (item.FeedbackDetails != null)
                    {
                        item.FeedbackDetails = Encoding.UTF8.GetString(Convert.FromBase64String(item.FeedbackDetails));

                    }
                }
                foreach (var item in indexData.Data)
                {
                    if (item.BranchFeedBackDetails != null)
                    {
                        item.BranchFeedBackDetails = Encoding.UTF8.GetString(Convert.FromBase64String(item.BranchFeedBackDetails));

                    }
                }
                #endregion

                ResultModel<int> indexDataCount = _highLevelReportService.GetIndexDataCount(index, conditionalFields, conditionalValue);

                int result = _highLevelReportService.GetCount(TableName.A_AuditIssues, "Id", new[]{ "createdBy",}, new[] { userName });

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });

            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<HighLevelReport>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }


        }

    }
}
