using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using Humanizer;
using MessagePack.Formatters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Office.Interop.Word;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SageERP.ExtensionMethods;
using Serilog;
using Shampan.Core.Interfaces.Services.Audit;
using Shampan.Core.Interfaces.Services.AuditFeedbackService;
using Shampan.Core.Interfaces.Services.AuditIssues;
using Shampan.Core.Interfaces.Services.AuditPoints;
using Shampan.Core.Interfaces.Services.BKAuditTemlateMasters;
using Shampan.Core.Interfaces.Services.Deshboard;
using Shampan.Core.Interfaces.Services.FiscalYear;
using Shampan.Core.Interfaces.Services.Mail;
using Shampan.Core.Interfaces.Services.Notification;
using Shampan.Core.Interfaces.Services.Settings;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Core.Interfaces.Services.TeamMember;
using Shampan.Core.Interfaces.Services.UserRoll;
using Shampan.Models;
using Shampan.Models.AuditModule;
using Shampan.Services;
using Shampan.Services.Audit;
using Shampan.Services.AuditFeedbackService;
using Shampan.Services.AuditIssues;
using Shampan.Services.Mail;
using ShampanERP.Controllers;
using ShampanERP.Models;
using ShampanERP.Persistence;
using SSLAudit.Models;
using StackExchange.Exceptional;
using StackExchange.Exceptional.Internal;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO.Pipelines;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using EmailSettings = Shampan.Models.EmailSettings;

namespace SSLAudit.Controllers.Audit
{
    [ServiceFilter(typeof(UserMenuActionFilter))]
    [Authorize]
    public class AuditController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IAuditMasterService _auditMasterService;
        private readonly IAuditAreasService _auditAreasService;
        private readonly IAuditIssueService _auditIssueService;
        private readonly IAuditIssueAttachmentsService _auditIssueAttachmentsService;
        private readonly IAuditFeedbackService _auditFeedbackService;
        private readonly IAuditBranchFeedbackService _auditBranchFeedbackService;
        private readonly IAuditFeedbackAttachmentsService _auditFeedbackAttachmentsService;
        private readonly IAuditBranchFeedbackAttachmentsService _auditBranchFeedbackAttachmentsService;
        private readonly IAuditUserService _auditUserService;
        private readonly IAuditIssueUserService _auditIssueUserService;
        private readonly IUserRollsService _userRollsService;
        private readonly IMemoryCache _memoryCache;
        private readonly ITeamMembersService _teamMembersService;
        private readonly ITeamsService _teamsService;
        private readonly ILogger<AuditController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDeshboardService _deshboardService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly INotificationService _notificationService;
        private readonly IMailService _mailService;
        private readonly IBKAuditTemlateMasterService _bkAuditTemlateMasterService;
        private readonly IAuditPointService _AuditPointService;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly ISettingsService _settingsService;

        public AuditController(
        ApplicationDbContext applicationDb,
        IAuditMasterService auditMasterService,
        IAuditAreasService auditAreasService,
        IAuditIssueService auditIssueService,
        IAuditIssueAttachmentsService auditIssueAttachmentsService,
        IAuditFeedbackService auditFeedbackService,
        IAuditBranchFeedbackService auditBranchFeedbackService,
        IUserRollsService userRollsService,
        IMemoryCache memoryCache,
        ITeamsService teamsService,
        ILogger<AuditController> logger,
        IConfiguration configuration,
        IAuditFeedbackAttachmentsService auditFeedbackAttachmentsService,
        IAuditUserService auditUserService,
        IAuditIssueUserService auditIssueUserService,
        ITeamMembersService teamMembersService,
        IAuditBranchFeedbackAttachmentsService auditBranchFeedbackAttachmentsService,
        IDeshboardService deshboardService,
        INotificationService notificationService,
        IWebHostEnvironment webHostEnvironment,
        IMailService mailService,
        IBKAuditTemlateMasterService bkAuditTemlateMasterService,
        IAuditPointService auditPointService,
        IFiscalYearService fiscalYearService,
        ISettingsService settingsService
        )
        {
            _applicationDb = applicationDb;
            _auditMasterService = auditMasterService;
            _auditAreasService = auditAreasService;
            _auditIssueService = auditIssueService;
            _auditIssueAttachmentsService = auditIssueAttachmentsService;
            _auditFeedbackService = auditFeedbackService;
            _auditFeedbackAttachmentsService = auditFeedbackAttachmentsService;
            _auditUserService = auditUserService;
            _auditIssueUserService = auditIssueUserService;
            _auditBranchFeedbackService = auditBranchFeedbackService;
            _userRollsService = userRollsService;
            _auditBranchFeedbackAttachmentsService = auditBranchFeedbackAttachmentsService;
            _auditUserService = auditUserService;
            _memoryCache = memoryCache;
            _teamsService = teamsService;
            _teamMembersService = teamMembersService;
            _logger = logger;
            _configuration = configuration;
            _deshboardService = deshboardService;
            _webHostEnvironment = webHostEnvironment;
            _notificationService = notificationService;
            _mailService = mailService;
            _bkAuditTemlateMasterService = bkAuditTemlateMasterService;
            _AuditPointService = auditPointService;
            _fiscalYearService = fiscalYearService;
            _settingsService = settingsService;
        }

        public async Task<IActionResult> Index(string edit, string? notification, int? id)
        {
            //Index 
            Notifications notifi = new Notifications();
            List<UserManuInfo> manu = new List<UserManuInfo>();
            AuditMaster master = new AuditMaster();
            string userName = User.Identity.Name;

            ResultModel<string> companyCode = _settingsService.GetSettingsValue(new[] { "SettingGroup", "SettingName" }, new[] { "Company", "Code" });


            #region SettingBranchName
            var cacheKey = $"{User.Identity.Name}_BranchClaim";
            if (_memoryCache.TryGetValue(cacheKey, out BranchClaim branchClaim))
            {
                string name = branchClaim.BranchName;

                var identity = new ClaimsIdentity(User.Identity);
                identity.AddClaim(new Claim(ClaimNames.CurrentBranch, branchClaim.BranchId.ToString()));
                identity.AddClaim(new Claim(ClaimNames.CurrentBranchName, branchClaim.BranchName.ToString().Trim()));

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties() { IssuedUtc = DateTimeOffset.Now, });

            }
            #endregion

            #region Notifications
            if (notification != null)
            {
                ResultModel<Notifications> result = _notificationService.UpdateNotification(id.ToString(), userName, notification);
            }
            #endregion

            manu = _userRollsService.GetUserManu(userName);
            master.Edit = edit;
            master.CompanyCode = companyCode.Data.ToUpper();

