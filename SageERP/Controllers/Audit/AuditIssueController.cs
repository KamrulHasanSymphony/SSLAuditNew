using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SageERP.ExtensionMethods;
using Shampan.Core.Interfaces.Services.Audit;
using Shampan.Core.Interfaces.Services.AuditIssues;
using Shampan.Core.Interfaces.Services.Mail;
using Shampan.Core.Interfaces.Services.Notification;
using Shampan.Core.Interfaces.Services.TeamMember;
using Shampan.Models;
using Shampan.Models.AuditModule;
using Shampan.Services;
using Shampan.Services.Audit;
using ShampanERP.Models;
using ShampanERP.Persistence;
using SSLAudit.Models;
using StackExchange.Exceptional;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Text;

namespace SSLAudit.Controllers.Audit
{
    [ServiceFilter(typeof(UserMenuActionFilter))]

    public class AuditIssueController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IAuditIssueService _auditIssueService;
        private readonly IAuditMasterService _auditMasterService;
        private readonly IAuditIssueAttachmentsService _auditIssueAttachmentsService;
        private readonly IAuditIssueUserService _auditIssueUserService;
        private readonly IConfiguration _configuration;
        private readonly INotificationService _notificationService;
        private readonly IMailService _mailService;
        private readonly ITeamMembersService _teamMembersService;



        public AuditIssueController(ApplicationDbContext applicationDb,
            IAuditIssueUserService auditIssueUserService,
            IAuditIssueService auditIssueService,
            IAuditIssueAttachmentsService auditIssueAttachmentsService,
            IAuditMasterService auditMasterService,
            IConfiguration configuration,
            INotificationService notificationService,
            IMailService mailService,
            ITeamMembersService teamMembersService
            )
        {
            _applicationDb = applicationDb;
            _auditIssueService = auditIssueService;
            _auditIssueAttachmentsService = auditIssueAttachmentsService;
            _auditMasterService = auditMasterService;
            _auditIssueUserService = auditIssueUserService;
            _configuration = configuration;
            _notificationService = notificationService;
            _mailService = mailService;
            _teamMembersService = teamMembersService;
        }

        public IActionResult Index(int? id)
        {
            if (id is null || id == 0)
            {
                return RedirectToAction("Index", "Audit");
            }

            AuditIssue auditIssue = new AuditIssue()
            {
                AuditId = id.Value
            };

            var auditMaster = _auditMasterService.GetAll(new[] { "Id" }, new[] { id.Value.ToString() }).Data;

            if (auditMaster != null && auditMaster.Count > 0)
            {
                auditIssue.AuditMaster = auditMaster.FirstOrDefault();
            }

            return View(auditIssue);

        }

        public IActionResult Create(int? id)
        {

            if (id is null || id == 0)
            {
                return RedirectToAction("Index", "Audit");
            }

            AuditIssue auditIssue = new AuditIssue()
            {
                Operation = "add",
                AuditId = id.Value
            };

            return View(auditIssue);

        }

