using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using SageERP.ExtensionMethods;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.BKAuditTemlateMasters;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using Shampan.Services.BKAuditTemlateMasters;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;

namespace SSLAudit.Controllers
{
    [ServiceFilter(typeof(UserMenuActionFilter))]
    [Authorize]
    public class BKAuditTemlateMasterController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IAdvancesService _advancesService;
        private readonly IBKAuditTemlateMasterService _BKAuditTemlateMasterService;

        public BKAuditTemlateMasterController(ApplicationDbContext applicationDb, ITeamsService teamsService,
            IAdvancesService advancesService, IBKAuditTemlateMasterService BKAuditTemlateMasterService)
        {
            _applicationDb = applicationDb;
            _advancesService = advancesService;
            _BKAuditTemlateMasterService = BKAuditTemlateMasterService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {

            ModelState.Clear();
            BKAuditTemlateMaster vm = new BKAuditTemlateMaster();
            vm.Operation = "add";
            return View("CreateEdit", vm);


        }

        public ActionResult CreateEdit(BKAuditTemlateMaster master)
        {
            ResultModel<BKAuditTemlateMaster> result = new ResultModel<BKAuditTemlateMaster>();
            try
            {

                if (master.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _BKAuditTemlateMasterService.Update(master);

                    return Ok(result);
                }
                else
                {

                    string userName = User.Identity.Name;

                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    result = _BKAuditTemlateMasterService.Insert(master);

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
        //public IActionResult GetAllMappingData(int bKAuditOfficeTypeId)
        public IActionResult GetAllMappingData(int bKAuditOfficeTypeId, int id)
        {

            if (bKAuditOfficeTypeId <= 0)
            {
                return Json(new { success = false, message = "Invalid BKAuditOfficeType" });
            }

            IndexModel index = new IndexModel();
            string userName = User.Identity.Name;
            ApplicationUser user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
            index.OrderName = "Id";
            index.createdBy = userName;

            string[] conditionalFields = new[]
                {
                            "AOT.Id"

            };

            string?[] conditionalValue = new[] { bKAuditOfficeTypeId.ToString() };

            ResultModel<List<MappingData>> indexData =
                    _BKAuditTemlateMasterService.GetIndexMappingData(index, conditionalFields, conditionalValue);

            ResultModel<int> indexDataCount = _BKAuditTemlateMasterService.GetIndexMappingDataCount(index, conditionalFields, conditionalValue);
            int result = _BKAuditTemlateMasterService.GetMappingCount(TableName.BKAuditTemlateMaster, "Id", new[] { "BKAuditTemlateMaster.createdBy", }, new[] { userName });

            if (indexData.Data != null)
            {
                for (int i = 0; i < indexData.Data.Count(); i++)
                {
                    int subTypeId = indexData.Data[i].BKCheckListSubTypeId;
                    if (subTypeId != 0 && subTypeId != null)
                    {
                        ResultModel<List<CheckListItem>> indexDataItem =
                            _BKAuditTemlateMasterService.GetIndexCheckListItemData(index, new[] { "BKCheckListSubTypesId" }, new[] { subTypeId.ToString() });

                        indexData.Data[i].CheckListItemList = indexDataItem.Data;


                        //Add

                        if (id > 0)
                        {
                            ResultModel<List<BKAuditTemplateDetails>> details = _BKAuditTemlateMasterService.GetDetailsAll(new[] { "ATD.BKAuditTemlateMasterId", "ATD.BKCheckListSubTypeId" }, new[] { id.ToString(), subTypeId.ToString() });
                            int data = details.Data.Count()-1;

                            if (indexData.Data[i].CheckListItemList.Count() != 0)
                            {
                                for (int j = 0; j < indexData.Data[i].CheckListItemList.Count(); j++)
                                {
                                    var checkListItem = indexData.Data[i].CheckListItemList[j];
                                    // Find matching detail by BKCheckListItemId
                                    var matchingDetail = details.Data.FirstOrDefault(d => d.BKCheckListItemId == checkListItem.Id);

                                    if (matchingDetail != null)
                                    {
                                        // Set Status based on matching detail
                                        checkListItem.Status = matchingDetail.Status;
                                        // Set IsFieldType based on matching detail
                                        checkListItem.IsFieldType = matchingDetail.IsFieldType;
                                    }
                                    else
                                    {
                                        // If no matching detail found, set defaults
                                        checkListItem.Status = false;
                                        checkListItem.IsFieldType = false;
                                    }

                                    //if(data >= 0)
                                    //{
                                    //    if (details.Data[j].Status == true && indexData.Data[i].CheckListItemList[j].Id == details.Data[j].BKCheckListItemId)
                                    //    {
                                    //        indexData.Data[i].CheckListItemList[j].Status = true;
                                    //    }
                                    //    else
                                    //    {
                                    //        indexData.Data[i].CheckListItemList[j].Status = false;
                                    //    }
                                    //    if (details.Data[j].IsFieldType == true)
                                    //    {
                                    //        indexData.Data[i].CheckListItemList[j].IsFieldType = true;
                                    //    }
                                    //    else
                                    //    {
                                    //        indexData.Data[i].CheckListItemList[j].IsFieldType = false;
                                    //    }
                                    //}
                                    //data--;


                                }
                            }
                        }

                        //end


                    }
                }
            }


            // Filter data to include only those with at least one CheckListItem
            indexData.Data = indexData.Data.Where(d => d.CheckListItemList != null && d.CheckListItemList.Any()).ToList();


            return Ok(new { data = indexData.Data, recordsTotal = result, recordsFiltered = indexDataCount.Data, length = indexData.Data.Count });

        }

        public ActionResult Edit(int id)
        {
            try
            {
                ResultModel<List<BKAuditTemlateMaster>> result =
                    _BKAuditTemlateMasterService.GetAll(new[] { "Id" }, new[] { id.ToString() });

                BKAuditTemlateMaster BKAuditTemlateMaster = result.Data.FirstOrDefault();
                BKAuditTemlateMaster.Operation = "update";
                BKAuditTemlateMaster.Id = id;

                //ResultModel<List<BKAuditTemplateDetails>> details = _BKAuditTemlateMasterService.GetDetailsAll(new[] { "ATD.BKAuditTemlateMasterId" }, new[] { id.ToString() });
                //BKAuditTemlateMaster.BKAuditTemplateDetailsList = details.Data;

                return View("CreateEdit", BKAuditTemlateMaster);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
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
                string code = Request.Form["code"].ToString();
                string advanceAmount = Request.Form["advanceAmount"].ToString();
                string description = Request.Form["description"].ToString();
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
                            "AdvanceAmount like",
                            "Description like",
                            "IsPost like"
                };

                string?[] conditionalValue = new[] { code, advanceAmount, description, post };

                ResultModel<List<BKAuditTemlateMaster>> indexData =
                    _BKAuditTemlateMasterService.GetIndexData(index, conditionalFields, conditionalValue);

                ResultModel<int> indexDataCount =
                _BKAuditTemlateMasterService.GetIndexDataCount(index, conditionalFields, conditionalValue);

                int result = _BKAuditTemlateMasterService.GetCount(TableName.BKAuditTemlateMaster, "Id", new[] { "BKAuditTemlateMaster.createdBy", }, new[] { userName });

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<BKAuditTemlateMaster>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }

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