            return View(master);

        }

        public IActionResult Create()
        {

            ResultModel<string> companyCode = _settingsService.GetSettingsValue(new[] { "SettingGroup", "SettingName" }, new[] { "Company", "Code" });


            AuditMaster auditMaster = new AuditMaster()
            {
                Operation = "add",
                IsPlaned = true,
                AuditStatus = "NA",
                ReportStatus = "NA",
                CompanyCode = companyCode.Data.ToString(),
                IsApprovedL4 = true,
                IsCompleteIssue = true,
                IsCompleteIssueTeamFeedback = true
            };

            #region ByGiving true for all User allowing to enter without Any Error to AuditIndex
            auditMaster.IsApprovedL4 = true;
            auditMaster.IsCompleteIssue = true;
            auditMaster.IsCompleteIssueTeamFeedback = true;
            #endregion

            return View(auditMaster);
        }

        public ActionResult PendingAuditApproval(AuditMaster master)
        {

            ResultModel<AuditMaster> result = new ResultModel<AuditMaster>();
            MailSetting mailSetting = new MailSetting();
            ResultModel<List<UserProfile>> notifiEmil = new ResultModel<List<UserProfile>>();
            AuditMail auditMail = new AuditMail();

            try
            {
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                UserProfile urp = new UserProfile();
                urp.UserName = user.UserName;

                notifiEmil = _auditMasterService.GetEamil(urp);
                var email = notifiEmil.Data.FirstOrDefault();
                var currentUrl = HttpContext.Request.GetDisplayUrl();
                string[] parts = new string[] { "", "" };
                parts = currentUrl.TrimStart('/').Split('/');
                string urlhttp = parts[0];
                string HostUrl = parts[2];
                auditMail.To = "nurul.amin@symphonysoftt.com";
                auditMail.URL = urlhttp + "//" + HostUrl + "/Audit/ApproveStatusIndex";
                auditMail.Name = "TestAudit";
                auditMail.Status = "PendingAuditApproval3";
                _mailService.SendEmail(auditMail);
                //MailService.PendingAuditApprovalEamil("nurul.amin@symphonysoftt.com", urlhttp + "//" + HostUrl + "/Audit/ApproveStatusIndex", "TestAudit");
                master.Status = "200";
                return Ok(master);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }
            return Ok(result);
        }

        [HttpPost]
        public ActionResult ReportCreateEdit(AuditIssue master)
        {
            ResultModel<AuditIssue> result = new ResultModel<AuditIssue>();
            try
            {
                if (master.Operation == "update")
                {
                    result = _auditIssueService.ReportStatusUpdate(master);
                    return Ok(result);
                }
                else
                {
                    if (result.Status == Status.Fail)
                    {
                        throw result.Exception;
                    }
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
        public ActionResult AuditStatusCreateEdit(AuditMaster master)
        {
            ResultModel<AuditMaster> result = new ResultModel<AuditMaster>();
            try
            {
                if (master.Operation == "update")
                {
                    result = _auditMasterService.AuditStatusUpdate(master);
                    return Ok(result);
                }
                else
                {
                    if (result.Status == Status.Fail)
                    {
                        throw result.Exception;
                    }
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }
            return Ok(result);
        }

        public IActionResult ExcelIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ExcelCreateEdit(IFormFile Attachments, AuditMaster masterlist, List<AuditMaster> AuditMasterList)
        {
            ResultModel<AuditMaster> result = new ResultModel<AuditMaster>();
            List<AuditMaster> auditlist = new List<AuditMaster>();
            try
            {
                if (Attachments == null)
                {
                    string data = "failed";
                    return Ok(data);
                }
                using (var stream = new MemoryStream())
                {
                    Attachments.CopyTo(stream);

                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        int lastRow = worksheet.Cells["A" + worksheet.Dimension.End.Row.ToString()].End.Row;
                        int rowCountWithValues = 1;

                        for (int row = 2; row <= lastRow; row++)
                        {
                            if (

                                worksheet.Cells[row, 1].Value != null || worksheet.Cells[row, 2].Value != null ||
                                worksheet.Cells[row, 3].Value != null || worksheet.Cells[row, 4].Value != null ||
                                worksheet.Cells[row, 5].Value != null || worksheet.Cells[row, 6].Value != null ||
                                worksheet.Cells[row, 7].Value != null || worksheet.Cells[row, 9].Value != null ||
                                worksheet.Cells[row, 8].Value != null || worksheet.Cells[row, 10].Value != null
                            )
                            {
                                rowCountWithValues++;
                            }
                        }

                        for (int row = 2; row <= rowCountWithValues; row++)
                        {
                            var master = new AuditMaster();

                            master.Name = worksheet.Cells[row, 1].Value?.ToString();
                            master.StartDate = worksheet.Cells[row, 2].Value?.ToString();
                            master.EndDate = worksheet.Cells[row, 3].Value?.ToString();
                            var planed = worksheet.Cells[row, 4].Value?.ToString();

                            if (planed == "True" || planed == "true") { master.IsPlaned = true; } else { master.IsPlaned = false; }

                            master.AuditTypeName = worksheet.Cells[row, 5].Value?.ToString();
                            master.AuditStatus = worksheet.Cells[row, 6].Value?.ToString();
                            master.TeamName = worksheet.Cells[row, 7].Value?.ToString();
                            master.BranchID = worksheet.Cells[row, 8].Value?.ToString();
                            master.BusinessTarget = worksheet.Cells[row, 9].Value?.ToString();
                            master.Remarks = worksheet.Cells[row, 10].Value?.ToString();

                            auditlist.Add(master);

                            if (result.Status == Status.Fail)
                            {
                                throw result.Exception;
                            }
                        }
                    }
                }
                ViewBag.AuditList = auditlist;
                return View();
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }
            return View();
        }

        [HttpPost]
        public ActionResult ExcelSaveCreateEdit([FromBody] AuditMaster auditMaster)
        {
            ResultModel<AuditMaster> result = new ResultModel<AuditMaster>();
            List<AuditMaster> auditlist = new List<AuditMaster>();
            ResultModel<List<AuditUser>> UserData = new ResultModel<List<AuditUser>>();

            try
            {
                PeramModel pm = new PeramModel();

                for (int row = 0; row < auditMaster.auditMasterList.Count(); row++)
                {
                    var master = new AuditMaster();
                    var audit = auditMaster.auditMasterList[row];
                    master.Name = audit.Name.ToString();
                    master.StartDate = audit.StartDate.ToString();
                    master.EndDate = audit.EndDate.ToString();
                    var planed = audit.IsPlaned.ToString();
                    if (planed == "True" || planed == "true") { master.IsPlaned = true; } else { master.IsPlaned = false; }
                    master.AuditTypeName = audit.AuditTypeName.ToString();
                    master.AuditTypeId = _teamsService.GetAuditTypeeByID(null, master.AuditTypeName, null, null);
                    master.AuditStatus = audit.AuditStatus.ToString();
                    master.TeamName = audit.TeamName.ToString();
                    master.TeamId = _teamsService.GetSingleValeByID(null, master.TeamName, null, null);
                    master.BranchID = audit.BranchID.ToString();
                    master.BranchID = _teamsService.GetBranchByID(null, master.BranchID, null, null);
                    master.BusinessTarget = audit.BusinessTarget.ToString();
                    master.Remarks = audit.Remarks.ToString();
                    master.Remarks = Base64Encode(master.Remarks.ToString());

                    auditlist.Add(master);

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = "";
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = "";
                    master.ReportStatus = "NA";
                    master.AuditStatus = "NA";
                    master.CompanyId = "1";

                    result = _auditMasterService.Insert(master);

                    var currentUrl = HttpContext.Request.GetDisplayUrl();
                    string[] parts = new string[] { "", "" };
                    parts = currentUrl.TrimStart('/').Split('/');
                    string urlhttp = parts[0];
                    string HostUrl = parts[2];
                    string AuditUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + result.Data.Id + "?edit=auditStatus";
                    string IssueUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + result.Data.Id + "?edit=issueApprove";
                    string BranchFeedUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + result.Data.Id + "?edit=branchFeedbackApprove";
                    string AuditPreviewUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + result.Data.Id + "?edit=issue";

                    MailSetting ms = new MailSetting();
                    ms.ApprovedUrl = AuditUrl;
                    ms.AutidId = result.Data.Id;
                    ms.Status = "Audit";
                    _auditMasterService.SaveUrl(ms);

                    ms.ApprovedUrl = IssueUrl;
                    ms.AutidId = result.Data.Id;
                    ms.Status = "Issue";
                    _auditMasterService.SaveUrl(ms);

                    ms.ApprovedUrl = BranchFeedUrl;
                    ms.AutidId = result.Data.Id;
                    ms.Status = "BranchFeedback";
                    _auditMasterService.SaveUrl(ms);

                    UserData = _auditMasterService.GetAuditUserTeamId(master.TeamId, pm);

                    foreach (var User in UserData.Data)
                    {
                        User.Audit.CreatedBy = user.UserName;
                        User.AuditId = result.Data.Id;
                        User.Audit.CreatedOn = DateTime.Now;
                        User.Audit.CreatedFrom = "";
                        User.Audit.LastUpdateBy = user.UserName;
                        User.Audit.LastUpdateOn = DateTime.Now;
                        _auditUserService.Insert(User);
                    }
                    if (result.Status == Status.Fail)
                    {
                        throw result.Exception;
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }
            return Ok();

        }

        [HttpPost]
        public ActionResult SendEmailCreateEdit(AuditMaster master)
        {

            ResultModel<AuditMaster> result = new ResultModel<AuditMaster>();
            ResultModel<List<AuditUser>> UserData = new ResultModel<List<AuditUser>>();
            AuditMail auditMail = new AuditMail();

            try
            {
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);

                master.Audit.CreatedBy = user.UserName;
                master.Audit.CreatedOn = DateTime.Now;
                master.Audit.CreatedFrom = "";
                master.Audit.LastUpdateBy = user.UserName;
                master.Audit.LastUpdateOn = DateTime.Now;
                master.Audit.LastUpdateFrom = "";
                master.ReportStatus = "NA";
                master.AuditStatus = "NA";
                master.CompanyId = "1";

                string[] parts = new string[] { "", "" };
                var currentUrl = HttpContext.Request.GetDisplayUrl();
                parts = currentUrl.TrimStart('/').Split('/');
                string urlhttp = parts[0];
                string HostUrl = parts[2];
                string AuditPreviewUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + master.Id + "?edit=audit";

                UserData = _auditMasterService.GetAuditUserAuditId(master.Id.ToString());

                foreach (var User in UserData.Data)
                {
                    auditMail.To = User.EmailAddress;
                    auditMail.URL = AuditPreviewUrl;
                    auditMail.Name = "";
                    auditMail.Status = "PendingAuditApproval2";
                    var model = _mailService.SendEmail(auditMail);
                }

                if (result.Status == Status.Fail)
                {
                    throw result.Exception;
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }
            return Ok(result);
        }

        [HttpPost]
        public ActionResult CreateEdit(AuditMaster master)
        {

            ResultModel<AuditMaster> result = new ResultModel<AuditMaster>();
            ResultModel<List<AuditUser>> UserData = new ResultModel<List<AuditUser>>();
            PeramModel pm = new PeramModel();
            Notifications notifiaction = new Notifications();

            try
            {

                ResultModel<string> companyCode = _settingsService.GetSettingsValue(new[] { "SettingGroup", "SettingName" }, new[] { "Company", "Code" });
                ResultModel<List<AuditMaster>> teamResult = _auditMasterService.GetAll(new[] { "Id" }, new[] { master.Id.ToString() });
                ResultModel<List<TeamMembers>> teamMembers = _teamMembersService.GetAll(new[] { "U.TeamId" }, new[] { master.TeamId.ToString() });

                if (master.Operation == "update")
                {
                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    UserData = _auditMasterService.GetAuditUserTeamId(master.TeamId, pm);

                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = "";
                    master.AuditUsers = UserData.Data;
                    master.TeamValue = teamResult.Data.FirstOrDefault().TeamId.ToString();


                    if (master.AuditStatus == null)
                    {
                        master.AuditStatus = "NA";
                    }
                    if (master.ReportStatus == null)
                    {
                        master.ReportStatus = "NA";
                    }

                    result = _auditMasterService.Update(master);

                    #region Update Team


                    //if (master.TeamId != master.TeamValue)
                    //{
                    //    if (result.Data != null)
                    //    {
                    //        UserData = _auditMasterService.GetAuditUserTeamId(master.TeamId, pm);
                    //        master.TeamValue = master.TeamId;
                    //        if (UserData.Data.Count() == 0)
                    //        {
                    //            var item = _auditUserService.Insert(null);
                    //            return Ok(item);
                    //        }
                    //        var currentUrl = HttpContext.Request.GetDisplayUrl();
                    //        string[] parts = new string[] { "", "" };
                    //        parts = currentUrl.TrimStart('/').Split('/');
                    //        string urlhttp = parts[0];
                    //        string HostUrl = parts[2];
                    //        string AuditUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + result.Data.Id + "?edit=auditStatus";
                    //        string IssueUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + result.Data.Id + "?edit=issueApprove";
                    //        string BranchFeedUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + result.Data.Id + "?edit=branchFeedbackApprove";
                    //        string AuditPreviewUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + result.Data.Id + "?edit=audit";
                    //        MailSetting ms = new MailSetting();
                    //        ms.ApprovedUrl = AuditUrl;
                    //        ms.AutidId = result.Data.Id;
                    //        ms.Status = "Audit";
                    //        _auditMasterService.SaveUrl(ms);
                    //        ms.ApprovedUrl = IssueUrl;
                    //        ms.AutidId = result.Data.Id;
                    //        ms.Status = "Issue";
                    //        _auditMasterService.SaveUrl(ms);
                    //        ms.ApprovedUrl = BranchFeedUrl;
                    //        ms.AutidId = result.Data.Id;
                    //        ms.Status = "BranchFeedback";
                    //        _auditMasterService.SaveUrl(ms);
                    //        foreach (var User in UserData.Data)
                    //        {
                    //            User.Audit.CreatedBy = user.UserName;
                    //            User.AuditId = result.Data.Id;
                    //            User.Audit.CreatedOn = DateTime.Now;
                    //            User.Audit.CreatedFrom = "";
                    //            User.Audit.LastUpdateBy = user.UserName;
                    //            User.Audit.LastUpdateOn = DateTime.Now;
                    //            _auditUserService.Insert(User);
                    //        }
                    //    }
                    //}

                    #endregion

                    if (result.Status == Status.Fail)
                    {
                        Exception ex = new Exception();
                        _logger.LogError(ex, "An error occurred in the Index action.");
                    }

                    return Ok(result);

                }
                else
                {

                    UserData = _auditMasterService.GetAuditUserTeamId(master.TeamId, pm);
                    master.TeamValue = master.TeamId;

                    if (UserData.Data.Count() == 0)
                    {
                        var item = new ResultModel<AuditUser>()
                        {
                            Status = Status.Fail,
                            Message = "You have to assign team member first for team",
                            Data = null
                        };
                        return Ok(item);
                    }

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = "";
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = "";
                    master.ReportStatus = "NA";
                    master.AuditStatus = "NA";
                    master.CompanyId = "1";


                    //FiscaleYearCheck

                    DateTime today = DateTime.UtcNow;
                    DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
                    string formattedDate = firstDayOfMonth.ToString("dd-MMM-yyyy");
                    string year = YearChange.BeginingYear;
                    string yearData = DateTime.Parse(YearChange.BeginingYear).Year.ToString();
                    string[] conditionalFields = new[] { "Year", "MonthStart" };
                    string?[] conditionalValue = new[] { yearData, formattedDate };
                    PeramModel pmData = new PeramModel();
                    pmData.FiscalYear = "FiscalYear";

                    //ResultModel<List<FiscalYearDetailVM>> FiscalYearData = _fiscalYearService.GetAllDetail(conditionalFields, conditionalValue, pmData);
                    //if(FiscalYearData.Data.Count == 0)
                    //{
                    //    var FiscalYear = new ResultModel<AuditMaster>()
                    //    {
                    //        Status = Status.Fail,
                    //        Message = "This FiscalYear Is Not Exixt",
                    //        Data = null
                    //    };
                    //    return Ok(FiscalYear);                              
                    //}
                    //if(FiscalYearData.Data.FirstOrDefault().MonthLock == true)
                    //{
                    //    var FiscalYear = new ResultModel<AuditMaster>()
                    //    {
                    //        Status = Status.Fail,
                    //        Message = "This Fiscal Year Month Is Lock",
                    //        Data = null
                    //    };
                    //    return Ok(FiscalYear);
                    //}

                    //End


                    result = _auditMasterService.Insert(master);


                    var currentUrl = HttpContext.Request.GetDisplayUrl();
                    string[] parts = new string[] { "", "" };
                    parts = currentUrl.TrimStart('/').Split('/');
                    string urlhttp = parts[0];
                    string HostUrl = parts[2];
                    string AuditUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + result.Data.Id + "?edit=auditStatus";
                    string IssueUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + result.Data.Id + "?edit=issueApprove";
                    string BranchFeedUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + result.Data.Id + "?edit=branchFeedbackApprove";
                    string AuditPreviewUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + result.Data.Id + "?edit=audit";

                    //Audit
                    MailSetting ms = new MailSetting();
                    ms.ApprovedUrl = AuditUrl;
                    ms.AutidId = result.Data.Id;
                    ms.Status = "Audit";
                    _auditMasterService.SaveUrl(ms);

                    //Issue
                    ms.ApprovedUrl = IssueUrl;
                    ms.AutidId = result.Data.Id;
                    ms.Status = "Issue";
                    _auditMasterService.SaveUrl(ms);

                    //BranchFeedback
                    ms.ApprovedUrl = BranchFeedUrl;
                    ms.AutidId = result.Data.Id;
                    ms.Status = "BranchFeedback";
                    _auditMasterService.SaveUrl(ms);

                    foreach (var User in UserData.Data)
                    {
                        User.Audit.CreatedBy = user.UserName;
                        User.AuditId = result.Data.Id;
                        User.Audit.CreatedOn = DateTime.Now;
                        User.Audit.CreatedFrom = "";
                        User.Audit.LastUpdateBy = user.UserName;
                        User.Audit.LastUpdateOn = DateTime.Now;
                        _auditUserService.Insert(User);
                    }


                    if (companyCode.Data.ToUpper() == "BRAC")
                    {

                        ResultModel<List<AuditPoint>> AuditPoint = _AuditPointService.GetAll(new[] { "AuditTypeId" }, new[] { result.Data.AuditTypeId.ToString() });

                        foreach (var auditpoint in AuditPoint.Data)
                        {
                            ResultModel<AuditAreas> Area = new ResultModel<AuditAreas>();
                            AuditAreas areamaster = new AuditAreas();

                            areamaster.AuditId = result.Data.Id;
                            areamaster.AuditTypeId = auditpoint.AuditTypeId;
                            areamaster.AuditPointId = auditpoint.Id;
                            areamaster.WeightPersent = auditpoint.WeightPersent;
                            areamaster.P_Level = auditpoint.P_Level;
                            areamaster.PID = auditpoint.PId;
                            areamaster.AuditPointName = auditpoint.AuditType;

                            Area = _auditAreasService.Insert(areamaster);
                        }
                    }

                    #region Notification

                    string Url = urlhttp + "//" + HostUrl + "/Audit?edit=Audit&notification=Audit&id=" + result.Data.Id;
                    ResultModel<Notifications> resultNotifi = _notificationService.InsertNotification(Url, master.Name, result.Data.Id, 0, user.UserName, "Audit", teamMembers.Data);

                    #endregion

                    if (result.Status == Status.Fail)
                    {
                        Exception ex = new Exception();
                        _logger.LogError(ex, "An error occurred in the Index action.");
                        throw result.Exception;
                    }

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
        public ActionResult ReportSeeAllCreateEdit(AuditReportHeading master)
        {
            ResultModel<AuditReportHeading> result = new ResultModel<AuditReportHeading>();

            try
            {
                if (master.Operation == "update")
                {

                    result = _auditMasterService.AuditReportHeadingUpdate(master);
                    if (result.Status == Status.Fail)
                    {
                        Exception ex = new Exception();
                        _logger.LogError(ex, "An error occurred in the Index action.");
                        throw result.Exception;
                    }
                    return Ok(result);

                }
                else
                {
                    result = _auditMasterService.AuditReportHeadingInsert(master);

                    if (result.Status == Status.Fail)
                    {
                        Exception ex = new Exception();
                        _logger.LogError(ex, "An error occurred in the Index action.");
                        throw result.Exception;
                    }

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
        public ActionResult ReportHeadingCreateEdit(AuditReportHeading master)
        {
            ResultModel<AuditReportHeading> result = new ResultModel<AuditReportHeading>();
            try
            {
                if (master.Operation == "update")
                {
                    result = _auditMasterService.AuditReportHeadingUpdate(master);

                    if (result.Status == Status.Fail)
                    {
                        Exception ex = new Exception();
                        _logger.LogError(ex, "An error occurred in the Index action.");
                        throw result.Exception;
                    }
                    return Ok(result);
                }
                else
                {
                    result = _auditMasterService.AuditReportHeadingInsert(master);

                    if (result.Status == Status.Fail)
                    {
                        Exception ex = new Exception();
                        _logger.LogError(ex, "An error occurred in the Index action.");
                        throw result.Exception;
                    }

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
        public ActionResult SecondReportHeadingCreateEdit(AuditReportHeading master)
        {
            ResultModel<AuditReportHeading> result = new ResultModel<AuditReportHeading>();
            try
            {
                if (master.Operation == "update")
                {
                    master.Check = "SecondReportHeading";
                    result = _auditMasterService.AuditReportHeadingUpdate(master);

                    if (result.Status == Status.Fail)
                    {
                        Exception ex = new Exception();
                        _logger.LogError(ex, "An error occurred in the Index action.");
                        throw result.Exception;
                    }

                    return Ok(result);
                }
                else
                {
                    master.Check = "SecondReportHeading";

                    result = _auditMasterService.AuditReportHeadingInsert(master);

                    if (result.Status == Status.Fail)
                    {
                        Exception ex = new Exception();
                        _logger.LogError(ex, "An error occurred in the Index action.");
                        throw result.Exception;
                    }

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
        public ActionResult AnnexureReportCreateEdit(AuditReportHeading master)
        {
            ResultModel<AuditReportHeading> result = new ResultModel<AuditReportHeading>();
            try
            {
                if (master.Operation == "update")
                {
                    master.Check = "AnnexureReport";
                    result = _auditMasterService.AuditReportHeadingUpdate(master);

                    if (result.Status == Status.Fail)
                    {
                        Exception ex = new Exception();
                        _logger.LogError(ex, "An error occurred in the Index action.");
                        throw result.Exception;
                    }

                    return Ok(result);
                }
                else
                {
                    master.Check = "AnnexureReport";
                    result = _auditMasterService.AuditReportHeadingInsert(master);

                    if (result.Status == Status.Fail)
                    {
                        Exception ex = new Exception();
                        _logger.LogError(ex, "An error occurred in the Index action.");
                        throw result.Exception;
                    }

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
        public ActionResult AreaCreateEdit(AuditAreas master)
        {
            ResultModel<AuditAreas> result = new ResultModel<AuditAreas>();
            try
            {
                if (master.Operation == "update")
                {
                    result = _auditAreasService.Update(master);

                    if (result.Status == Status.Fail)
                    {
                        Exception ex = new Exception();
                        _logger.LogError(ex, "An error occurred in the Index action.");
                        throw result.Exception;
                    }

                    return Ok(result);
                }
                else
                {

                    master.PID = "";
                    master.AuditPointName = "";

                    result = _auditAreasService.Insert(master);

                    if (result.Status == Status.Fail)
                    {
                        Exception ex = new Exception();
                        _logger.LogError(ex, "An error occurred in the Index action.");
                        throw result.Exception;
                    }

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
        public ActionResult AuditUserCreateEdit(AuditUser master)
        {
            ResultModel<AuditUser> result = new ResultModel<AuditUser>();
            AuditMail auditMail = new AuditMail();
            ResultModel<List<AuditUser>> indexData = new ResultModel<List<AuditUser>>();
            ResultModel<List<AuditUser>> TeamUser = new ResultModel<List<AuditUser>>();
            ResultModel<List<AuditMaster?>> AuditMaster = new ResultModel<List<AuditMaster?>>();
            AuditMaster? audit = new AuditMaster();

            try
            {
                PeramModel pm = new PeramModel();

                if (master.Operation == "update")
                {
                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = "";

                    #region GettingTeamAndSendMassage
                    IndexModel index = new IndexModel();
                    index.startRec = 0;
                    index.OrderName = "Id";
                    index.orderDir = "desc";
                    index.pageSize = 100;
                    index.AuditId = master.AuditId;
                    string[] conditionalFields = { };
                    string?[] conditionalValue = { };
                    indexData = _auditUserService.GetIndexData(index, conditionalFields, conditionalValue);
                    TeamUser = null;

                    foreach (var auditUser in indexData.Data)
                    {
                        if (auditUser.TeamId == 0.ToString())
                        {
                            //
                        }
                        else
                        {
                            TeamUser = _auditMasterService.GetAuditUserTeamId(auditUser.TeamId, pm);
                        }
                    }

                    foreach (var teamuser in TeamUser.Data)
                    {
                        if (master.UserId == teamuser.UserId)
                        {
                            master.IsUserInsert = false;
                            var item = _auditUserService.Insert(master);
                            return Ok(item);
                        }
                    }

                    master.TeamId = 0.ToString();

                    #endregion


                    result = _auditUserService.Update(master);


                    if (result.Status == Status.Fail)
                    {
                        Exception ex = new Exception();
                        _logger.LogError(ex, "An error occurred in the Index action.");
                        throw result.Exception;
                    }

                    return Ok(result);

                }
                else
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = "";

                    #region Getting Team And Send Massage

                    IndexModel index = new IndexModel();
                    index.startRec = 0;
                    index.OrderName = "Id";
                    index.orderDir = "desc";
                    index.pageSize = 100;
                    index.AuditId = master.AuditId;
                    string[] conditionalFields = { };
                    string?[] conditionalValue = { };
                    indexData = _auditUserService.GetIndexData(index, conditionalFields, conditionalValue);
                    TeamUser = null;

                    foreach (var auditUser in indexData.Data)
                    {
                        if (auditUser.TeamId == 0.ToString())
                        {
                            //
                        }
                        else
                        {
                            TeamUser = _auditMasterService.GetAuditUserTeamId(auditUser.TeamId, pm);
                        }
                    }
                    foreach (var teamuser in TeamUser.Data)
                    {
                        if (master.UserId == teamuser.UserId)
                        {
                            master.IsUserInsert = false;
                            var item = _auditUserService.Insert(master);
                            return Ok(item);
                        }
                    }
                    master.TeamId = 0.ToString();

                    #endregion


                    result = _auditUserService.Insert(master);


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
                    string AuditPreviewUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + result.Data.AuditId + "?edit=feedback";

                    AuditMaster = _auditMasterService.GetAll(new[] { "Id" }, new[] { result.Data.AuditId.ToString() });
                    audit = AuditMaster.Data.FirstOrDefault();

                    auditMail.To = master.EmailAddress;
                    auditMail.URL = AuditPreviewUrl;
                    auditMail.Name = audit.Name;
                    auditMail.Status = "AuditIssue";
                    var model = _mailService.SendEmail(auditMail);

                    if (result.Status == Status.Fail)
                    {
                        Exception ex = new Exception();
                        _logger.LogError(ex, "An error occurred in the Index action.");
                        throw result.Exception;
                    }

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
        public ActionResult AuditIssueUserCreateEdit(AuditIssueUser master)
        {
            ResultModel<AuditIssueUser> result = new ResultModel<AuditIssueUser>();
            ResultModel<List<AuditMaster?>> AuditMaster = new ResultModel<List<AuditMaster?>>();
            AuditMaster? audit = new AuditMaster();

            try
            {
                if (master.Operation == "update")
                {
                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Issue.LastUpdateBy = user.UserName;
                    master.Issue.LastUpdateOn = DateTime.Now;
                    master.Issue.LastUpdateFrom = "";

                    result = _auditIssueUserService.Update(master);

                    if (result.Status == Status.Fail)
                    {
                        Exception ex = new Exception();
                        _logger.LogError(ex, "An error occurred in the Index action.");
                        throw result.Exception;
                    }
                    return Ok(result);
                }
                else
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Issue.CreatedBy = user.UserName;
                    master.Issue.CreatedOn = DateTime.Now;
                    master.Issue.CreatedFrom = "";

                    result = _auditIssueUserService.Insert(master);

                    UserProfile urp = new UserProfile();
                    urp.Id = master.AuditId;
                    urp.UserName = user.UserName;
                    MailSetting mailSetting = new MailSetting();
                    mailSetting.Id = master.AuditId;

                    var notifiEmai = _auditMasterService.GetEamil(urp);
                    var email = notifiEmai.Data.FirstOrDefault();
                    AuditMaster = _auditMasterService.GetAll(new[] { "Id" }, new[] { master.AuditId.ToString() });
                    audit = AuditMaster.Data.FirstOrDefault();

                    var currentUrl = HttpContext.Request.GetDisplayUrl();
                    string[] parts = new string[] { "", "" };
                    parts = currentUrl.TrimStart('/').Split('/');
                    string urlhttp = parts[0];
                    string HostUrl = parts[2];

                    string AuditPreviewUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + result.Data.AuditId + "?edit=Branchfeedback";

                    if (result.Status == Status.Fail)
                    {
                        Exception ex = new Exception();
                        _logger.LogError(ex, "An error occurred in the Index action.");
                        throw result.Exception;
                    }

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
        public ActionResult AuditIssueUserAllEmailCreateEdit(AuditIssueUser master)
        {
            ResultModel<AuditIssueUser> result = new ResultModel<AuditIssueUser>();
            ResultModel<AuditMail> model = new ResultModel<AuditMail>();
            ResultModel<List<AuditMaster?>> AuditMaster = new ResultModel<List<AuditMaster?>>();
            AuditMaster? audit = new AuditMaster();
            int mailCount = 0;

            try
            {
                AuditMail auditMail = new AuditMail();
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
                string AuditPreviewUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + master.AuditId + "?edit=Branchfeedback&common=branch";

                AuditMaster = _auditMasterService.GetAll(new[] { "Id" }, new[] { master.AuditId.ToString() });
                audit = AuditMaster.Data.FirstOrDefault();

                if (audit.IsCompleteIssueTeamFeedback == true)
                {
                    foreach (var item in master.AuditIssueUserList)
                    {
                        mailCount = _auditIssueUserService.GetAuditIssueUserById(new[] { "Id" }, new[] { item.Id.ToString() }).Data.FirstOrDefault().MailCount;
                        mailCount = mailCount + 1;
                        master.MailCount = mailCount;
                        auditMail.To = item.EmailAddress;
                        auditMail.URL = AuditPreviewUrl;
                        auditMail.Name = audit.Name;
                        auditMail.WebRoot = _webHostEnvironment.WebRootPath;
                        auditMail.Status = "BranchFeedbackUserMail";

                        model = _mailService.SendEmail(auditMail);

                        result = _auditIssueUserService.AudiIssueUserUpdateById(master);
                    }
                }

                return Ok(model);

            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);
        }

        public async Task<ActionResult<IList<AuditMaster>>> Edit(int id, string edit = "audit", string common = null)
        {
            Notifications notificnn = new Notifications();
            ResultModel<List<AuditMaster?>> result = new ResultModel<List<AuditMaster?>>();
            ResultModel<List<AuditUser>> UserData = new ResultModel<List<AuditUser>>();
            ResultModel<List<AuditApprove?>> auditData = new ResultModel<List<AuditApprove?>>();
            ResultModel<List<UserRolls?>> rolls = new ResultModel<List<UserRolls?>>();
            AuditApprove? ap = new AuditApprove();
            UserRolls? rl = new UserRolls();
            ResultModel<List<AuditIssueUser>> IssueUser = new ResultModel<List<AuditIssueUser>>();

            try
            {

                ResultModel<string> companyCode = _settingsService.GetSettingsValue(new[] { "SettingGroup", "SettingName" }, new[] { "Company", "Code" });

                string user = User.Identity.Name;

                #region SettingBranchName
                var cacheKey = $"{User.Identity.Name}_BranchClaim";
                if (_memoryCache.TryGetValue(cacheKey, out BranchClaim branchClaim))
                {
                    string name = branchClaim.BranchName;
                    var identity = new ClaimsIdentity(User.Identity);
                    identity.AddClaim(new Claim(ClaimNames.CurrentBranch, branchClaim.BranchId.ToString()));
                    identity.AddClaim(new Claim(ClaimNames.CurrentBranchName, branchClaim.BranchName.ToString().Trim()));
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal,
                        new AuthenticationProperties() { IssuedUtc = DateTimeOffset.Now, });

                }
                #endregion

                #region Notification
                if (common != null && common != "branch")
                {
                    string[] parts = common.Split(' ');
                    string notificationType = parts[0];
                    string notificationId = parts[1];

                    if (notificationType != null)
                    {
                        notificnn = SetNotificationValueForUpdate(notificationId, user, notificationType);
                        _notificationService.Update(notificnn);
                    }

                }
                #endregion 

                #region UserIsCorrectOrNotFromEmail
                if (common == "branch")
                {
                    bool IsBranchUser = _deshboardService.AuditBranchUserGetAll(user);
                    if (!IsBranchUser)
                    {
                        return RedirectToAction("Index", "LogIn");
                    }
                }
                #endregion


                result = _auditMasterService.GetAll(new[] { "Id" }, new[] { id.ToString() });


                if (result.Status == Status.Fail)
                {
                    throw result.Exception;
                }

                AuditMaster? auditMaster = result.Data.FirstOrDefault();

                if (auditMaster is null)
                {
                    return RedirectToAction("Index");
                }

                auditMaster.AuditAreas = _auditAreasService.GetAll(new[] { "AuditId" }, new[] { id.ToString() }).Data;
                auditMaster.Operation = "update";
                auditMaster.Edit = edit;
                auditMaster.ApproveStatus = edit;
                auditMaster.TeamValue = auditMaster.TeamId;
                string userName = User.Identity.Name;

                #region ForShowingAlreadyApprovedButtonForAuditIssueBranchFeedback

                auditData = _auditMasterService.GetAuditById(id, userName);
                rolls = _auditMasterService.GetUserRoles(userName);
                ap = auditData.Data.FirstOrDefault();
                rl = rolls.Data.FirstOrDefault();

                int countRL = 0;
                int countAP = 0;

                if (ap != null)
                {
                    auditMaster.auditApprove = ap;
                }

                if (edit == "auditStatus")
                {
                    //UserRolls
                    if (rl.AuditApproval1)
                    {
                        countRL = countRL + 1;
                    }
                    if (rl.AuditApproval2)
                    {
                        countRL = countRL + 1;
                    }
                    if (rl.AuditApproval3)
                    {
                        countRL = countRL + 1;
                    }
                    if (rl.AuditApproval4)
                    {
                        countRL = countRL + 1;
                    }
                    //AuditApprove
                    if (ap.IsApprovedL1 && userName == ap.ApprovedByL1)
                    {
                        countAP = countAP + 1;
                    }
                    if (ap.IsApprovedL2 && userName == ap.ApprovedByL2)
                    {
                        countAP = countAP + 1;
                    }
                    if (ap.IsApprovedL3 && userName == ap.ApprovedByL3)
                    {
                        countAP = countAP + 1;
                    }
                    if (ap.IsApprovedL4 && userName == ap.ApprovedByL4)
                    {
                        countAP = countAP + 1;
                    }

                    if (countRL > countAP)
                    {
                    }
                    else
                    {
                        auditMaster.HideMessage = "AuditHide";
                    }

                }
                if (edit == "issueApprove")
                {
                    //UserRolls
                    if (rl.AuditIssueApproval1)
                    {
                        countRL = countRL + 1;
                    }
                    if (rl.AuditIssueApproval2)
                    {
                        countRL = countRL + 1;
                    }
                    if (rl.AuditIssueApproval3)
                    {
                        countRL = countRL + 1;
                    }
                    if (rl.AuditIssueApproval4)
                    {
                        countRL = countRL + 1;
                    }
                    //AuditApprove
                    if (ap.IssueIsApprovedL1)
                    {
                        countAP = countAP + 1;
                    }
                    if (ap.IssueIsApprovedL2)
                    {
                        countAP = countAP + 1;
                    }
                    if (ap.IssueIsApprovedL3)
                    {
                        countAP = countAP + 1;
                    }
                    if (ap.IssueIsApprovedL4)
                    {
                        countAP = countAP + 1;
                    }
                    if (countRL > countAP)
                    {
                    }
                    else
                    {
                        auditMaster.HideMessage = "IssueHide";
                    }
                }
                if (edit == "branchFeedbackApprove")
                {
                    //UserRolls
                    if (rl.AuditFeedbackApproval1)
                    {
                        countRL = countRL + 1;
                    }
                    if (rl.AuditFeedbackApproval2)
                    {
                        countRL = countRL + 1;
                    }
                    if (rl.AuditFeedbackApproval3)
                    {
                        countRL = countRL + 1;
                    }
                    if (rl.AuditFeedbackApproval4)
                    {
                        countRL = countRL + 1;
                    }
                    //AuditApprove
                    if (ap.BFIsApprovedL1)
                    {
                        countAP = countAP + 1;
                    }
                    if (ap.BFIsApprovedL2)
                    {
                        countAP = countAP + 1;
                    }
                    if (ap.BFIsApprovedL3)
                    {
                        countAP = countAP + 1;
                    }
                    if (ap.BFIsApprovedL4)
                    {
                        countAP = countAP + 1;
                    }
                    if (countRL > countAP)
                    {
                    }
                    else
                    {
                        auditMaster.HideMessage = "BranchFeedbackHide";
                    }
                }

                #endregion

                #region HODButtonShowOrNotShow
                UserData = _auditMasterService.GetAuditUserTeamId(auditMaster.TeamId);
                if (UserData.Data != null)
                {
                    foreach (AuditUser audit in UserData.Data)
                    {
                        if (userName == audit.UserName)
                        {
                            auditMaster.IsHOD = true;
                            auditMaster.IsBranchComplete = true;
                            auditMaster.IsTeam = true;

                        }
                    }
                }
                #endregion

                #region CheckingReviewerTrueOrFalse
                if (auditMaster.IsHOD != true)
                {
                    auditMaster.IsReviewer = true;
                }
                #endregion

                #region CheckingAuditIssueUsers
                IssueUser = _auditMasterService.GetAuditIssueUserById(id);
                if (IssueUser.Data != null)
                {
                    foreach (AuditIssueUser audit in IssueUser.Data)
                    {
                        if (userName == audit.UserName)
                        {
                            auditMaster.IsIssueUser = true;
                        }
                    }
                }
                #endregion

                #region IssueFeedbackBranchFeedbackIconCheckedorUnChecked
                auditMaster.IssueCompleted = auditMaster.IsCompleteIssue;
                auditMaster.BranchFeedbackCompleted = auditMaster.IsCompleteIssueBranchFeedback;
                auditMaster.FeedbackCompleted = auditMaster.IsCompleteIssueTeamFeedback;
                #endregion

                #region IssueFeedbackBranchFeedbackIndexIconValidationSettingCode
                if (edit == "audit" || edit == "auditStatus" || edit == "issueApprove" || edit == "branchFeedbackApprove")
                {
                    auditMaster.IsApprovedL4 = true;
                    auditMaster.IsCompleteIssue = true;
                    auditMaster.IsCompleteIssueTeamFeedback = true;

                }
                else if (edit == "issue")
                {
                    auditMaster.IsApprovedL4 = auditMaster.IsApprovedL4;
                    auditMaster.IsCompleteIssue = true;
                    auditMaster.IsCompleteIssueTeamFeedback = true;
                }
                else if (edit == "feedback")
                {
                    auditMaster.IsApprovedL4 = true;
                    auditMaster.IsCompleteIssue = auditMaster.IsCompleteIssue;
                    auditMaster.IsCompleteIssueTeamFeedback = true;
                }
                else if (edit == "BranchFeedback")
                {
                    auditMaster.IsApprovedL4 = true;
                    auditMaster.IsCompleteIssue = true;
                    auditMaster.IsCompleteIssueTeamFeedback = auditMaster.IsCompleteIssueTeamFeedback;
                }
                #endregion

                #region ForMailSendingPart
                if (edit == "SendEmail")
                {
                    auditMaster.IsApprovedL4 = true;
                    auditMaster.IsCompleteIssue = true;
                    auditMaster.IsCompleteIssueTeamFeedback = true;
                }
                #endregion

                auditMaster.CompanyCode = companyCode.Data.ToString();

                return View("Create", auditMaster);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }
        public ActionResult<IList<AuditMaster>> Delete(AuditMaster master)
        {
            ResultModel<AuditMaster> result = new ResultModel<AuditMaster>();
            PeramModel pm = new PeramModel();
            ResultModel<List<AuditUser>> UserData = new ResultModel<List<AuditUser>>();

            try
            {

                #region ChackingTeamUsers
                UserData = _auditMasterService.GetAuditUserTeamId(master.TeamValue, pm);
                if (UserData.Data != null)
                {
                    foreach (AuditUser audit in UserData.Data)
                    {
                        if (master.UserId == audit.UserId)
                        {
                            var res = new ResultModel<AuditMaster>()
                            {
                                Status = Status.Fail,
                                Message = "This Is Team. You Can not Delete This User Name"
                            };
                            return Ok(res);
                        }
                    }
                }
                #endregion

                result = _auditMasterService.Delete(master.Id);
                return Ok(result);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }

        public ActionResult<IList<AuditMaster>> SendToHOD(AuditMaster master)
        {
            ResultModel<AuditMaster> result = new ResultModel<AuditMaster>();
            ResultModel<List<AuditMaster?>> AuditMaster = new ResultModel<List<AuditMaster?>>();
            AuditMaster? audit = new AuditMaster();
            ResultModel<HODdetails> hod = new ResultModel<HODdetails>();
            HODdetails details = new HODdetails();

            try
            {
                result = _auditMasterService.UpdateHOD(master);
                if (result.Data != null)
                {
                    var currentUrl = HttpContext.Request.GetDisplayUrl();
                    string[] parts = new string[] { "", "" };
                    parts = currentUrl.TrimStart('/').Split('/');
                    string urlhttp = parts[0];
                    string HostUrl = parts[2];
                    AuditMaster = _auditMasterService.GetAll(new[] { "Id" }, new[] { master.Id.ToString() });
                    audit = AuditMaster.Data.FirstOrDefault();
                    string HODLink = urlhttp + "//" + HostUrl + "/Audit/Edit/" + result.Data.Id + "?edit=issueApprove";

                    if (audit != null)
                    {
                        hod = _auditMasterService.GetHODdetails();
                        details = hod.Data;
                        MailService.PendingAuditApprovalSend(details.Email, HODLink, audit.Name);
                    }
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> AuditStatusIndex()
        {
            #region BranchSetting
            var cacheKey = $"{User.Identity.Name}_BranchClaim";
            if (_memoryCache.TryGetValue(cacheKey, out BranchClaim branchClaim))
            {
                string name = branchClaim.BranchName;

                var identity = new ClaimsIdentity(User.Identity);
                identity.AddClaim(new Claim(ClaimNames.CurrentBranch, branchClaim.BranchId.ToString()));
                identity.AddClaim(new Claim(ClaimNames.CurrentBranchName, branchClaim.BranchName.ToString().Trim()));

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties() { IssuedUtc = DateTimeOffset.Now, });

            }
            #endregion 
            return View();
        }

        public IActionResult _auditStatusIndex()
        {
            try
            {
                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                index.UserName = userName;
                ApplicationUser user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                var search = Request.Form["search[value]"].FirstOrDefault();
                string users = Request.Form["BranchCode"].ToString();
                string branch = Request.Form["branchName"].ToString();
                string Address = Request.Form["Address"].ToString();
                string TelephoneNo = Request.Form["TelephoneNo"].ToString();
                string code = Request.Form["code"].ToString();
                string name = Request.Form["name"].ToString();
                string approvalStatus = Request.Form["approvalStatus"].ToString();

                if (approvalStatus == "Reject")
                {
                    index.AuditStatus = "Reject";
                }
                else
                {
                    index.AuditStatus = approvalStatus;
                }

                string post = Request.Form["ispost"].ToString();

                if (post == "Select")
                {
                    post = "";
                }
                string draw = Request.Form["draw"].ToString();
                var startRec = Request.Form["start"].FirstOrDefault();
                var pageSize = Request.Form["length"].FirstOrDefault();
                var orderName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][Name]"].FirstOrDefault();
                var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
                index.SearchValue = Request.Form["search[value]"].FirstOrDefault();
                index.OrderName = "Id";
                index.orderDir = "desc";
                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);
                index.createdBy = userName;
                string[] conditionalFields = null;
                string?[] conditionalValue = null;

                conditionalFields = new[]
                {
                            "Code like",
                            "Name like",
                            "bp.BranchName like"
                };

                conditionalValue = new[] { code, name, branch };

                ResultModel<List<AuditMaster>> indexData =
                    _auditMasterService.GetAuditStatusData(index, conditionalFields, conditionalValue);

                ResultModel<int> indexDataCount =
                 _auditMasterService.GetStatusDataCount(index, conditionalFields, conditionalValue);

                int result = _auditMasterService.GetCount(TableName.A_Audits, "Id", new[] { "A_Audits.createdBy", }, new[] { userName });

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<AuditMaster>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }
        }

        public async Task<IActionResult> ApproveStatusIndex()
        {
            #region GetBranchName
            var cacheKey = $"{User.Identity.Name}_BranchClaim";
            if (_memoryCache.TryGetValue(cacheKey, out BranchClaim branchClaim))
            {
                string name = branchClaim.BranchName;

                var identity = new ClaimsIdentity(User.Identity);
                identity.AddClaim(new Claim(ClaimNames.CurrentBranch, branchClaim.BranchId.ToString()));
                identity.AddClaim(new Claim(ClaimNames.CurrentBranchName, branchClaim.BranchName.ToString().Trim()));

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties() { IssuedUtc = DateTimeOffset.Now, });

            }
            #endregion 

            return View();
        }

        public IActionResult _approveStatusIndex(string status)
        {
            try
            {
                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                index.UserName = userName;
                ApplicationUser user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                var search = Request.Form["search[value]"].FirstOrDefault();
                string users = Request.Form["BranchCode"].ToString();
                string branch = Request.Form["BranchName"].ToString();
                string Address = Request.Form["Address"].ToString();
                string TelephoneNo = Request.Form["TelephoneNo"].ToString();
                string code = Request.Form["code"].ToString();
                string name = Request.Form["name"].ToString();
                string description = Request.Form["description"].ToString();
                string approveStatus = Request.Form["approveStatus"].ToString();
                string post = Request.Form["ispost"].ToString();

                if (post == "Select")
                {
                    post = "";

                }

                string draw = Request.Form["draw"].ToString();
                var startRec = Request.Form["start"].FirstOrDefault();
                var pageSize = Request.Form["length"].FirstOrDefault();
                var orderName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][Name]"].FirstOrDefault();
                var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
                index.SearchValue = Request.Form["search[value]"].FirstOrDefault();
                index.OrderName = "Id";
                index.orderDir = orderDir;
                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);
                index.createdBy = userName;
                string[] conditionalFields = null;
                string?[] conditionalValue = null;

                if (status == "self")
                {
                    conditionalFields = new[]
                    {
                             "Code like",
                            "Name like",
                            "ad.IsPost ",
                            "ad.createdBy ",
                     };

                    conditionalValue = new[] { code, name, "Y", index.createdBy };
                }
                else
                {
                    conditionalFields = new[]
                    {
                            "Code like",
                            "Name like",
                            "ad.IsPost "
                    };
                    conditionalValue = new[] { code, name, "Y" };
                }

                ResultModel<List<AuditMaster>> indexData =
                    _auditMasterService.GetAuditApproveIndexDataStatus(index, conditionalFields, conditionalValue);


                ResultModel<int> indexDataCount =
                 _auditMasterService.GetAuditApprovDataCount(index, conditionalFields, conditionalValue);

                int result = _auditMasterService.GetCount(TableName.A_Audits, "Id", new[] { "A_Audits.createdBy", }, new[] { userName });

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<AuditMaster>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }

        }

        public ActionResult MultipleAuditApproval(AuditMaster master)
        {
            ResultModel<AuditMaster> result = new ResultModel<AuditMaster>();
            ResultModel<List<AuditMaster?>> AuditMaster = new ResultModel<List<AuditMaster?>>();
            AuditMaster? audit = new AuditMaster();

            try
            {
                foreach (string ID in master.IDs)
                {
                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.PostedBy = user.UserName;
                    master.Audit.PostedOn = DateTime.Now;
                    master.Audit.PostedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    master.Operation = "approved";
                    master.Id = Convert.ToInt32(ID);
                    UserProfile urp = new UserProfile();
                    urp.Id = Convert.ToInt32(ID);
                    urp.UserName = userName;
                    MailSetting mailSetting = new MailSetting();
                    mailSetting.Id = Convert.ToInt32(ID);
                    ResultModel<List<UserProfile>> notifiEmai = null;

                    notifiEmai = _auditMasterService.GetEamil(urp);
                    mailSetting.Status = "Audit";
                    var email = notifiEmai.Data.FirstOrDefault();
                    var AuditUrl = _auditMasterService.GetUrl(mailSetting);
                    var url = AuditUrl.Data.FirstOrDefault();
                    AuditMaster = _auditMasterService.GetAll(new[] { "Id" }, new[] { ID });
                    audit = AuditMaster.Data.FirstOrDefault();

                    result = _auditMasterService.MultipleAuditApproval(master);

                    if (email != null)
                    {
                        AuditMail auditMail = new AuditMail();
                        auditMail.EmailAddress = email.Email;
                        auditMail.URL = url.ApprovedUrl;
                        auditMail.Name = audit.Name;
                        auditMail.Status = "ConfirmationOfAuditApproval";
                        var model = _mailService.SendEmail(auditMail);
                    }

                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }
            return Ok("");
        }

        public ActionResult ReportDataUpdate(AuditMaster master)
        {
            ResultModel<AuditMaster> result = new ResultModel<AuditMaster>();
            try
            {
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                master.Audit.PostedBy = user.UserName;
                master.Audit.PostedOn = DateTime.Now;
                master.Audit.PostedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                result = _auditMasterService.ReportDataUpdate(master);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }
            return Ok("");
        }

        public IActionResult SelfApproveStatusIndex()
        {
            return View();
        }

        public IActionResult _selfapproveStatusIndex()
        {
            try
            {
                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                index.UserName = userName;
                ApplicationUser user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                var search = Request.Form["search[value]"].FirstOrDefault();
                string users = Request.Form["BranchCode"].ToString();
                string branch = Request.Form["BranchName"].ToString();
                string Address = Request.Form["Address"].ToString();
                string TelephoneNo = Request.Form["TelephoneNo"].ToString();
                string code = Request.Form["code"].ToString();
                string name = Request.Form["name"].ToString();
                string description = Request.Form["description"].ToString();
                string approveStatus = Request.Form["approveStatus"].ToString();
                string post = Request.Form["ispost"].ToString();

                if (post == "Select")
                {
                    post = "";

                }

                string draw = Request.Form["draw"].ToString();
                var startRec = Request.Form["start"].FirstOrDefault();
                var pageSize = Request.Form["length"].FirstOrDefault();
                var orderName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][Name]"].FirstOrDefault();
                var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
                index.SearchValue = Request.Form["search[value]"].FirstOrDefault();
                index.OrderName = "Id";
                index.orderDir = orderDir;
                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);
                index.createdBy = userName;

                string[] conditionalFields = new[]
                {
                            "Code like",
                            "Name like",
                            "ad.IsPost "

                };
                string?[] conditionalValue = new[] { code, name, "Y" };

                ResultModel<List<AuditMaster>> indexData =
                    _auditMasterService.GetIndexDataSelfStatus(index, conditionalFields, conditionalValue);

                ResultModel<int> indexDataCount =
                _auditMasterService.GetIndexDataCount(index, conditionalFields, conditionalValue);

                int result = _auditMasterService.GetCount(TableName.A_Audits, "Id", new[] { "A_Audits.createdBy", }, new[] { userName });

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<AuditMaster>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }

        }
        public async Task<IActionResult> IssueApproveStatusIndex()
        {
            #region BranchSetting
            var cacheKey = $"{User.Identity.Name}_BranchClaim";
            if (_memoryCache.TryGetValue(cacheKey, out BranchClaim branchClaim))
            {
                string name = branchClaim.BranchName;

                var identity = new ClaimsIdentity(User.Identity);
                identity.AddClaim(new Claim(ClaimNames.CurrentBranch, branchClaim.BranchId.ToString()));
                identity.AddClaim(new Claim(ClaimNames.CurrentBranchName, branchClaim.BranchName.ToString().Trim()));

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties() { IssuedUtc = DateTimeOffset.Now, });
            }
            #endregion

            return View();
        }

        public IActionResult _issueApproveStatusIndex()
        {
            try
            {
                PeramModel pm = new PeramModel();
                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                index.UserName = userName;
                ApplicationUser user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                var search = Request.Form["search[value]"].FirstOrDefault();
                string users = Request.Form["BranchCode"].ToString();
                string branch = Request.Form["BranchName"].ToString();
                string Address = Request.Form["Address"].ToString();
                string TelephoneNo = Request.Form["TelephoneNo"].ToString();
                string code = Request.Form["code"].ToString();
                string name = Request.Form["name"].ToString();
                string description = Request.Form["description"].ToString();
                string approveStatus = Request.Form["approveStatus"].ToString();
                string post = Request.Form["ispost"].ToString();
                if (post == "Select")
                {
                    post = "";

                }
                string draw = Request.Form["draw"].ToString();
                var startRec = Request.Form["start"].FirstOrDefault();
                var pageSize = Request.Form["length"].FirstOrDefault();
                var orderName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][Name]"].FirstOrDefault();
                var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
                index.SearchValue = Request.Form["search[value]"].FirstOrDefault();
                index.OrderName = "Id";
                index.orderDir = orderDir;
                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);
                index.createdBy = userName;

                string[] conditionalFields = new[]
                {
                            "Code like",
                            "Name like",
                            "ad.IsPost "
                };

                string?[] conditionalValue = new[] { code, name, "Y" };

                ResultModel<List<AuditMaster>> indexData =
                    _auditMasterService.IssueGetIndexDataStatus(index, conditionalFields, conditionalValue, pm);

                ResultModel<int> indexDataCount =
                _auditMasterService.GetAuditIssueDataCount(index, conditionalFields, conditionalValue);

                int result = _auditMasterService.GetCount(TableName.A_Audits, "Id", new[] { "A_Audits.createdBy", }, new[] { userName });

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<AuditMaster>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }
        }
        public async Task<IActionResult> FeedBackApproveStatusIndex()
        {
            #region BranchSetting
            var cacheKey = $"{User.Identity.Name}_BranchClaim";
            if (_memoryCache.TryGetValue(cacheKey, out BranchClaim branchClaim))
            {
                string name = branchClaim.BranchName;
                var identity = new ClaimsIdentity(User.Identity);
                identity.AddClaim(new Claim(ClaimNames.CurrentBranch, branchClaim.BranchId.ToString()));
                identity.AddClaim(new Claim(ClaimNames.CurrentBranchName, branchClaim.BranchName.ToString().Trim()));
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties() { IssuedUtc = DateTimeOffset.Now, });

            }
            #endregion 

            return View();
        }

        public IActionResult _feedBackApproveStatusIndex()
        {
            PeramModel pm = new PeramModel();

            try
            {

                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                index.UserName = userName;
                ApplicationUser user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                var search = Request.Form["search[value]"].FirstOrDefault();
                string users = Request.Form["BranchCode"].ToString();
                string branch = Request.Form["BranchName"].ToString();
                string Address = Request.Form["Address"].ToString();
                string TelephoneNo = Request.Form["TelephoneNo"].ToString();
                string code = Request.Form["code"].ToString();
                string name = Request.Form["name"].ToString();
                string description = Request.Form["description"].ToString();
                string approveStatus = Request.Form["approveStatus"].ToString();
                string post = Request.Form["ispost"].ToString();

                if (post == "Select")
                {
                    post = "";

                }

                string draw = Request.Form["draw"].ToString();
                var startRec = Request.Form["start"].FirstOrDefault();
                var pageSize = Request.Form["length"].FirstOrDefault();
                var orderName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][Name]"].FirstOrDefault();
                var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
                index.SearchValue = Request.Form["search[value]"].FirstOrDefault();
                index.OrderName = "Id";
                index.orderDir = orderDir;
                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);
                index.createdBy = userName;

                string[] conditionalFields = new[]
                {
                            "Code like",
                            "Name like",
                            "ad.IsPost "
                };

                string?[] conditionalValue = new[] { code, name, "Y" };
                ResultModel<List<AuditMaster>> indexData =
                    _auditMasterService.FeedBackGetIndexDataStatus(index, conditionalFields, conditionalValue, pm);

                ResultModel<int> indexDataCount =
                _auditMasterService.GetAuditFeedBackDataCount(index, conditionalFields, conditionalValue);

                int result = _auditMasterService.GetCount(TableName.A_Audits, "Id", new[] { "A_Audits.createdBy", }, new[] { userName });

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<AuditMaster>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }
        }

        public ActionResult MultiplePost(AuditMaster master)
        {
            ResultModel<AuditMaster> result = new ResultModel<AuditMaster>();
            AuditMail auditMail = new AuditMail();
            ResultModel<List<AuditMaster?>> AuditMaster = new ResultModel<List<AuditMaster?>>();
            AuditMaster? audit = new AuditMaster();
            ResultModel<HODdetails> hod = new ResultModel<HODdetails>();
            HODdetails details = new HODdetails();

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

                    result = _auditMasterService.MultiplePost(master);

                    var currentUrl = HttpContext.Request.GetDisplayUrl();
                    string[] parts = new string[] { "", "" };
                    parts = currentUrl.TrimStart('/').Split('/');
                    string urlhttp = parts[0];
                    string HostUrl = parts[2];
                    AuditMaster = _auditMasterService.GetAll(new[] { "Id" }, new[] { master.Id.ToString() });
                    audit = AuditMaster.Data.FirstOrDefault();
                    string HODLink = urlhttp + "//" + HostUrl + "/Audit/ApproveStatusIndex";

                    if (audit != null)
                    {
                        hod = _auditMasterService.GetHODdetails();
                        details = hod.Data;
                        auditMail.Name = audit.Name;
                        auditMail.URL = HODLink;
                        auditMail.EmailAddress = details.Email;
                        auditMail.To = details.Email;
                        auditMail.Status = "PendingAuditApproval";
                        var model = _mailService.SendEmail(auditMail);
                    }

                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok("");
        }

        public ActionResult MultipleUnPost(AuditMaster master)
        {

            ResultModel<AuditMaster> result = new ResultModel<AuditMaster>();

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
                    result = _auditMasterService.MultipleUnPost(master);
                }

                return Ok(result);

            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok("");
        }

        public ActionResult MultipleIssueSave(AuditMaster master)
        {
            ResultModel<AuditFeedback> result = new ResultModel<AuditFeedback>();
            ResultModel<List<AuditMaster?>> AuditMaster = new ResultModel<List<AuditMaster?>>();
            ResultModel<List<AuditIssue>> auditIssues = new ResultModel<List<AuditIssue>>();
            AuditMaster? audit = new AuditMaster();
            ResultModel<List<AuditUser>> GetAuditTeamUsers = new ResultModel<List<AuditUser>>();

            try
            {
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);


                ResultModel<List<AuditMaster>> teamResult = _auditMasterService.GetAll(new[] { "Id" }, new[] { master.Id.ToString() });
                ResultModel<List<TeamMembers>> teamMembers = _teamMembersService.GetAll(new[] { "U.TeamId" }, new[] { teamResult.Data.FirstOrDefault().TeamId.ToString() });


                master.Audit.CreatedBy = user.UserName;
                master.Audit.CreatedOn = DateTime.Now;
                master.Audit.CreatedFrom = "";

                auditIssues = _auditIssueService.GetIssuesByAuditId(master);
                ResultModel<List<AuditFeedback>> auditFeedbacks = _auditFeedbackService.GetAuditFeedbackByAuditId(master);

                int issueCount = auditIssues.Data.Count();
                int feedbackCount = auditFeedbacks.Data.Count();
                int[] arrIssue = new int[issueCount];
                int[] arrFeedback = new int[feedbackCount];
                int countIssue = arrIssue.Count();
                int countFeedback = arrFeedback.Count();

                #region IssueSelectionSortAlgo
                for (int i = 0; i < issueCount; i++)
                {
                    arrIssue[i] = auditIssues.Data[i].Id;
                }
                for (int i = 0; i < issueCount - 1; i++)
                {
                    int minIndex = i;
                    for (int j = i + 1; j < issueCount; j++)
                    {
                        if (arrIssue[j] < arrIssue[minIndex])
                        {
                            minIndex = j;
                        }
                    }
                    int temp = arrIssue[minIndex];
                    arrIssue[minIndex] = arrIssue[i];
                    arrIssue[i] = temp;
                }
                #endregion 

                #region FeedbackSelectionSortAlgo

                for (int i = 0; i < feedbackCount; i++)
                {
                    arrFeedback[i] = auditFeedbacks.Data[i].AuditIssueId;
                }
                for (int i = 0; i < feedbackCount - 1; i++)
                {
                    int minIndex = i;
                    for (int j = i + 1; j < feedbackCount; j++)
                    {
                        if (arrFeedback[j] < arrFeedback[minIndex])
                        {
                            minIndex = j;
                        }
                    }
                    int temp = arrFeedback[minIndex];
                    arrFeedback[minIndex] = arrFeedback[i];
                    arrFeedback[i] = temp;
                }
                #endregion 

                var uniqueElements = arrIssue.Where(x => !arrFeedback.Contains(x)).ToArray();

                if (issueCount != feedbackCount)
                {
                    foreach (var Issueid in uniqueElements)
                    {
                        AuditFeedback feedback = new AuditFeedback();
                        feedback.AuditId = master.Id;
                        feedback.AuditIssueId = Issueid;
                        feedback.Heading = "Seems Okay";
                        feedback.Audit = master.Audit;
                        feedback.FeedbackDetails = Base64Encode("Seems Okay");

                        result = _auditFeedbackService.Insert(feedback);

                        AuditMaster = _auditMasterService.GetAll(new[] { "Id" }, new[] { feedback.AuditId.ToString() });
                        audit = AuditMaster.Data.FirstOrDefault();
                        GetAuditTeamUsers = _auditMasterService.GetAuditUserTeamId(audit.TeamId, null);

                        var teamCurrentUrl = HttpContext.Request.GetDisplayUrl();
                        string[] partstm = new string[] { "", "" };
                        partstm = teamCurrentUrl.TrimStart('/').Split('/');
                        string urlhttptm = partstm[0];
                        string HostUrltm = partstm[2];

                        string Url = urlhttptm + "//" + HostUrltm + "/Audit/Edit/" + feedback.AuditId + "?edit=feedback";

                        if (GetAuditTeamUsers.Data.Count() != 0)
                        {
                            foreach (var obj in GetAuditTeamUsers.Data)
                            {
                                if (obj.EmailAddress != null)
                                {
                                    MailService.SendFeedbackReviewedlMail(obj.EmailAddress, Url, audit.Name);

                                }
                            }
                        }


                        //notification


                        var currentUrl = HttpContext.Request.GetDisplayUrl();
                        string[] parts = new string[] { "", "" };
                        parts = currentUrl.TrimStart('/').Split('/');
                        string urlhttp = parts[0];
                        string HostUrl = parts[2];

                        string urlFeedback = urlhttp + "//" + HostUrl + "/Audit/Edit/" + result.Data.AuditId + "?edit=feedback&common=feedbackNotification " + result.Data.Id;
                        ResultModel<Notifications> resultNotification = _notificationService.InsertNotification(urlFeedback, feedback.Heading, result.Data.AuditId, result.Data.Id, user.UserName, "Feedback", teamMembers.Data);


                        //end

                    }
                }
                else
                {
                    return Ok(result);
                }

                return Ok(result);
            }

            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }
            return Ok();
        }

        public ActionResult IssueComplete(AuditMaster master)
        {
            ResultModel<AuditMaster> result = new ResultModel<AuditMaster>();
            try
            {
                result = _auditMasterService.IssueCompleted(master);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }
            return Ok("");
        }

        public ActionResult FeedbackComplete(AuditMaster master)
        {
            ResultModel<AuditMaster> result = new ResultModel<AuditMaster>();

            try
            {
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                master.Audit.LastUpdateBy = user.UserName;
                master.Audit.LastUpdateOn = DateTime.Now;
                master.Audit.LastUpdateFrom = "";

                result = _auditMasterService.FeedbackComplete(master);

                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok("");
        }

        public ActionResult BranchFeedbackComplete(AuditMaster master)
        {
            ResultModel<AuditMaster> result = new ResultModel<AuditMaster>();
            try
            {
                result = _auditMasterService.BranchFeedbackCompleted(master);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok("");
        }

        public ActionResult MultipleReject(AuditMaster master)
        {
            ResultModel<AuditMaster> result = new ResultModel<AuditMaster>();
            IssueRejectComments model = new IssueRejectComments();

            try
            {
                foreach (string ID in master.IDs)
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Approval.IsRejected = true;
                    master.Approval.RejectedBy = user.UserName;
                    master.Approval.RejectedDate = DateTime.Now;
                    master.Operation = "reject";

                    if (master.ApproveStatus == "issueApprove")
                    {
                        master.Operation = "IssueReject";
                    }
                    if (master.ApproveStatus == "branchFeedbackApprove")
                    {
                        master.Operation = "BranchFeedbackReject";
                    }

                    result = _auditMasterService.MultipleUnPost(master);
                }

                if (master.IssueIDs != null)
                {
                    foreach (var Id in master.IssueIDs)
                    {
                        if (Id != null)
                        {

                            model.AuditId = master.Id;
                            model.AuditIssueId = Id;
                            model.IsIssueReject = true;
                            model.IssuesRejectComments = master.RejectedComments;
                            model.Audit.CreatedBy = User.Identity.Name;
                            model.Audit.CreatedOn = DateTime.Now;

                            _auditIssueService.IssueRejectStatusUpdate(model);

                        }
                    }
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }
            return Ok("");
        }

        public ActionResult MultipleApproved(AuditMaster master)
        {
            ResultModel<AuditMaster> result = new ResultModel<AuditMaster>();
            ResultModel<List<AuditMaster?>> AuditMaster = new ResultModel<List<AuditMaster?>>();
            ResultModel<List<UserProfile>> notifiEmail = new ResultModel<List<UserProfile>>();
            AuditMaster? audit = new AuditMaster();
            ResultModel<List<AuditUser>> GetAuditTeamUsers = new ResultModel<List<AuditUser>>();
            AuditMail auditMail = new AuditMail();
            Notifications notifiaction = new Notifications();

            try
            {


                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                ResultModel<List<AuditMaster>> teamResult = _auditMasterService.GetAll(new[] { "Id" }, new[] { master.Id.ToString() });
                ResultModel<List<TeamMembers>> teamMembers = _teamMembersService.GetAll(new[] { "U.TeamId" }, new[] { teamResult.Data.FirstOrDefault().TeamId.ToString() });


                foreach (string ID in master.IDs)
                {
                    master.Audit.PostedBy = user.UserName;
                    master.Audit.PostedOn = DateTime.Now;
                    master.Audit.PostedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    master.Operation = "approved";
                    UserProfile urp = new UserProfile();
                    urp.Id = master.Id;
                    urp.UserName = userName;
                    MailSetting mailSetting = new MailSetting();
                    mailSetting.Id = master.Id;

                    if (master.ApproveStatus == "issueApprove")
                    {
                        notifiEmail = _auditMasterService.GetEamilForIssue(urp);
                        mailSetting.Status = "Issue";

                    }
                    else if (master.ApproveStatus == "auditStatus")
                    {
                        notifiEmail = _auditMasterService.GetEamil(urp);
                        mailSetting.Status = "Audit";
                    }
                    else if (master.ApproveStatus == "branchFeedbackApprove")
                    {
                        notifiEmail = _auditMasterService.GetEamilForBranchFeedback(urp);
                        mailSetting.Status = "BranchFeedback";
                    }

                    var email = notifiEmail.Data.FirstOrDefault();
                    var AuditUrl = _auditMasterService.GetUrl(mailSetting);
                    var url = AuditUrl.Data.FirstOrDefault();
                    AuditMaster = _auditMasterService.GetAll(new[] { "Id" }, new[] { master.Id.ToString() });
                    audit = AuditMaster.Data.FirstOrDefault();

                    result = _auditMasterService.MultipleUnPost(master);

                    GetAuditTeamUsers = _auditMasterService.GetAuditUserTeamId(master.TeamValue, null);

                    if (GetAuditTeamUsers.Data.Count() != 0)
                    {
                        foreach (var obj in GetAuditTeamUsers.Data)
                        {
                            if (obj.EmailAddress != null)
                            {

                                auditMail.EmailAddress = obj.EmailAddress;
                                auditMail.URL = url.ApprovedUrl;
                                auditMail.Name = audit.Name;
                                auditMail.Status = "ConfirmationOfAuditApproval";
                                var model = _mailService.SendEmail(auditMail);

                            }
                        }
                    }

                }

                #region Multiple Issue Approve
                if (master.IssueIDs != null)
                {
                    foreach (var Id in master.IssueIDs)
                    {
                        if (Id != null)
                        {
                            IssueRejectComments model = new IssueRejectComments();
                            AuditIssue Issuemodel = new AuditIssue();
                            model.AuditId = master.Id;
                            model.AuditIssueId = Id;
                            model.IsIssueApprove = true;
                            model.IssueApproveComments = "Approved By HOD";
                            model.Audit.CreatedBy = User.Identity.Name;
                            model.Audit.CreatedOn = DateTime.Now;
                            Issuemodel.Id = Convert.ToInt32(Id);
                            Issuemodel.Audit.CreatedBy = User.Identity.Name;
                            Issuemodel.Audit.CreatedOn = DateTime.Now;

                            _auditIssueService.IssueApproveStatusUpdate(Issuemodel);
                            _auditIssueService.IssueApprove(model);

                        }
                    }
                }
                #endregion

                #region Notification

                if (master.IssueIDs == null)
                {

                    var currentUrl = HttpContext.Request.GetDisplayUrl();
                    string[] parts = new string[] { "", "" };
                    parts = currentUrl.TrimStart('/').Split('/');
                    string urlhttp = parts[0];
                    string HostUrl = parts[2];

                    //notifiaction.IsAudit = true;
                    //notifiaction.Audit.CreatedBy = User.Identity.Name;
                    notifiaction.AuditId = result.Data.Id;
                    //notifiaction.Audit.CreatedOn = DateTime.Now;
                    //notifiaction.NotificationStatus = "AuditApprove";
                    //string AuditApprove = urlhttp + "//" + HostUrl + "/Audit?edit=AuditApproved&notification=AuditApproved&id=" + notifiaction.AuditId;
                    //notifiaction.AuditApproveUrl = AuditApprove;
                    //notifiaction.AuditName = master.Name;
                    //_notificationService.Insert(notifiaction);

                    string Url = urlhttp + "//" + HostUrl + "/Audit?edit=AuditApproved&notification=AuditApproved&id=" + notifiaction.AuditId;
                    ResultModel<Notifications> resultNotifi = _notificationService.InsertNotification(Url, master.Name, result.Data.Id, 0, user.UserName, "AuditApprove", teamMembers.Data);

                }

                #endregion

                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<IList<AuditMaster>> _index(string edit)
        {
            try
            {
                PeramModel pm = new PeramModel();
                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                string? search = Request.Form["search[value]"].FirstOrDefault();
                string draw = Request.Form["draw"].ToString();
                var startRec = Request.Form["start"].FirstOrDefault();
                var pageSize = Request.Form["length"].FirstOrDefault();
                var orderName = "Id";
                var orderDir = "desc";
                string code = Request.Form["code"].ToString();
                string name = Request.Form["name"].ToString();
                string auditStatus = Request.Form["auditStatus"].ToString();
                string startdate = Request.Form["startdate"].ToString();
                string enddate = Request.Form["enddate"].ToString();
                string ispost = Request.Form["ispost"].ToString();
                string auditPlan = Request.Form["auditPlan"].ToString();
                string branchName = Request.Form["branchName"].ToString();
                string auditType = Request.Form["auditType"].ToString();
                string auditApprove = Request.Form["auditApprove"].ToString();
                string issueComplete = Request.Form["issueComplete"].ToString();
                string feedback = Request.Form["feedback"].ToString();
                string branchFeedback = Request.Form["branchFeedback"].ToString();
                string fromdate = Request.Form["fromDate"].ToString();
                string todate = Request.Form["toDate"].ToString();

                if (ispost == "Select")
                {
                    ispost = "";
                }

                index.AuditPlan = auditPlan;
                index.AuditApprove = auditApprove;
                index.IssueComplete = issueComplete;
                index.Feedback = feedback;
                index.BranchFeedback = branchFeedback;
                index.SearchValue = Request.Form["search[value]"].FirstOrDefault();
                index.OrderName = orderName;
                index.orderDir = orderDir;
                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);
                index.createdBy = userName;
                index.CurrentBranchid = User.GetCurrentBranchId();
                index.Status = edit;
                index.UserName = userName;
                index.FromDate = fromdate;
                index.ToDate = todate;

                string[] conditionalFields = new[]
                {
                    "ad.[Code] like",
                    "ad.[Name] like",
                    "ad.[AuditStatus] like",
                    "FORMAT(ad.StartDate,'yyyy-MM-dd') like",
                    "FORMAT(ad.EndDate,'yyyy-MM-dd') like",
                    "ad.[IsPost] like",
                    "bp.[BranchName] like",
                    "At.[AuditType] like"
                };
                string?[] conditionalValue = new[] { code, name, auditStatus, startdate, enddate, ispost, branchName, auditType };

                ResultModel<List<AuditMaster>> indexData = _auditMasterService.GetIndexData(index, conditionalFields, conditionalValue, pm);

                #region Showing IssueTotal
                foreach (var audit in indexData.Data)
                {
                    int issues = _auditMasterService.GetTotoalIssuesById(audit.Id, "");
                    audit.TotalIssues = issues;

                }
                #endregion

                ResultModel<int> indexDataCount = _auditMasterService.GetIndexDataCount(index, conditionalFields, conditionalValue, pm);
                int result = _auditMasterService.GetCount(TableName.A_Audits, "Id", null, null);

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<AuditMaster>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }
        }

        [HttpPost]
        public ActionResult<IList<AuditAreas>> _indexArea(int id)
        {
            try
            {
                ResultModel<string> settingvalue = _AuditPointService.GetSettingsValue(new[] { "SettingGroup", "SettingName" }, new[] { "AuditPoint", "AuditMarkLevel" });


                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                string? search = Request.Form["search[value]"].FirstOrDefault();
                string draw = Request.Form["draw"].ToString();
                var startRec = Request.Form["start"].FirstOrDefault();
                var pageSize = Request.Form["length"].FirstOrDefault();
                var orderName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][Name]"].FirstOrDefault();
                var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
                index.SearchValue = Request.Form["search[value]"].FirstOrDefault();
                index.OrderName = "Id";
                index.orderDir = "desc";
                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);
                index.createdBy = userName;
                index.AuditId = id;
                index.AuditMarkLevel = settingvalue.Data;

                string[] conditionalFields = {
                    "AuditArea like"
                };
                string?[] conditionalValue = { search };

                ResultModel<List<AuditAreas>> indexData =
                    _auditAreasService.GetIndexData(index,
                        conditionalFields, conditionalValue);

                ResultModel<int> indexDataCount =
                    _auditAreasService.GetIndexDataCount(index,
                        conditionalFields, conditionalValue);

                int result = _auditMasterService.GetCount(TableName.A_AuditAreas, "Id", null, null);

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<AuditAreas>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }
        }



        [HttpPost]
        public ActionResult<IList<AuditAreas>> _GDICIndexArea(int id)
        {
            try
            {
                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                string? search = Request.Form["search[value]"].FirstOrDefault();
                string draw = Request.Form["draw"].ToString();
                var startRec = Request.Form["start"].FirstOrDefault();
                var pageSize = Request.Form["length"].FirstOrDefault();
                var orderName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][Name]"].FirstOrDefault();
                var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
                index.SearchValue = Request.Form["search[value]"].FirstOrDefault();
                index.OrderName = "Id";
                index.orderDir = "desc";
                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);
                index.createdBy = userName;
                index.AuditId = id;
                string[] conditionalFields = {
                    "AuditArea like"
                };
                string?[] conditionalValue = { search };

                ResultModel<List<AuditAreas>> indexData =
                    _auditAreasService.GDIC_GetIndexData(index,
                        conditionalFields, conditionalValue);

                ResultModel<int> indexDataCount =
                    _auditAreasService.GDIC_GetIndexDataCount(index,
                        conditionalFields, conditionalValue);

                int result = _auditMasterService.GetCount(TableName.A_AuditAreas, "Id", null, null);

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<AuditAreas>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }
        }



        [HttpPost]
        public ActionResult<IList<AuditUser>> _indexAuditUser(int id)
        {
            try
            {
                int i = 0;
                PeramModel pm = new PeramModel();
                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                string? search = Request.Form["search[value]"].FirstOrDefault();
                string draw = Request.Form["draw"].ToString();
                var startRec = Request.Form["start"].FirstOrDefault();
                var pageSize = Request.Form["length"].FirstOrDefault();
                var orderName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][Name]"].FirstOrDefault();
                var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
                index.SearchValue = Request.Form["search[value]"].FirstOrDefault();
                index.OrderName = "Id";
                index.orderDir = "desc";
                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);
                index.createdBy = userName;
                index.AuditId = id;
                ResultModel<List<AuditMaster?>> AuditData = _auditMasterService.GetAll(new[] { "Id" }, new[] { id.ToString() });
                var audit = AuditData.Data.FirstOrDefault();

                string[] conditionalFields = {
                    "EmailAddress like",
                    "usr.UserName like"
                };

                string?[] conditionalValue = { search, search };

                ResultModel<List<AuditUser>> indexData = _auditUserService.GetIndexData(index, conditionalFields, conditionalValue);
                ResultModel<List<AuditUser>> TeamUser = null;

                #region Setting Team Or Reviewer User
                if (id != 0)
                {
                    TeamUser = _auditMasterService.GetAuditUserTeamId(audit.TeamId, pm);
                }
                foreach (var auditUser in indexData.Data)
                {
                    if (auditUser.TeamId == 0.ToString())
                    {
                        indexData.Data[i].UserName = auditUser.UserName + "(Reviewer User)";
                    }
                    else
                    {
                        indexData.Data[i].UserName = auditUser.UserName + "(Team User)";
                    }
                    i++;
                }
                #endregion

                ResultModel<int> indexDataCount = _auditUserService.GetIndexDataCount(index, conditionalFields, conditionalValue);

                int result = _auditMasterService.GetCount(TableName.AuditUsers, "Id", null, null);

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<AuditUser>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }
        }


        [HttpPost]
        public ActionResult<IList<AuditIssueUser>> _indexAuditIssueUser(int id)
        {
            try
            {
                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                string? search = Request.Form["search[value]"].FirstOrDefault();
                string draw = Request.Form["draw"].ToString();
                var startRec = Request.Form["start"].FirstOrDefault();
                var pageSize = Request.Form["length"].FirstOrDefault();
                var orderName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][Name]"].FirstOrDefault();
                var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
                index.SearchValue = Request.Form["search[value]"].FirstOrDefault();
                index.OrderName = "Id";
                index.orderDir = "desc";
                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);
                index.createdBy = userName;
                index.AuditIssueId = id;

                string[] conditionalFields = {
                    "EmailAddress like"
                };
                string?[] conditionalValue = { search };


                ResultModel<List<AuditIssueUser>> indexData =
                    _auditIssueUserService.GetIndexData(index,
                        conditionalFields, conditionalValue);


                ResultModel<int> indexDataCount =
                    _auditAreasService.GetIndexDataCount(index,
                        conditionalFields, conditionalValue);

                int result = _auditMasterService.GetCount(TableName.AuditUsers, "Id", null, null);

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<AuditIssueUser>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }
        }

        [HttpPost]
        public ActionResult<IList<AuditReportUsers>> _indexAuditReportUser(int id)
        {
            try
            {
                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                string? search = Request.Form["search[value]"].FirstOrDefault();
                string draw = Request.Form["draw"].ToString();
                var startRec = Request.Form["start"].FirstOrDefault();
                var pageSize = Request.Form["length"].FirstOrDefault();
                var orderName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][Name]"].FirstOrDefault();
                var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
                index.SearchValue = Request.Form["search[value]"].FirstOrDefault();
                index.OrderName = "Id";
                index.orderDir = "desc";
                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);
                index.createdBy = userName;
                index.AuditId = id;

                string[] conditionalFields = {
                    "EmailAddress like"
                };

                string?[] conditionalValue = { search };

                ResultModel<List<AuditReportUsers>> indexData = _auditIssueUserService.AuditReportUsersGetIndexData(index, conditionalFields, conditionalValue);

                ResultModel<int> indexDataCount = _auditAreasService.GetIndexDataCount(index, conditionalFields, conditionalValue);
                int result = _auditMasterService.GetCount(TableName.AuditUsers, "Id", null, null);

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<AuditIssueUser>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }
        }

        public IActionResult AuditResponseIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult<IList<AuditResponse>> _indexAuditResponse()
        {
            try
            {
                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                string? search = Request.Form["search[value]"].FirstOrDefault();
                string draw = Request.Form["draw"].ToString();
                var startRec = Request.Form["start"].FirstOrDefault();
                var pageSize = Request.Form["length"].FirstOrDefault();
                var orderName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][Name]"].FirstOrDefault();
                var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();


                string code = Request.Form["code"].ToString();
                string name = Request.Form["name"].ToString();
                string startdate = Request.Form["startdate"].ToString();
                string enddate = Request.Form["enddate"].ToString();
                string ispost = Request.Form["ispost"].ToString();

                if (ispost == "Select")
                {
                    ispost = "";

                }

                index.SearchValue = Request.Form["search[value]"].FirstOrDefault();
                index.OrderName = "Code";
                index.orderDir = orderDir;
                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);

                index.createdBy = userName;
                index.CurrentBranchid = User.GetCurrentBranchId(); ;

                string[] conditionalFields = new[]
                {
                    "A.Name like",
                    "AI.IssueName like",
                    "AI.DateOfSubmission like",
                    "E.EnumValue like"
                };
                string?[] conditionalValue = new[] { search, search, search, search };

                ResultModel<List<AuditResponse>> indexData = _auditMasterService.AuditResponseGetIndexData(index, conditionalFields, conditionalValue);

                ResultModel<int> indexDataCount = _auditMasterService.AuditResponseGetIndexDataCount(index, conditionalFields, conditionalValue);

                int result = _auditMasterService.GetCount(TableName.A_AuditIssues, "Id", null, null);


                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<AuditMaster>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }
        }

        [HttpPost]
        public ActionResult SeeAllModal(AuditReportHeading auditSeeALl)
        {

            ModelState.Clear();
            string edit = auditSeeALl.Edit;
            AuditMaster master = new AuditMaster();
            AuditBranchFeedback branchFeedback = new AuditBranchFeedback();
            AuditFeedback auditFeedback = new AuditFeedback();
            AuditBranchFeedback branchFeedbackDepartment = new AuditBranchFeedback();
            AuditBranchFeedback auditResponseFollowUp = new AuditBranchFeedback();

            if (auditSeeALl.AuditId != 0)
            {
                ResultModel<List<AuditReportHeading?>> result =
                 _auditMasterService.GetReportHeadingById(new[] { "AuditId" }, new[] { auditSeeALl.AuditId.ToString() });

                ResultModel<List<AuditIssue?>> auditIssueRresult =
                   _auditMasterService.GetReportData(new[] { "ad.Id" }, new[] { auditSeeALl.AuditId.ToString() });

                master.auditIssue = auditIssueRresult.Data;

                AuditReportHeading seeAll = result.Data.FirstOrDefault();
                string htmlContent = $"";

                var headingPart = seeAll.AuditReportDetails;
                if (headingPart != null)
                {
                    string decodedHeading = Encoding.UTF8.GetString(Convert.FromBase64String(headingPart));
                    htmlContent = htmlContent + $"<p>{decodedHeading}</p>";
                }

                var secondPart = seeAll.AuditSecondReportDetails;
                if (secondPart != null)
                {
                    string decodedSecond = Encoding.UTF8.GetString(Convert.FromBase64String(secondPart));
                    htmlContent = htmlContent + $"<p>{decodedSecond}</p>";
                }

                if (auditIssueRresult.Data.Count() != 0)
                {
                    foreach (var issue in auditIssueRresult.Data)
                    {

                        ResultModel<List<AuditBranchFeedback?>> resultABF =
                              _auditMasterService.GetReportBranchFeedbackData(new[] { "ABF.AuditIssueId" }, new[] { issue.Id.ToString() });

                        branchFeedback = resultABF.Data.FirstOrDefault();
                        if (branchFeedback != null)
                        {
                            issue.AuditBranchFeedback = branchFeedback;
                        }

                    }
                }

                if (auditIssueRresult.Data.Count() != 0)
                {
                    foreach (var issue in auditIssueRresult.Data)
                    {
                        ResultModel<List<AuditFeedback?>> resultAF =
                              _auditMasterService.GetReportAuditFeedbackData(new[] { "AF.AuditIssueId" }, new[] { issue.Id.ToString() });

                        auditFeedback = resultAF.Data.FirstOrDefault();
                        if (auditFeedback != null)
                        {
                            issue.AuditFeedback = auditFeedback;
                        }

                    }
                }

                if (auditIssueRresult.Data.Count() != 0)
                {
                    foreach (var issue in auditIssueRresult.Data)
                    {

                        ResultModel<List<AuditBranchFeedback?>> DepartmentFollowUp =
                              _auditMasterService.GetBranchFeedbackDeprtemnetFollowUpData(new[] { "ABF.AuditIssueId" }, new[] { issue.Id.ToString() });

                        branchFeedbackDepartment = DepartmentFollowUp.Data.FirstOrDefault();

                        if (branchFeedbackDepartment != null)
                        {
                            issue.AuditBranchFeedbackDepartmentFollowUp = branchFeedbackDepartment;
                        }

                    }
                }

                if (auditIssueRresult.Data.Count() != 0)
                {
                    foreach (var issue in auditIssueRresult.Data)
                    {

                        ResultModel<List<AuditBranchFeedback?>> AuditResponseFollowUp =
                              _auditMasterService.GetBranchFeedbackAuditResponseFollowUpData(new[] { "ABF.AuditIssueId" }, new[] { issue.Id.ToString() });

                        auditResponseFollowUp = AuditResponseFollowUp.Data.FirstOrDefault();

                        if (auditResponseFollowUp != null)
                        {
                            issue.AuditBranchFeedbackAuditResponserFollwoUp = auditResponseFollowUp;
                        }
                    }
                }

                if (auditIssueRresult.Data.Count() != 0)
                {
                    int auditResponse = 0;
                    foreach (var issue in auditIssueRresult.Data)
                    {

                        if (issue.AuditBranchFeedbackAuditResponserFollwoUp == null)
                        {
                            issue.AuditBranchFeedbackAuditResponserFollwoUp = new AuditBranchFeedback();
                            issue.AuditBranchFeedbackAuditResponserFollwoUp.IssueDetails = "";
                            auditResponse++;
                        }
                        else
                        {
                            if (issue.AuditBranchFeedbackAuditResponserFollwoUp.IssueDetails != null)
                            {
                                //
                            }
                            else
                            {
                                issue.AuditBranchFeedbackAuditResponserFollwoUp.IssueDetails = "";
                            }
                            auditResponse++;
                        }

                    }
                }

                int i = 0; int j = 0;

                foreach (var issuePart in master.auditIssue)
                {
                    i++;
                    AuditIssue issue = auditIssueRresult.Data[j];

                    htmlContent = htmlContent + $"<p><b>{"Issue :" + i}</b></p>";
                    htmlContent = htmlContent + $"<p>{issuePart.IssueProcess}</p>";
                    htmlContent = htmlContent + $"<p>{issuePart.IssuePriority}</p>";
                    htmlContent = htmlContent + $"<p>{issuePart.IssueName}</p>";

                    if (issuePart.IssueDetails != null)
                    {
                        string decodedIssue = Encoding.UTF8.GetString(Convert.FromBase64String(issuePart.IssueDetails));
                        htmlContent = htmlContent + $"<p>{decodedIssue}</p>";

                    }

                    htmlContent = htmlContent + $"<p><b>{"Risk"}</b></p>";
                    htmlContent = htmlContent + $"<div>{issuePart.Risk}</div>";

                    if (issue.AuditBranchFeedback != null)
                    {
                        if (issue.AuditBranchFeedback.IssueDetails != null)
                        {
                            htmlContent = htmlContent + $"<p><b>{"Departmental Response : "}</b></p>";
                            htmlContent = htmlContent + $"<p><b><hr></b></p>";
                            string departmentResponse = Encoding.UTF8.GetString(Convert.FromBase64String(issue.AuditBranchFeedback.IssueDetails));
                            htmlContent = htmlContent + $"<p>{departmentResponse}</p>";
                            htmlContent = htmlContent + $"<p><b>{"Expected Implemention date:"}</b></p>";
                            htmlContent = htmlContent + $"<p>{issue.AuditBranchFeedback.ImplementationDate}</p>";
                        }
                    }
                    if (issue.AuditFeedback != null)
                    {
                        if (issue.AuditFeedback.IssueDetails != null)
                        {
                            htmlContent = htmlContent + $"<p><b>{"Audit Response: "}</b></p>";
                            string auditResponse = Encoding.UTF8.GetString(Convert.FromBase64String(issue.AuditFeedback.IssueDetails));
                            htmlContent = htmlContent + $"<p>{auditResponse}</p>";
                        }
                    }
                    if (issue.AuditBranchFeedbackDepartmentFollowUp != null)
                    {
                        if (issue.AuditBranchFeedbackDepartmentFollowUp.IssueDetails != null)
                        {
                            htmlContent = htmlContent + $"<p><b>{"Follow-up Response | Department:"}</b></p>";
                            htmlContent = htmlContent + $"<p><b><hr></b></p>";
                            string followUpDepartmentResponse = Encoding.UTF8.GetString(Convert.FromBase64String(issue.AuditBranchFeedbackDepartmentFollowUp.IssueDetails));
                            htmlContent = htmlContent + $"<p>{followUpDepartmentResponse}</p>";
                        }
                    }
                    if (issue.AuditBranchFeedbackAuditResponserFollwoUp != null)
                    {
                        if (issue.AuditBranchFeedbackAuditResponserFollwoUp.IssueDetails != null)
                        {
                            htmlContent = htmlContent + $"<p><b>{"Audit Response after follow-up:"}</b></p>";
                            string auditResponseAfterFollowUp = Encoding.UTF8.GetString(Convert.FromBase64String(issue.AuditBranchFeedbackAuditResponserFollwoUp.IssueDetails));
                            htmlContent = htmlContent + $"<p>{auditResponseAfterFollowUp}</p>";
                        }
                    }

                }
                var annexurePart = seeAll.AuditAnnexureDetails;
                if (annexurePart != null)
                {
                    string decodedAnnexure = Encoding.UTF8.GetString(Convert.FromBase64String(annexurePart));
                    htmlContent = htmlContent + $"<p>{decodedAnnexure}</p>";
                }

                auditSeeALl.AuditSeeAllDetails = htmlContent;

                if (result.Status == Status.Fail)
                {
                    throw result.Exception;
                }
            }

            auditSeeALl.Edit = edit;
            auditSeeALl.Operation = "update";
            return PartialView("_AuditSeeAllReport", auditSeeALl);

        }

        [HttpPost]
        public ActionResult AuditReportModal(AuditReportHeading auditReportHeading)
        {
            AuditReportHeading reportHeading = new AuditReportHeading();
            ResultModel<List<AuditReportHeading?>> result = new ResultModel<List<AuditReportHeading?>>();

            try
            {

                ModelState.Clear();
                string edit = auditReportHeading.Edit;

                if (auditReportHeading.AuditId != 0)
                {
                    result = _auditMasterService.GetReportHeadingById(new[] { "AuditId" }, new[] { auditReportHeading.AuditId.ToString() });

                    if (result.Status == Status.Fail)
                    {
                        throw result.Exception;
                    }

                    reportHeading = result.Data.FirstOrDefault();

                    if (reportHeading != null)
                    {
                        auditReportHeading = reportHeading;
                        auditReportHeading.Operation = "update";
                    }

                }
                auditReportHeading.Edit = edit;
                return PartialView("_AuditReport", auditReportHeading);
            }

            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
                return PartialView("_AuditReport", auditReportHeading);
            }

        }

        [HttpPost]
        public ActionResult AuditSecondReportModal(AuditReportHeading secondAuditReportHeading)
        {
            AuditReportHeading secondReportHeading = new AuditReportHeading();
            ResultModel<List<AuditReportHeading?>> result = new ResultModel<List<AuditReportHeading?>>();

            ModelState.Clear();
            string edit = secondAuditReportHeading.Edit;

            if (secondAuditReportHeading.AuditId != 0)
            {
                result = _auditMasterService.GetReportHeadingById(new[] { "AuditId" }, new[] { secondAuditReportHeading.AuditId.ToString() });

                if (result.Status == Status.Fail)
                {
                    throw result.Exception;
                }

                secondReportHeading = result.Data.FirstOrDefault();

                if (secondReportHeading != null)
                {
                    secondAuditReportHeading = secondReportHeading;
                    secondAuditReportHeading.Operation = "update";
                }

            }

            secondAuditReportHeading.Edit = edit;
            return PartialView("_AuditSecondReport", secondAuditReportHeading);

        }
        [HttpPost]
        public ActionResult AnnexureReportModal(AuditReportHeading annexureReport)
        {
            AuditReportHeading annexureReportData = new AuditReportHeading();
            ResultModel<List<AuditReportHeading?>> result = new ResultModel<List<AuditReportHeading?>>();

            ModelState.Clear();
            string edit = annexureReport.Edit;

            if (annexureReport.AuditId != 0)
            {
                result = _auditMasterService.GetReportHeadingById(new[] { "AuditId" }, new[] { annexureReport.AuditId.ToString() });

                if (result.Status == Status.Fail)
                {
                    throw result.Exception;
                }

                annexureReportData = result.Data.FirstOrDefault();

                if (annexureReportData != null)
                {
                    annexureReport = annexureReportData;
                    annexureReport.Operation = "update";
                }

            }
            annexureReport.Edit = edit;
            return PartialView("_AuditAnnexureReport", annexureReport);
        }

        [HttpPost]
        public ActionResult AuditReportUserModal(AuditReportUsers audirReportUser)
        {

            ModelState.Clear();
            if (audirReportUser.AuditId != 0)
            {
                //
            }

            return PartialView("_AuditReportUserEmail", audirReportUser);

        }

        [HttpPost]
        public ActionResult AuditReportInsertUserModal(AuditReportUsers auditReportUser)
        {
            ResultModel<List<AuditReportUsers?>> result = new ResultModel<List<AuditReportUsers?>>();

            ModelState.Clear();
            auditReportUser.Operation = "add";
            if (auditReportUser.Id != 0)
            {
                result = _auditIssueUserService.AuditReportUsersGetAll(new[] { "Id" }, new[] { auditReportUser.Id.ToString() });

                if (result.Status == Status.Fail)
                {
                    throw result.Exception;
                }

                auditReportUser = result.Data.FirstOrDefault();
                auditReportUser.Operation = "update";

            }

            return PartialView("_AuditReporteUser", auditReportUser);
        }

        [HttpPost]
        public ActionResult AuditReportInserUser(AuditReportUsers master)
        {

            ResultModel<AuditReportUsers> result = new ResultModel<AuditReportUsers>();
            ResultModel<List<AuditMaster?>> AuditMaster = new ResultModel<List<AuditMaster?>>();
            AuditMaster? audit = new AuditMaster();

            try
            {
                if (master.Operation == "update")
                {
                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = "";

                    result = _auditIssueUserService.AuditReportUsersUpdate(master);

                    if (result.Status == Status.Fail)
                    {
                        Exception ex = new Exception();
                        _logger.LogError(ex, "An error occurred in the Index action.");
                        throw result.Exception;
                    }

                    return Ok(result);
                }
                else
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = "";

                    result = _auditIssueUserService.AuditReportUsersInsert(master);

                    UserProfile urp = new UserProfile();
                    urp.Id = master.AuditId;
                    urp.UserName = user.UserName;
                    MailSetting mailSetting = new MailSetting();
                    mailSetting.Id = master.AuditId;

                    var notifiEmai = _auditMasterService.GetEamil(urp);

                    var email = notifiEmai.Data.FirstOrDefault();
                    AuditMaster = _auditMasterService.GetAll(new[] { "Id" }, new[] { master.AuditId.ToString() });
                    audit = AuditMaster.Data.FirstOrDefault();

                    var currentUrl = HttpContext.Request.GetDisplayUrl();
                    string[] parts = new string[] { "", "" };
                    parts = currentUrl.TrimStart('/').Split('/');
                    string urlhttp = parts[0];
                    string HostUrl = parts[2];

                    if (result.Status == Status.Fail)
                    {
                        Exception ex = new Exception();
                        _logger.LogError(ex, "An error occurred in the Index action.");
                        throw result.Exception;
                    }

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
        public ActionResult AuditReportEmailSend(AuditReportUsers master)
        {
            ResultModel<AuditReportUsers> result = new ResultModel<AuditReportUsers>();
            ResultModel<AuditMail> mailResult = new ResultModel<AuditMail>();
            AuditMail auditMail = new AuditMail();
            ResultModel<List<AuditMaster?>> AuditMaster = new ResultModel<List<AuditMaster?>>();
            AuditMaster? audit = new AuditMaster();

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

                string AuditPreviewUrl = urlhttp + "//" + HostUrl + "/Report/AuditeeResponse" + "?id=" + master.AuditReportUserList.FirstOrDefault().AuditId;
                AuditMaster = _auditMasterService.GetAll(new[] { "Id" }, new[] { master.AuditReportUserList.FirstOrDefault().AuditId.ToString() });
                audit = AuditMaster.Data.FirstOrDefault();

                foreach (var item in master.AuditReportUserList)
                {
                    auditMail.To = item.EmailAddress;
                    auditMail.URL = AuditPreviewUrl;
                    auditMail.Name = audit.Name;
                    auditMail.Status = "AuditReport";
                    mailResult = _mailService.SendEmail(auditMail);

                }

                return Ok(mailResult);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }
            return Ok(result);
        }

        [HttpPost]
        public ActionResult AuditAreaModal(AuditAreas auditAreas)
        {
            ResultModel<string> settingvalue = _AuditPointService.GetSettingsValue(new[] { "SettingGroup", "SettingName" }, new[] { "AuditPoint", "MaximumMark" });

            ModelState.Clear();
            string edit = auditAreas.Edit;

            if (auditAreas.Id != 0)
            {
                ResultModel<List<AuditAreas?>> result = _auditAreasService.GetAll(new[] { "AA.Id" }, new[] { auditAreas.Id.ToString() });

                if (result.Status == Status.Fail)
                {
                    throw result.Exception;
                }
                auditAreas = result.Data.FirstOrDefault();
                auditAreas.Operation = "update";

            }
            auditAreas.MaximumMark = settingvalue.Data;
            auditAreas.Edit = edit;
            return PartialView("_AreaDetail", auditAreas);
        }

        [HttpPost]
        public ActionResult GDICAuditAreaModal(AuditAreas auditAreas)
        {
            ModelState.Clear();
            string edit = auditAreas.Edit;

            if (auditAreas.Id != 0)
            {
                ResultModel<List<AuditAreas?>> result = _auditAreasService.GetAll(new[] { "AA.Id" }, new[] { auditAreas.Id.ToString() });

                if (result.Status == Status.Fail)
                {
                    throw result.Exception;
                }
                auditAreas = result.Data.FirstOrDefault();
                auditAreas.Operation = "update";

            }
            auditAreas.Edit = edit;
            return PartialView("_GDICAreaDetail", auditAreas);
        }

        [HttpPost]
        public ActionResult auditPointModal(AuditPoint auditAreas)
        {
            ModelState.Clear();
            string edit = auditAreas.Edit;

            if (auditAreas.Id != 0)
            {
                ResultModel<List<AuditPoint>> result = _AuditPointService.GetAll(new[] { "Id" }, new[] { auditAreas.Id.ToString() });

                AuditPoint AuditPoint = result.Data.FirstOrDefault();
                auditAreas.Operation = "update";
                AuditPoint.Id = AuditPoint.Id;
                //AuditPoint.PIdData = AuditPoint.PId;

                auditAreas.AuditType = AuditPoint.AuditType;

            }

            auditAreas.Operation = "add";
            auditAreas.Edit = edit;
            return PartialView("_AuditPointModal", auditAreas);

        }

        [HttpPost]
        public ActionResult AuditStatusModal(AuditMaster master)
        {
            ModelState.Clear();
            string edit = master.Edit;

            if (master.Id != 0)
            {
                ResultModel<List<AuditMaster>> result = _auditMasterService.GetAll(new[] { "Id" }, new[] { master.Id.ToString() });

                if (result.Status == Status.Fail)
                {
                    throw result.Exception;
                }

                master = result.Data.FirstOrDefault();
                master.Operation = "update";
                master.BranchIDStatus = master.BranchID;
            }

            master.Edit = edit;
            return PartialView("_AuditStatus", master);
        }

        [HttpPost]
        public ActionResult AuditIssueModal(AuditIssue AuditIssue)
        {
            ModelState.Clear();
            string edit = AuditIssue.Edit;

            if (AuditIssue.Id != 0)
            {
                ResultModel<List<AuditIssue?>> result = _auditIssueService.GetAll(new[] { "Id" }, new[] { AuditIssue.Id.ToString() });
                if (result.Status == Status.Fail)
                {
                    throw result.Exception;
                }

                AuditIssue = result.Data.FirstOrDefault();
                AuditIssue.AttachmentsList = _auditIssueAttachmentsService.GetAll(new[] { "AuditIssueId" }, new[] { AuditIssue.Id.ToString() }).Data;
                AuditIssue.Operation = "update";

            }

            AuditIssue.Edit = edit;
            return PartialView("_AuditIssue", AuditIssue);
        }

        [HttpPost]
        public ActionResult AuditIssueModalGDIC(AuditIssue AuditIssue)
        {
            ModelState.Clear();
            string edit = AuditIssue.Edit;

            if (AuditIssue.Id != 0)
            {
                ResultModel<List<AuditIssue?>> result = _auditIssueService.GetAll(new[] { "Id" }, new[] { AuditIssue.Id.ToString() });
                if (result.Status == Status.Fail)
                {
                    throw result.Exception;
                }

                AuditIssue = result.Data.FirstOrDefault();
                AuditIssue.AttachmentsList = _auditIssueAttachmentsService.GetAll(new[] { "AuditIssueId" }, new[] { AuditIssue.Id.ToString() }).Data;
                AuditIssue.Operation = "update";

            }

            AuditIssue.Edit = edit;
            return PartialView("_AuditIssueGDIC", AuditIssue);
        }


        [HttpPost]
        public ActionResult AuditReportStatusModal(AuditIssue master)
        {
            ModelState.Clear();
            string edit = master.Edit;

            if (master.Id != 0)
            {
                ResultModel<List<AuditIssue>> result = _auditIssueService.GetAll(new[] { "Id" }, new[] { master.Id.ToString() });

                if (result.Status == Status.Fail)
                {
                    throw result.Exception;
                }

                master = result.Data.FirstOrDefault();
                master.IssueDetailsUpdate = master.IssueDetails;
                master.IssuePriorityUpdate = master.IssuePriority;
                master.Operation = "update";
                master.ReportStatusModal = master.ReportStatus;
            }

            master.Edit = edit;
            return PartialView("_AuditReportStatus", master);
        }


        [HttpPost]
        public ActionResult AuditFeedbackModal(AuditFeedback auditFeedback)
        {
            ResultModel<List<AuditUser>> UserData = new ResultModel<List<AuditUser>>();
            PeramModel pm = new PeramModel();
            ModelState.Clear();
            string edit = auditFeedback.Edit;
            if (auditFeedback.Id != 0)
            {
                ResultModel<List<AuditFeedback?>> result = _auditFeedbackService.GetAll(new[] { "af.Id" }, new[] { auditFeedback.Id.ToString() });
                if (result.Status == Status.Fail)
                {
                    throw result.Exception;
                }
                auditFeedback = result.Data.FirstOrDefault();
                auditFeedback.FeedbackDetails = auditFeedback.IssueDetails;

                auditFeedback.AttachmentsList = _auditFeedbackAttachmentsService.GetAll(new[] { "AuditFeedbackId" }, new[] { auditFeedback.Id.ToString() }).Data;
                auditFeedback.Operation = "update";
            }
            auditFeedback.Edit = edit;


            #region CheckingTeamUser
            string userName = User.Identity.Name;
            UserData = _auditMasterService.GetAuditUserTeamId(auditFeedback.TeamId, pm);
            if (UserData.Data != null)
            {
                foreach (AuditUser audit in UserData.Data)
                {
                    if (userName == audit.UserName)
                    {
                        auditFeedback.IsTeam = true;
                    }
                }
            }
            if (userName == auditFeedback.CreatedBy)
            {
                auditFeedback.IsFeedback = true;
            }
            else
            {
                auditFeedback.IsFeedback = false;
            }
            #endregion

            return PartialView("_AuditIssueFeedback", auditFeedback);
        }

        [HttpPost]
        public ActionResult AuditIssuePreviewModal(AuditIssue AuditIssue)
        {

            ModelState.Clear();
            string edit = AuditIssue.Edit;
            if (AuditIssue.Id != 0)
            {

                ResultModel<AuditIssue> feedback = _auditFeedbackService.GetAuditLastFeedback(AuditIssue);
                AuditIssue = feedback.Data;
                AuditIssue.IssueDetails = AuditIssue.IssueDetails;
                AuditIssue.IssuePriority = AuditIssue.EnumValue;
                AuditIssue.FeedbackDetails = AuditIssue.IssueDetails;
                AuditIssue.AttachmentsList = _auditIssueAttachmentsService.GetAll(new[] { "AuditIssueId" }, new[] { AuditIssue.Id.ToString() }).Data;
                AuditIssue.DateOfSubmission = AuditIssue.DateOfSubmission;
                string dateTimeString = AuditIssue.DateOfSubmission;

                if (DateTime.TryParse(dateTimeString, out DateTime originalDateTime))
                {
                    DateTime dateOnly = originalDateTime.Date;
                    string dateOnlyString = dateOnly.ToString("yyyy-MM-dd");
                    AuditIssue.DateOfSubmission = dateOnlyString;
                }

                if (AuditIssue.IssueDetails != null)
                {
                    AuditIssue.IssueDetails = Base64Decode(AuditIssue.IssueDetails);
                }

                AuditIssue.Operation = "update";
            }

            AuditIssue.Edit = edit;
            return PartialView("_AuditIssuePreview", AuditIssue);

        }

        [HttpPost]
        public ActionResult AuditBranchFeedbackModal(AuditBranchFeedback AuditBranchFeedback)
        {

            PeramModel pm = new PeramModel();
            ModelState.Clear();
            string edit = AuditBranchFeedback.Edit;

            if (AuditBranchFeedback.Id != 0)
            {
                ResultModel<List<AuditBranchFeedback?>> result = _auditBranchFeedbackService.GetAll(new[] { "abf.Id" }, new[] { AuditBranchFeedback.Id.ToString() });

                if (result.Status == Status.Fail)
                {
                    throw result.Exception;
                }

                AuditBranchFeedback = result.Data.FirstOrDefault();
                AuditBranchFeedback.AuditBranchIssueId = AuditBranchFeedback.AuditIssueId;

                AuditBranchFeedback.AttachmentsList = _auditBranchFeedbackAttachmentsService.GetAll(new[] { "AuditBranchFeedbackId" }, new[] { AuditBranchFeedback.Id.ToString() }).Data;
                AuditBranchFeedback.Operation = "update";
            }
            AuditBranchFeedback.Edit = edit;

            #region CheckTeamUsers
            string userName = User.Identity.Name;
            ResultModel<List<AuditUser>> UserData = _auditMasterService.GetAuditUserTeamId(AuditBranchFeedback.TeamId, pm);
            if (UserData.Data != null)
            {
                foreach (AuditUser audit in UserData.Data)
                {
                    if (userName == audit.UserName)
                    {
                        AuditBranchFeedback.IsTeam = true;
                    }
                }
            }
            if (AuditBranchFeedback.ButtonStatus == "" || AuditBranchFeedback.ButtonStatus == null)
            {
                AuditBranchFeedback.ButtonStatus = "NotAllowToShow";
            }
            if (userName == AuditBranchFeedback.CreatedBy)
            {
                AuditBranchFeedback.ButtonStatus = "TeamFeedbackShow";
            }
            else
            {
                AuditBranchFeedback.ButtonStatus = "NotAllowToShow";
            }

            if (AuditBranchFeedback.Id == 0)
            {
                //AuditBranchFeedback.IssueStatus = "1045";
            }
            #endregion 

            return PartialView("_AuditIssueBranchFeedback", AuditBranchFeedback);

        }

        [HttpPost]
        public ActionResult AuditUserModal(AuditUser auditUser)
        {
            ModelState.Clear();
            string edit = auditUser.Edit;

            if (auditUser.Id != 0)
            {
                ResultModel<List<AuditUser?>> result = _auditUserService.GetAll(new[] { "Id" }, new[] { auditUser.Id.ToString() });

                if (result.Status == Status.Fail)
                {
                    throw result.Exception;
                }
                auditUser = result.Data.FirstOrDefault();
                auditUser.Operation = "update";
            }
            auditUser.Edit = edit;
            return PartialView("_AuditUser", auditUser);
        }

        [HttpPost]
        public ActionResult AuditIssueUserModal(AuditIssueUser auditIssueUser)
        {
            ModelState.Clear();
            string edit = auditIssueUser.Edit;

            if (auditIssueUser.Id != 0)
            {
                ResultModel<List<AuditIssueUser?>> result =
                    _auditIssueUserService.GetAll(new[] { "Id" }, new[] { auditIssueUser.Id.ToString() });

                if (result.Status == Status.Fail)
                {
                    throw result.Exception;
                }
                auditIssueUser = result.Data.FirstOrDefault();
                auditIssueUser.Operation = "update";
            }

            auditIssueUser.Edit = edit;
            return PartialView("_AuditIssueUser", auditIssueUser);
        }

        [HttpGet]
        public ActionResult GetUserInfo(string userId)
        {
            ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.Id == userId);
            return Ok(new { user.Email });
        }

        [HttpGet]
        public ActionResult GetIssueDeadLine(string issueId)
        {
            ResultModel<List<AuditIssue?>> result = _auditIssueService.GetAll(new[] { "Id" }, new[] { issueId.ToString() });
            if (result.Status == Status.Fail)
            {
                throw result.Exception;
            }

            AuditIssue? auditIssue = result.Data.FirstOrDefault();
            auditIssue.AuditBranchFeedback.Status = auditIssue.IssueStatus;
            DateTime dateTime = DateTime.Parse(auditIssue.ImplementationDate);
            auditIssue.ImplementationDate = dateTime.ToString("M/d/yyyy");
            return Ok(new { auditIssue.ImplementationDate, auditIssue.IssueDeadLine, auditIssue.AuditBranchFeedback.Status });
        }

        public IActionResult GenerateReport()
        {
            var templatePath = "E:\\Nurul Amin\\SSLAudit\\SageERP\\wwwroot\\GdicReport.docx";
            var copyPath = "E:\\Nurul Amin\\SSLAudit\\SageERP\\wwwroot\\GeneratedReport.docx";
            using (var templateStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
            {
                using (var doc = WordprocessingDocument.Open(templateStream, false))
                {
                    System.IO.File.Copy(templatePath, copyPath, true);
                    using (var copyStream = new FileStream(copyPath, FileMode.Open, FileAccess.ReadWrite))
                    {
                        using (var copyDoc = WordprocessingDocument.Open(copyStream, true))
                        {

                            var mainPart = copyDoc.MainDocumentPart;
                            var document = mainPart.Document;
                            mainPart.Document.Save();

                        }
                    }
                }
            }

            var generatedReport = System.IO.File.ReadAllBytes(copyPath);
            var fileName = "GeneratedReport.docx";
            return File(generatedReport, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);

        }
        private void FindAndReplace(WordprocessingDocument document, string placeholder, string replacement)
        {
            var body = document.MainDocumentPart.Document.Body;
        }
        private void FindAndReplace(Microsoft.Office.Interop.Word.Application wordApp, object toFindText, object replaceWithText)
        {
            object matchCase = true;

            object matchwholeWord = true;

            object matchwildCards = false;

            object matchSoundLike = false;

            object nmatchAllforms = false;

            object forward = true;

            object format = false;

            object matchKashida = false;

            object matchDiactitics = false;

            object matchAlefHamza = false;

            object matchControl = false;

            object read_only = false;

            object visible = true;

            object replace = -2;

            object wrap = 1;

            wordApp.Selection.Find.Execute(ref toFindText, ref matchCase,
                                            ref matchwholeWord, ref matchwildCards, ref matchSoundLike,

                                            ref nmatchAllforms, ref forward,

                                            ref wrap, ref format, ref replaceWithText,

                                                ref replace, ref matchKashida,

                                            ref matchDiactitics, ref matchAlefHamza,

                                             ref matchControl);
        }
        private void CreateWordDocument(object filename, object SaveAs)
        {
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            object missing = Missing.Value;
            Microsoft.Office.Interop.Word.Document myWordDoc = null;
            object readOnly = false;
            object isvisible = false;
            wordApp.Visible = false;
            myWordDoc = wordApp.Documents.Open(ref filename, ref missing, ref readOnly,
                                                ref missing, ref missing, ref missing,
                                                ref missing, ref missing, ref missing,
                                                ref missing, ref missing, ref missing,
                                                 ref missing, ref missing, ref missing, ref missing);
            myWordDoc.Activate();

            this.FindAndReplace(wordApp, "Example", "Anything");

            myWordDoc.SaveAs2(ref SaveAs, ref missing, ref missing, ref missing,
                                                            ref missing, ref missing, ref missing,
                                                            ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                                                            ref missing, ref missing, ref missing);

            this.FindAndReplace(wordApp, "<name>", "Nurul Amin");

            myWordDoc.Close();
            wordApp.Quit();

        }
        public ActionResult AuditExcel(string fromDate, string toDate, string branchId, string Id = "")
        {
            try
            {
                AuditMaster vms = new AuditMaster();

                string Branchid;
                string BranchName = "";
                string CompanyName = "";
                string CompanyAddress = "";

                if (branchId == "null")
                {
                    branchId = null;
                }
                if (string.IsNullOrEmpty(branchId))
                {
                    Branchid = User.GetCurrentBranchId();
                }
                else
                {
                    Branchid = branchId;
                }

                ReportModel vm = new ReportModel();
                vm.Id = Id;
                vm.ToDate = toDate;
                vm.FromDate = fromDate;
                vm.BranchId = Branchid;
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

                dt = _auditMasterService.DetailsInformation(vm);

                if (!dt.Data.Columns.Contains("SerialNumber"))
                {
                    dt.Data.Columns.Add("SerialNumber", typeof(int)).SetOrdinal(0);
                }
                for (int i = 0; i < dt.Data.Rows.Count; i++)
                {
                    dt.Data.Rows[i]["SerialNumber"] = i + 1;
                }

                if (!dt.Data.Columns.Contains("TotalIssues"))
                {
                    dt.Data.Columns.Add("TotalIssues", typeof(int));
                }
                foreach (DataRow audit in dt.Data.Rows)
                {
                    int idValue = Convert.ToInt32(audit["Id"].ToString());
                    int issues = _auditMasterService.GetTotoalIssuesById(idValue, "");
                    audit["TotalIssues"] = issues;
                }


                dt.Data.Columns["Id"].ColumnName = "ID No.";
                dt.Data.Columns["SerialNumber"].ColumnName = "SL No.";
                dt.Data.Columns["Code"].ColumnName = "Code";
                dt.Data.Columns["Name"].ColumnName = "Name";
                dt.Data.Columns["BranchName"].ColumnName = "Branch Name";
                dt.Data.Columns["AuditTypeId"].ColumnName = "Audit Type";
                dt.Data.Columns["AuditStatus"].ColumnName = "Audit Status";
                dt.Data.Columns["TotalIssues"].ColumnName = "Total Issues";
                dt.Data.Columns["IsPlaned"].ColumnName = "Audit Plan";
                dt.Data.Columns["StartDate"].ColumnName = "Start Date";
                dt.Data.Columns["EndDate"].ColumnName = "End Date";
                dt.Data.Columns["IsApprovedL4"].ColumnName = "Audit Approve";
                dt.Data.Columns["IsCompleteIssueTeamFeedback"].ColumnName = "Feedback Approve";
                dt.Data.Columns["IsCompleteIssueBranchFeedback"].ColumnName = "BranchFeedback Approve";
                dt.Data.Columns["ApproveStatus"].ColumnName = "Approve Status";
                dt.Data.Columns["IsPost"].ColumnName = "IsPost";

                #region Excel

                string filename = "Audit Index Report" + "-" + DateTime.Now.ToString("yyyy-MM-dd");
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add("Audit Index Report");

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
            string reportHeader = "Audit Index Report";
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

            workSheet.Cells[ReportHeaders.Length + 2, 2].Value = "From Date:";
            workSheet.Cells[ReportHeaders.Length + 2, 3].Value = fromDate;
            workSheet.Cells[ReportHeaders.Length + 2, 4].Value = "To Date:";
            workSheet.Cells[ReportHeaders.Length + 2, 5].Value = toDate;

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

        public static string Base64Encode(string plainText)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(bytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            byte[] bytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(bytes);
        }
        public static string Base64Decode1(string base64EncodedData)
        {
            byte[] bytes = Convert.FromBase64String(base64EncodedData);

            string decodedIssueDetails = Encoding.UTF8.GetString(bytes);
            return decodedIssueDetails;
        }

        public Notifications SetNotificationValueForUpdate(string id, string userName, string notificationType)
        {
            Notifications notifications = new Notifications();
            notifications.Id = Convert.ToInt32(id);
            notifications.Audit.LastUpdateOn = DateTime.Now;
            notifications.Audit.LastUpdateBy = userName;
            notifications.NotificationStatus = notificationType;

            return notifications;
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


