using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SageERP.ExtensionMethods;
using Shampan.Core.Interfaces.Repository.BKCheckListTypes;
using Shampan.Core.Interfaces.Repository.AuditPoints;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Services.AuditPoints;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using Shampan.Repository.SqlServer.BKCheckListTypes;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;
using Newtonsoft.Json;
using Shampan.Services.UserProfil;
using DocumentFormat.OpenXml.Office2010.Excel;
using Shampan.Models.AuditModule;
using Shampan.Core.Interfaces.Services.Audit;
using NuGet.Packaging.Core;
using System.Collections.Generic;

namespace SSLAudit.Controllers
{
    [ServiceFilter(typeof(UserMenuActionFilter))]
    [Authorize]
    public class AuditPointsController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IAdvancesService _advancesService;
        private readonly IBKCheckListTypeService _BKCheckListTypeService;
        private readonly IAuditPointService _AuditPointService;
        private readonly IAuditAreasService _auditAreasService;

        public AuditPointsController(ApplicationDbContext applicationDb, ITeamsService teamsService,
        IAdvancesService advancesService, IBKCheckListTypeService BKCheckListTypeService,
        IAuditPointService AuditPointService, IAuditAreasService auditAreasService

            )
        {
            _applicationDb = applicationDb;
            _advancesService = advancesService;
            _BKCheckListTypeService = BKCheckListTypeService;
            _AuditPointService = AuditPointService;
            _auditAreasService = auditAreasService;
        }

        public IActionResult Index()
        {

            ResultModel<string> settingvalue = _AuditPointService.GetSettingsValue(new[] { "SettingGroup", "SettingName" }, new[] { "AuditPoint", "AuditLevel" });
            AuditPoint master = new AuditPoint();
            master.PId = "xx";          
            return View(master);

        }
        public IActionResult _index()
        {
            try
            {
                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                ApplicationUser user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                var search = Request.Form["search[value]"].FirstOrDefault();
                string users = Request.Form["BranchCode"].ToString();
                string branch = Request.Form["BranchName"].ToString();
                string Address = Request.Form["Address"].ToString();
                string TelephoneNo = Request.Form["TelephoneNo"].ToString();

                string auditType = Request.Form["auditType"].ToString();
                string weightPersent = Request.Form["weightPersent"].ToString();
                string level = Request.Form["level"].ToString();

                string PId = Request.Form["PId"].ToString();
                string P_Level = Request.Form["P_Level"].ToString();


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
                index.PId = PId;
                index.P_Level = P_Level;

                index.createdBy = userName;

                string[] conditionalFields = new[]
                {

                            "AuditType like",
                            "WeightPersent like",
                            "P_Level like"

                };

                string?[] conditionalValue = new[] { auditType, weightPersent, level };

                ResultModel<List<AuditPoint>> indexData =
                    _AuditPointService.GetIndexData(index, conditionalFields, conditionalValue);

                ResultModel<int> indexDataCount =
                _AuditPointService.GetIndexDataCount(index, conditionalFields, conditionalValue);

                int result = _AuditPointService.GetCount(TableName.AuditPoints, "Id", new[] { "AuditPoints.createdBy", }, new[] { userName });

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<AuditPoint>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }

        }

        public ActionResult Create()
        {

            ModelState.Clear();
            AuditPoint vm = new AuditPoint();
            vm.Operation = "add";
            return View("CreateEdit", vm);

        }

        public ActionResult CreateEdit(AuditPoint master)
        {
            ResultModel<AuditPoint> result = new ResultModel<AuditPoint>();

            try
            {
                if (master.Operation == "update")
                {
                    master.P_Level = Convert.ToInt32(master.P_Level) + 1;
                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _AuditPointService.Update(master);

                    return Ok(result);
                }
                else
                {

                    string userName = User.Identity.Name;
                    master.P_Level = Convert.ToInt32(master.P_Level) + 1;

                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    result = _AuditPointService.Insert(master);

                    return Ok(result);
                }


            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);
        }

        public ActionResult Edit(int id)
        {
            try
            {
                ResultModel<List<AuditPoint>> result =
                    _AuditPointService.GetAll(new[] { "Id" }, new[] { id.ToString() });

                AuditPoint AuditPoint = result.Data.FirstOrDefault();
                AuditPoint.Operation = "update";
                AuditPoint.Id = id;

                return View("CreateEdit", AuditPoint);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public ActionResult GetAuditPointLevel([FromBody] AuditPoint param)
        {
            ResultModel<List<AuditPoint>> result = new ResultModel<List<AuditPoint>>();

            if (param != null && param.PId != "xx")
            {

                ResultModel<List<AuditPoint>> resultData = _AuditPointService.GetAll(new[] { "AuditType" }, new[] { param.AuditType.ToString() });
                if (resultData != null)
                {
                    param.Id = resultData.Data.FirstOrDefault().Id;
                }

                List<AuditPoint> AuditPoint = new List<AuditPoint>();
                result = _AuditPointService.GetAll(new[] { "Id" }, new[] { param.Id.ToString() });

                ResultModel<List<AuditPoint>> resultAuditTypeId = _AuditPointService.GetAll(new[] { "Id" }, new[] { param.PId.ToString() });
                if (resultAuditTypeId.Data.Count != 0)
                {
                    result.Data.FirstOrDefault().AuditTypeId = resultAuditTypeId.Data.FirstOrDefault().AuditTypeId;
                }

            }


            return Ok(result.Data);
        }

        [HttpPost]
        public ActionResult AuditAreaModal(AuditAreas auditAreas)
        {
            ModelState.Clear();
            string edit = auditAreas.Edit;

            if (auditAreas.Id != 0)
            {
                ResultModel<List<AuditAreas?>> result = _auditAreasService.GetAll(new[] { "Id" }, new[] { auditAreas.Id.ToString() });

                if (result.Status == Status.Fail)
                {
                    throw result.Exception;
                }
                auditAreas = result.Data.FirstOrDefault();
                auditAreas.Operation = "update";

            }
            auditAreas.Edit = edit;
            return PartialView("_AreaDetailDemo", auditAreas);
        }

        public ActionResult MultiplePost(Advances master)
        {
            ResultModel<Advances> result = new ResultModel<Advances>();

            try
            {
                foreach (string ID in master.IDs)
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.PostedBy = user.UserName;
                    master.Audit.PostedOn = DateTime.Now;
                    master.Audit.PostedFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _advancesService.MultiplePost(master);

                }

                return Ok(result);

            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok("");
        }

        public ActionResult MultipleUnPost(Advances master)
        {
            ResultModel<Advances> result = new ResultModel<Advances>();

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


                    result = _advancesService.MultipleUnPost(master);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok("");
        }


        public ActionResult MultipleReject(Advances master)
        {
            ResultModel<Advances> result = new ResultModel<Advances>();

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

                    result = _advancesService.MultipleUnPost(master);

                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok("");
        }


        public ActionResult MultipleApproved(Advances master)
        {
            ResultModel<Advances> result = new ResultModel<Advances>();

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

                    result = _advancesService.MultipleUnPost(master);
                }


                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok("");
        }


    }
}