        [HttpPost]
        public ActionResult CreateEdit(AuditIssue master)
        {

            ResultModel<AuditIssue> result = new ResultModel<AuditIssue>();
            PeramModel pm = new PeramModel();
            ResultModel<List<AuditUser>> Users = new ResultModel<List<AuditUser>>();
            Notifications notifiaction = new Notifications();


            try
            {
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                ResultModel<List<AuditMaster>> teamResult = _auditMasterService.GetAll(new[] { "Id" }, new[] { master.AuditId.ToString() });
                ResultModel<List<TeamMembers>> teamMembers = _teamMembersService.GetAll(new[] { "U.TeamId" }, new[] { teamResult.Data.FirstOrDefault().TeamId.ToString() });


             

                if (master.Operation == "update")
                {

                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = "";

                    result = _auditIssueService.Update(master);

                    return Ok(result);
                }
                else
                {


                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = "";
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = "";

                    result = _auditIssueService.Insert(master);


                    #region Notification

                    var currentUrl = HttpContext.Request.GetDisplayUrl();
                    string[] parts = new string[] { "", "" };
                    parts = currentUrl.TrimStart('/').Split('/');
                    string urlhttp = parts[0];
                    string HostUrl = parts[2];
                    string Url = urlhttp + "//" + HostUrl + "/Audit/Edit/" + result.Data.AuditId + "?edit=issue&common=issueNotification " + result.Data.Id;             
                    //notifiaction = SetNotificationValueForInsert(Url, master.IssueName, result.Data.AuditId, result.Data.Id, user.UserName, "Issue");
                    //_notificationService.Insert(notifiaction);
                    ResultModel<Notifications> resultNotification = _notificationService.InsertNotification(Url, master.IssueName, result.Data.AuditId, result.Data.Id, user.UserName, "Issue", teamMembers.Data);

                    #endregion


                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);
        }
       

        [HttpPost]
        public ActionResult<IList<AuditIssue>> _index(int? id)
        {
            try
            {
                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                string? search = Request.Form["search[value]"].FirstOrDefault();
                string issuename = Request.Form["issuename"].ToString();
                string issuepriority = Request.Form["issuepriority"].ToString();
                string dateofsubmission = Request.Form["dateofsubmission"].ToString();
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
                index.AuditId = id.Value;

                string[] conditionalFields = new[]
                {
                    "IssueName like",
                    "Enums.EnumValue like",
                    "DateOfSubmission like"
                };
                string?[] conditionalValue = new[] { issuename, issuepriority, dateofsubmission };


                ResultModel<List<AuditIssue>> indexData =
                    _auditIssueService.GetIndexData(index,
                        conditionalFields, conditionalValue);


                ResultModel<int> indexDataCount =
                    _auditIssueService.GetIndexDataCount(index,
                        new[] { "A_AuditIssues.AuditId" }, new[] { index.AuditId.ToString() });

                int result = _auditIssueService.GetCount(TableName.A_AuditIssues, "Id", new[]
                        {
                            "createdBy",
                        }, new[] { userName });


                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });

            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<AuditIssue>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }
        }


        public ActionResult<IList<AuditIssue>> Edit(int id)
        {
            try
            {
                ResultModel<List<AuditIssue?>> result =
                    _auditIssueService.GetAll(new[] { "Id" }, new[] { id.ToString() });

                if (result.Status == Status.Fail)
                {
                    throw result.Exception;
                }

                AuditIssue? auditMaster = result.Data.FirstOrDefault();
                auditMaster.AttachmentsList = _auditIssueAttachmentsService.GetAll(new[] { "AuditIssueId" }, new[] { id.ToString() }).Data;
                auditMaster.Operation = "update";

                return View("Create", auditMaster);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> AuditIssueIndex(string edit)
        {
            return View();
        }

        [HttpPost]
        public ActionResult<IList<AuditIssue>> _auditIssueIndex(string edit)
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
                string issuepriority = Request.Form["issuepriority"].ToString();
                string dateofsubmission = Request.Form["dateofsubmission"].ToString();
                string operational = Request.Form["operational"].ToString();
                string financial = Request.Form["Financial"].ToString();
                string compliance = Request.Form["compliance"].ToString();

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
                index.Status = edit;
                index.UserName = userName;

                string[] conFields = new string[10];
                string[] conditionalFields = new[]
                {
                    "A.Code like",
                    "A.Name like",
                    "A.AuditStatus like",

                    "AI.IssueName like",
                    "Enums.EnumValue like",
                    "AI.DateOfSubmission like",
                    "AI.Operational like",
                    "AI.Financial like",
                    "AI.Compliance like"
                };

                string?[] conditionalValue = new[] { auditCode, auditName, auditStatus, issuename, issuepriority, dateofsubmission, operational, financial, compliance };

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

                ResultModel<List<AuditIssue>> indexData = _auditIssueService.GetAuditIssueIndexData(index, conditionalFields, conditionalValue);

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

                ResultModel<int> indexDataCount = _auditIssueService.GetAuditIssueIndexCount(index, conditionalFields, conditionalValue);

                int result = _auditIssueService.GetCount(TableName.A_AuditIssues, "Id", new[] { "createdBy", }, new[] { userName });

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });

            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<AuditIssue>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }
        }

        public async Task<IActionResult> ExcelIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult<IList<AuditIssue>> _excelIndex()
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

                ResultModel<List<AuditIssue>> indexData = _auditIssueService.GetExcelIndexData(index, conditionalFields, conditionalValue);

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

                ResultModel<int> indexDataCount =
                    _auditIssueService.GetExcelIndexCount(index, conditionalFields, conditionalValue);

                int result = _auditIssueService.GetCount(TableName.A_AuditIssues, "Id", new[]
                        {
                            "createdBy",
                        }, new[] { userName });

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });

            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<AuditIssue>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }
        }

        [HttpPost]
        public ActionResult FollowUpAuditIssueEamil(AuditIssueUser master)
        {
            ResultModel<AuditIssueUser> result = new ResultModel<AuditIssueUser>();
            AuditMail auditMail = new AuditMail();

            try
            {
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                UserProfile urp = new UserProfile();
                urp.Id = master.AuditId;
                urp.UserName = user.UserName;
                MailSetting mailSetting = new MailSetting();
                mailSetting.Id = master.AuditId;
                var notifiEmai = _auditMasterService.GetEamil(urp);
                var email = notifiEmai.Data.FirstOrDefault();
                var currentUrl = HttpContext.Request.GetDisplayUrl();
                string[] parts = new string[] { "", "" };
                parts = currentUrl.TrimStart('/').Split('/');
                string urlhttp = parts[0];
                string HostUrl = parts[2];
                string AuditPreviewUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + master.AuditId + "?edit=Branchfeedback";
                ResultModel<List<AuditMaster?>> AuditMaster = _auditMasterService.GetAll(new[] { "Id" }, new[] { master.AuditId.ToString() });
                AuditMaster? auditMaster = AuditMaster.Data.FirstOrDefault();

                List<AuditIssueUser> userlist = _auditIssueUserService.GetAuditIssueUserById(new[] { "AuditIssueId" }, new[] { master.AuditIssueId.ToString() }).Data;

                foreach (var item in userlist)
                {
                    auditMail.To = master.EmailAddress;
                    auditMail.URL = AuditPreviewUrl;
                    auditMail.Name = auditMaster.Name;
                    auditMail.Status = "FollowUpAuditIssue";
                    var model = _mailService.SendEmail(auditMail);
                }
                return Ok(master);

            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }
            return Ok(result);
        }

        [HttpPost]
        public ActionResult IssuedeadLineLapsed(AuditIssueUser master)
        {
            ResultModel<AuditIssueUser> result = new ResultModel<AuditIssueUser>();
            ResultModel<List<AuditMaster?>> AuditMaster = new ResultModel<List<AuditMaster?>>();
            List<AuditIssueUser> userlist = new List<AuditIssueUser>();
            AuditMail emailsent = new AuditMail();
            ResultModel<AuditMail> model = new ResultModel<AuditMail>();

            try
            {
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                UserProfile urp = new UserProfile();
                urp.Id = master.AuditId;
                urp.UserName = user.UserName;
                MailSetting mailSetting = new MailSetting();
                mailSetting.Id = master.AuditId;

                var notifiEmai = _auditMasterService.GetEamil(urp);
                var email = notifiEmai.Data.FirstOrDefault();

                var currentUrl = HttpContext.Request.GetDisplayUrl();
                string[] parts = new string[] { "", "" };
                parts = currentUrl.TrimStart('/').Split('/');
                string urlhttp = parts[0];
                string HostUrl = parts[2];
                string AuditPreviewUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + master.AuditId + "?edit=Branchfeedback";

                AuditMaster = _auditMasterService.GetAll(new[] { "Id" }, new[] { master.AuditId.ToString() });
                AuditMaster? audit = AuditMaster.Data.FirstOrDefault();
                userlist = _auditIssueUserService.GetAuditIssueUserById(new[] { "AuditIssueId" }, new[] { master.AuditIssueId.ToString() }).Data;

                foreach (var item in userlist)
                {
                    emailsent.To = item.EmailAddress;
                    emailsent.URL = AuditPreviewUrl;
                    emailsent.Name = audit.Name;
                    emailsent.Status = "IssuedeadLineLapsed";
                    model = _mailService.SendEmail(emailsent);
                    //MailService.IssuedeadLineLapsedEamil(item.EmailAddress, AuditPreviewUrl, audit.Name);
                    //master.Status = "200";
                }

                return Ok(model);
                //return Ok(master);

            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);
        }
        public ActionResult TotalPendingIssuesReview(AuditIssueUser master)
        {
            ResultModel<AuditIssueUser> result = new ResultModel<AuditIssueUser>();
            AuditMail auditMail = new AuditMail();
            ResultModel<AuditMail> mail = new ResultModel<AuditMail>();
            ResultModel<List<AuditMaster?>> AuditMaster = new ResultModel<List<AuditMaster?>>();
            AuditMaster? audit = new AuditMaster();
            List<AuditIssueUser> userlist = new List<AuditIssueUser>();

            try
            {
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                UserProfile urp = new UserProfile();
                urp.Id = master.AuditId;
                urp.UserName = user.UserName;
                MailSetting mailSetting = new MailSetting();
                mailSetting.Id = master.AuditId;

                var notifiEmai = _auditMasterService.GetEamil(urp);
                var email = notifiEmai.Data.FirstOrDefault();

                var currentUrl = HttpContext.Request.GetDisplayUrl();
                string[] parts = new string[] { "", "" };
                parts = currentUrl.TrimStart('/').Split('/');
                string urlhttp = parts[0];
                string HostUrl = parts[2];
                string AuditPreviewUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + master.AuditId + "?edit=Branchfeedback";
                AuditMaster = _auditMasterService.GetAll(new[] { "Id" }, new[] { master.AuditId.ToString() });
                audit = AuditMaster.Data.FirstOrDefault();

                userlist = _auditIssueUserService.GetAuditIssueUserById(new[] { "AuditIssueId" }, new[] { master.AuditIssueId.ToString() }).Data;

                foreach (var item in userlist)
                {
                    auditMail.To = item.EmailAddress;
                    auditMail.URL = AuditPreviewUrl;
                    auditMail.Name = audit.Name;
                    auditMail.Status = "TotalPendingIssuesReview";
                    mail = _mailService.SendEmail(auditMail);                   
                }

                return Ok(mail);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }
            return Ok(result);
        }

        public ActionResult<IList<AuditIssueUser>> Delete(AuditIssueUser master)
        {
            try
            {
                ResultModel<AuditIssueUser> result = _auditIssueUserService.Delete(master.Id);
                return Ok(result);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }

        public ActionResult<IList<AuditReportUsers>> ReportDelete(AuditReportUsers master)
        {
            try
            {
                ResultModel<AuditReportUsers> result = _auditIssueUserService.ReportDelete(master.Id);
                return Ok(result);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }
        public ActionResult MultiplePost(AuditIssue master)
        {

            ResultModel<AuditIssue> result = new ResultModel<AuditIssue>();

            try
            {
                foreach (string ID in master.IDs)
                {
                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.PostedBy = user.UserName;
                    master.Audit.PostedOn = DateTime.Now;
                    master.Audit.PostedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    master.Operation = "post";
                    result = _auditIssueService.MultiplePost(master);

                }

                return Ok(result);

            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok("");
        }

        public ActionResult MultipleUnPost(AuditIssue master)
        {
            ResultModel<AuditIssue> result = new ResultModel<AuditIssue>();

            try
            {
                foreach (string ID in master.IDs)
                {
                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.PostedBy = user.UserName;
                    master.Audit.PostedOn = DateTime.Now;
                    master.Audit.PostedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    master.Operation = "unpost";
                    result = _auditIssueService.MultipleUnPost(master);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok("");
        }

        [HttpPost]
        public IActionResult DeleteFile(string filePath, string id)
        {
            string saveDirectory = "wwwroot\\files";

            ResultModel<AuditIssueAttachments> result = new ResultModel<AuditIssueAttachments>
            {
                Message = "File could not be deleted"
            };

            try
            {
                var path = Path.Combine(saveDirectory, filePath);
                if (!System.IO.File.Exists(path)) return Ok(result);

                result = _auditIssueAttachmentsService.Delete(Convert.ToInt32(id.Replace("file-", "")));

                if (result.Status == Status.Success)
                {
                    System.IO.File.Delete(path);
                }


                return Ok(result);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);

                return RedirectToAction("Index");
            }
        }


        [HttpGet]
        public async Task<IActionResult> DownloadFile(string filePath)
        {
            string saveDirectory = "wwwroot\\files";

            try
            {
                var path = Path.Combine(saveDirectory, filePath);
                var memory = new MemoryStream();
                await using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                var ext = Path.GetExtension(path).ToLowerInvariant();
                return File(memory, GetMimeType(ext), Path.GetFileName(path));
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);

                return RedirectToAction("Index");
            }
        }

        public ActionResult DeActive(AuditIssue master)
        {
            ResultModel<AuditIssue> result = new ResultModel<AuditIssue>();
            try
            {
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                master.Audit.LastUpdateBy = user.UserName;
                master.Audit.LastUpdateOn = DateTime.Now;
                master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                result = _auditIssueService.DeActiveIssue(master);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }
            return Ok(result);
        }
        public ActionResult Active(AuditIssue master)
        {
            ResultModel<AuditIssue> result = new ResultModel<AuditIssue>();
            try
            {
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                master.Audit.LastUpdateBy = user.UserName;
                master.Audit.LastUpdateOn = DateTime.Now;
                master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                result = _auditIssueService.ActiveIssue(master);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }
            return Ok(result);
        }


        public ActionResult IssueExcel(string fromDate, string toDate, string Id = "",bool Compliance=false,
            bool Financial=false,bool Operational=false, bool InvestigationOrForensis=false,bool StratigicMeeting=false,
            bool ManagementReviewMeeting = false,bool OtherMeeting = false,bool Training = false)
        {
            try
            {
                AuditMaster vms = new AuditMaster();

                string Branchid;
                string BranchName = "";
                string CompanyName = "";
                string CompanyAddress = "";


                ReportModel vm = new ReportModel();
                vm.Id = Id;
                vm.ToDate = toDate;
                vm.FromDate = fromDate;
                vm.Compliance = Compliance;
                vm.Financial = Financial;
                vm.Operational = Operational;
                vm.InvestigationOrForensis = InvestigationOrForensis;
                vm.StratigicMeeting = StratigicMeeting;
                vm.ManagementReviewMeeting = ManagementReviewMeeting;
                vm.OtherMeeting = OtherMeeting;
                vm.Training = Training;

                if (vm.BranchId == "-1")
                {
                    BranchName = "ALL";
                    CompanyName = "-";
                    CompanyAddress = "-";
                }
                else
                {
                    BranchProfile branchProfile = new BranchProfile();
                    CompanyName = "Green Delta Insurance Company Limited";
                    CompanyAddress = "Green Delta AIMS Tower (6th Floor), 51-52, Mohakhali C/A, Bir Uttam AK Khandakar Road, 1212 Dhaka, Dhaka Division, Gulshan, Dhaka ";

                    if (branchProfile != null)
                    {
                        BranchName = branchProfile.BranchName;

                    }
                }

                ResultModel<System.Data.DataTable> dt = new ResultModel<System.Data.DataTable>();


                dt = _auditIssueService.DetailsInformation(vm);


                if (!dt.Data.Columns.Contains("SerialNumber"))
                {
                    dt.Data.Columns.Add("SerialNumber", typeof(int)).SetOrdinal(0);
                }
                for (int i = 0; i < dt.Data.Rows.Count; i++)
                {
                    dt.Data.Rows[i]["SerialNumber"] = i + 1;
                }

                // Define columns to update
                string[] columnsToUpdate = new string[]
                {
                    "InvestigationOrForensis",
                    "StratigicMeeting",
                    "ManagementReviewMeeting",
                    "OtherMeeting",
                    "Training",
                    "Operational",
                    "Financial",
                    "Compliance"
                };

                foreach (var column in columnsToUpdate)
                {
                    dt.Data.Columns.Add(column + "_Symbol", typeof(string));
                }

                foreach (DataRow row in dt.Data.Rows)
                {
                    foreach (var column in columnsToUpdate)
                    {
                        bool value = row.Field<bool>(column);
                        row[column + "_Symbol"] = value ? "✔" : "✖";
                    }
                }
                foreach (var column in columnsToUpdate)
                {
                    dt.Data.Columns.Remove(column);
                }
                foreach (var column in columnsToUpdate)
                {
                    dt.Data.Columns[column + "_Symbol"].ColumnName = column;
                }


                //dt.Data.Columns["Id"].ColumnName = "ID No.";
                dt.Data.Columns["SerialNumber"].ColumnName = "SL No.";
                dt.Data.Columns["Code"].ColumnName = "Code";
                dt.Data.Columns["Name"].ColumnName = "Audit Name";
                dt.Data.Columns["AuditStatus"].ColumnName = "Audit Status";
                dt.Data.Columns["IssuePriority"].ColumnName = "Issue Priority";
                dt.Data.Columns["IssueStatus"].ColumnName = "Issue Status";
                dt.Data.Columns["IssueName"].ColumnName = "Issue Name";
                dt.Data.Columns["IsPost"].ColumnName = "IsPost";
                dt.Data.Columns["Risk"].ColumnName = "Risk";
                dt.Data.Columns["IssueOpenDate"].ColumnName = "Issue Open Date";
                dt.Data.Columns["DateOfSubmission"].ColumnName = "Date Of Submission";
                dt.Data.Columns["IssueDeadLine"].ColumnName = "Issue DeadLine";
                dt.Data.Columns["ImplementationDate"].ColumnName = "Implementation Date";
                dt.Data.Columns["InvestigationOrForensis"].ColumnName = "Investigation Or Forensis";
                dt.Data.Columns["StratigicMeeting"].ColumnName = "Stratigic Meeting";
                dt.Data.Columns["ManagementReviewMeeting"].ColumnName = "Management Review Meeting";
                dt.Data.Columns["OtherMeeting"].ColumnName = "Other Meeting";
                dt.Data.Columns["Training"].ColumnName = "Training";
                dt.Data.Columns["Operational"].ColumnName = "Operational";
                dt.Data.Columns["Financial"].ColumnName = "Financial";
                dt.Data.Columns["Compliance"].ColumnName = "Compliance";

                #region Excel

                string filename = "High Level Report" + "-" + DateTime.Now.ToString("yyyy-MM-dd");
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add("High Level Report");

                ExcelSheetFormat(dt.Data, workSheet, vm.FromDate, vm.ToDate, BranchName, CompanyAddress, CompanyName);

                #region Excel Download

                using (var memoryStream = new MemoryStream())
                {
                    excel.SaveAs(memoryStream);
                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename + ".xlsx");
                }
                #endregion

                return Redirect("Index");

                #endregion
            }

            catch (Exception e)
            {
                throw;
            }


        }
        private void ExcelSheetFormat(System.Data.DataTable dt, ExcelWorksheet workSheet, string fromDate, string toDate, string BranchName, string CompanyAddress, string CompanyName)
        {

            string companyName = "Company Name: " + CompanyName;
            string companyAddress = "Company Address: " + CompanyAddress;
            string reportHeader = "High Level Report";
            string[] ReportHeaders = new string[] { companyName, companyAddress, reportHeader };
            int TableHeadRow = ReportHeaders.Length + 4;
            int RowCount = dt.Rows.Count;
            int ColumnCount = dt.Columns.Count;
            int GrandTotalRow = TableHeadRow + RowCount + 1;


            workSheet.Cells[TableHeadRow, 1].LoadFromDataTable(dt, true);


            var format = new OfficeOpenXml.ExcelTextFormat();
            format.Delimiter = '~';
            format.TextQualifier = '"';
            format.DataTypes = new[] { eDataTypes.String };

            for (int i = 0; i < ReportHeaders.Length; i++)
            {
                workSheet.Cells[i + 1, 1, i + 1, ColumnCount].Merge = true;
                workSheet.Cells[i + 1, 1, i + 1, ColumnCount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[i + 1, 1, i + 1, ColumnCount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[i + 1, 1, i + 1, ColumnCount].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);


                workSheet.Cells[i + 1, 1, i + 1, ColumnCount].Style.Font.Color.SetColor(System.Drawing.Color.Blue);


                switch (i)
                {
                    case 0:
                        workSheet.Cells[i + 1, 1].Style.Font.Size = 18;
                        break;
                    case 1:
                    case 2:
                        workSheet.Cells[i + 1, 1].Style.Font.Size = 14;
                        break;
                    case 3:
                        workSheet.Cells[i + 1, 1].Style.Font.Size = 16;
                        break;
                }

                workSheet.Cells[i + 1, 1].LoadFromText(ReportHeaders[i], format);
            }

            // Load the DataTable after setting headers
            workSheet.Cells[TableHeadRow, 1].LoadFromDataTable(dt, true);

            workSheet.Cells[ReportHeaders.Length + 2, 2].Value = "From Date:";
            workSheet.Cells[ReportHeaders.Length + 2, 3].Value = fromDate;
            workSheet.Cells[ReportHeaders.Length + 2, 4].Value = "To Date:";
            workSheet.Cells[ReportHeaders.Length + 2, 5].Value = toDate;

            // Change text color of date cells
            workSheet.Cells[ReportHeaders.Length + 2, 2, ReportHeaders.Length + 2, 5].Style.Font.Color.SetColor(System.Drawing.Color.Red);


            int colNumber = 0;

            foreach (DataColumn col in dt.Columns)
            {
                colNumber++;
                if (col.DataType == typeof(DateTime))
                {
                    workSheet.Column(colNumber).Style.Numberformat.Format = "dd-MMM-yyyy";
                }
                else if (col.DataType == typeof(Decimal))
                {
                    workSheet.Column(colNumber).Style.Numberformat.Format = "#,##0.00_);[Red](#,##0.00)";
                    workSheet.Cells[GrandTotalRow, colNumber].Formula = "=Sum(" + workSheet.Cells[TableHeadRow + 1, colNumber].Address + ":" + workSheet.Cells[(TableHeadRow + RowCount), colNumber].Address + ")";
                }

                workSheet.Column(colNumber).AutoFit();
            }

            workSheet.Cells[TableHeadRow, 1, TableHeadRow, ColumnCount].Style.Font.Bold = true;
            workSheet.Cells[GrandTotalRow, 1, GrandTotalRow, ColumnCount].Style.Font.Bold = true;
            workSheet.Cells["A" + TableHeadRow + ":" + IdentityExtensions.Alphabet[ColumnCount - 1] + (TableHeadRow + RowCount + 2)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells["A" + TableHeadRow + ":" + IdentityExtensions.Alphabet[ColumnCount] + (TableHeadRow + RowCount + 1)].Style.Border.Left.Style = ExcelBorderStyle.Thin;

        }

        private string GetMimeType(string ext)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(ext, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
        public Notifications SetNotificationValueForInsert(string URL, string Name, int auditId, int commonId, string UserName, string notification)
        {
            Notifications notifiaction = new Notifications();

            notifiaction.URL = URL;
            notifiaction.Name = Name;
            notifiaction.AuditId = auditId;
            notifiaction.CommonId = commonId;
            notifiaction.Audit.CreatedBy = UserName;
            notifiaction.Audit.CreatedOn = DateTime.Now;
            notifiaction.NotificationStatus = notification;
            return notifiaction;
        }

    }
}
