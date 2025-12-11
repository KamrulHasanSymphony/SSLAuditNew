using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Shampan.Core.Interfaces.Repository.AuditFeedbackRepo;
using Shampan.Core.Interfaces.Services.Audit;
using Shampan.Core.Interfaces.Services.AuditFeedbackService;
using Shampan.Core.Interfaces.Services.AuditIssues;
using Shampan.Core.Interfaces.Services.Mail;
using Shampan.Core.Interfaces.Services.Notification;
using Shampan.Core.Interfaces.Services.TeamMember;
using Shampan.Models;
using Shampan.Models.AuditModule;
using Shampan.Services;
using Shampan.Services.AuditFeedbackService;
using Shampan.Services.AuditIssues;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;
using System.Data.SqlClient;
using System.Security.Policy;

namespace SSLAudit.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]

	public class AuditFeedbackController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IAuditFeedbackService _auditFeedbackService;
        private readonly IAuditFeedbackAttachmentsService _auditFeedbackAttachmentsService;
        private readonly IConfiguration _configuration;
        private readonly INotificationService _notificationService;
        private readonly IAuditMasterService _auditMasterService;
        private readonly IMailService _mailService;
        private readonly ITeamMembersService _teamMembersService;



        public AuditFeedbackController(ApplicationDbContext applicationDb, 
            IAuditFeedbackService auditFeedbackService,
            IAuditFeedbackAttachmentsService auditFeedbackAttachmentsService,
             IConfiguration configuration,
            INotificationService notificationService,
            IAuditMasterService auditMasterService,
            IMailService mailService,
            ITeamMembersService teamMembersService
        )
        {

            _applicationDb = applicationDb;
            _auditFeedbackService = auditFeedbackService;
            _auditFeedbackAttachmentsService = auditFeedbackAttachmentsService;
            _configuration = configuration;
            _notificationService = notificationService;
            _auditMasterService = auditMasterService;
            _mailService = mailService;
            _teamMembersService = teamMembersService;

        }

        public IActionResult Index(int? id)
        {
            if (id is null || id == 0)
            {
                return RedirectToAction("Index", "Audit");
            }

            AuditFeedback auditIssue = new AuditFeedback()
            {
                AuditId = id.Value
            };

            return View(auditIssue);
        }

        public IActionResult Create(int? id)
        {

            if (id is null || id == 0)
            {
                return RedirectToAction("Index", "Audit");
            }

            AuditFeedback auditIssue = new AuditFeedback()
            {
                Operation = "add",
                AuditId = id.Value
            };
            return View(auditIssue);
        }


        [HttpPost]
        public ActionResult CreateEdit(AuditFeedback master)
        {
            ResultModel<AuditFeedback> result = new ResultModel<AuditFeedback>();
            ResultModel<List<AuditMaster?>> AuditMaster = new ResultModel<List<AuditMaster?>>();
            AuditMaster? audit = new AuditMaster();
            ResultModel<List<AuditUser>> GetAuditTeamUsers = new ResultModel<List<AuditUser>>();
            Notifications notifiaction = new Notifications();
            AuditMail email = new AuditMail();


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

                    result = _auditFeedbackService.Update(master);

                    return Ok(result);
                }

             

                master.Audit.CreatedBy = user.UserName;
                master.Audit.CreatedOn = DateTime.Now;
                master.Audit.CreatedFrom = "";
                master.Audit.LastUpdateBy = user.UserName;
                master.Audit.LastUpdateOn = DateTime.Now;
                master.Audit.LastUpdateFrom = "";

                result = _auditFeedbackService.Insert(master);

                #region MailSend
                AuditMaster = _auditMasterService.GetAll(new[] { "Id" }, new[] { master.AuditId.ToString() });
                audit = AuditMaster.Data.FirstOrDefault();
                GetAuditTeamUsers = _auditMasterService.GetAuditUserTeamId(audit.TeamId, null);
                var teamCurrentUrl = HttpContext.Request.GetDisplayUrl();
                string[] partstm = new string[] { "", "" };
                partstm = teamCurrentUrl.TrimStart('/').Split('/');
                string urlhttptm = partstm[0];
                string HostUrltm = partstm[2];
                string Url = urlhttptm + "//" + HostUrltm + "/Audit/Edit/" + master.AuditId + "?edit=feedback";

                if (GetAuditTeamUsers.Data.Count() != 0)
                {
                    foreach (var obj in GetAuditTeamUsers.Data)
                    {
                        if (obj.EmailAddress != null)
                        {
                            email.To = obj.EmailAddress;
                            email.URL = Url;
                            email.Name = audit.Name;
                            email.Status = "SendFeedbackReviewedlMail";
                            //MailService.SendFeedbackReviewedlMail(obj.EmailAddress, Url, audit.Name);
                            _mailService.SendEmail(email);

                        }
                    }
                }
                #endregion

                #region Notification
                var currentUrl = HttpContext.Request.GetDisplayUrl();
                string[] parts = new string[] { "", "" };
                parts = currentUrl.TrimStart('/').Split('/');
                string urlhttp = parts[0];
                string HostUrl = parts[2];
         
                //string url = urlhttp + "//" + HostUrl + "/Audit/Edit/" + result.Data.AuditId + "?edit=feedback&common=feedbackNotification "+ result.Data.Id;
                //notifiaction = SetNotificationValueForInsert(url, master.Heading, result.Data.AuditId, result.Data.Id, user.UserName, "Feedback");
                //_notificationService.Insert(notifiaction);

                string urlFeedback = urlhttp + "//" + HostUrl + "/Audit/Edit/" + result.Data.AuditId + "?edit=feedback&common=feedbackNotification " + result.Data.Id;
                ResultModel<Notifications> resultNotifi = _notificationService.InsertNotification(urlFeedback, master.Heading, result.Data.AuditId, result.Data.Id, user.UserName, "Feedback", teamMembers.Data);


                #endregion

                return Ok(result);

            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);
        }

       

        public ActionResult<IList<AuditFeedback>> Edit(int id)
        {
            try
            {
                ResultModel<List<AuditFeedback?>> result =
                    _auditFeedbackService.GetAll(new[] { "Id" }, new[] { id.ToString() });

                if (result.Status == Status.Fail)
                {
                    throw result.Exception;
                }

                AuditFeedback? auditMaster = result.Data.FirstOrDefault();

                auditMaster.AttachmentsList = _auditFeedbackAttachmentsService
                    .GetAll(new[] {"AuditFeedbackId"}, new[] {id.ToString()}).Data;

                auditMaster.Operation = "update";

                return View("Create", auditMaster);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public ActionResult<IList<AuditFeedback>> _index(int? id)
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
                index.AuditId = id.Value;


                string[] conditionalFields = new[]
                {
                    "ai.IssueName like",
                    "af.Heading like"
                };
                string?[] conditionalValue = new[] { search, search };


                ResultModel<List<AuditFeedback>> indexData =
                    _auditFeedbackService.GetIndexData(index,
                conditionalFields, conditionalValue);


                ResultModel<int> indexDataCount =
                    _auditFeedbackService.GetIndexDataCount(index,
                conditionalFields, conditionalValue);

                int result = _auditFeedbackService.GetCount(TableName.A_AuditFeedbacks, "Id", null,null);
             
                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<AuditIssue>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }
        }

        [HttpPost]
        public ActionResult<IList<AuditFeedback>> _indexBranchFeedback(int? id)
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
				index.OrderName = orderName;
				index.orderDir = orderDir;
				index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);
                index.createdBy = userName;
                index.AuditId = id.Value;

                string[] conditionalFields = new[]
                {
                    "ai.IssueName like",
                    "ad.Name like"
                };
                string?[] conditionalValue = new[] { search, search };


                ResultModel<List<AuditFeedback>> indexData =
                    _auditFeedbackService.GetIndexData(index,
                conditionalFields, conditionalValue);


                ResultModel<int> indexDataCount =
                    _auditFeedbackService.GetIndexDataCount(index,
                conditionalFields, conditionalValue);

                int result = _auditFeedbackService.GetCount(TableName.A_Audits, "Id", null, null);
                
                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<AuditIssue>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }
        }



        public ActionResult MultiplePost(AuditFeedback master)
        {
            ResultModel<AuditFeedback> result = new ResultModel<AuditFeedback>();

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
                    result = _auditFeedbackService.MultiplePost(master);

                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }
            return Ok("");
        }
        public ActionResult MultipleUnPost(AuditFeedback master)
        {
            ResultModel<AuditFeedback> result = new ResultModel<AuditFeedback>();

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
                    result = _auditFeedbackService.MultipleUnPost(master);
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

            ResultModel<AuditFeedbackAttachments> result = new ResultModel<AuditFeedbackAttachments>
            {
                Message = "File could not be deleted"
            };

            try
            {
                var path = Path.Combine(saveDirectory, filePath);
                if (!System.IO.File.Exists(path)) return Ok(result);

                result = _auditFeedbackAttachmentsService.Delete(Convert.ToInt32(id.Replace("file-", "")));

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
