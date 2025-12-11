using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Shampan.Models;
using SSLAudit.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Shampan.Services.Tour;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;
using Shampan.Core.Interfaces.Services.Branch;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Core.Interfaces.Services.Tour;
using Shampan.Core.Interfaces.Services.UserRoll;
using ShampanERP.Persistence;
using ShampanERP.Models;
using StackExchange.Exceptional;

namespace SSLAudit.Controllers
{
    [ServiceFilter(typeof(UserMenuActionFilter))]
    [Authorize]
    public class BranchAccountingController : Controller
    {

        private readonly ApplicationDbContext _applicationDb;
        private readonly IToursService _toursService;
        private readonly IUserRollsService _userRollsService;
        private readonly IMemoryCache _memoryCache;
        private readonly IBranchProfileService _branchProfileService;



        public BranchAccountingController(ApplicationDbContext applicationDb, ITeamsService teamsService, IToursService toursService,
        IMemoryCache memoryCache, IUserRollsService userRollsService, IBranchProfileService branchProfileService)
        {

            _applicationDb = applicationDb;
            _toursService = toursService;
            _userRollsService = userRollsService;
            _memoryCache = memoryCache;
            _branchProfileService = branchProfileService;

        }

        public async Task<IActionResult> Itegration()
        {
            return View();
        }


        public ActionResult SaleIntegration(SalesInvoice vm)
        {
            ResultModel<SalesInvoice> resultData = new ResultModel<SalesInvoice>();

            TourRepo _repo = new TourRepo();
            List<SalesInvoice> emp = new List<SalesInvoice>();
            List<PurchaseInvoice> empData = new List<PurchaseInvoice>();
            SalesInvoice param = new SalesInvoice();
            PurchaseInvoice paramData = new PurchaseInvoice();

            ResultVM result = _repo.GetSaleData(vm);
            if (result.Status == "Success" && result.DataVM != null)
            {
                emp = JsonConvert.DeserializeObject<List<SalesInvoice>>(result.DataVM.ToString());
            }
            else
            {
                emp = null;
            }

            foreach(SalesInvoice data in emp)
            {
                resultData = _toursService.SaleInsert(data);

            }


            //result = _repo.GetPurchaseData(paramData);
            //if (result.Status == "Success" && result.DataVM != null)
            //{
            //    empData = JsonConvert.DeserializeObject<List<PurchaseInvoice>>(result.DataVM.ToString());
            //}
            //else
            //{
            //    emp = null;
            //}


            //return View();
            return Ok(resultData);
        }

        public async Task<IActionResult> Index()
        {
            return View();
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
                string teamname = Request.Form["teamname"].ToString();
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
                index.orderDir = "desc";

                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);


                index.createdBy = userName;


                string[] conditionalFields = new[]
                {
                            "Tours.Code like",
                            "TeamName like",
                            "Tours.Description like",
                            "Tours.IsPost like"

                };

                string?[] conditionalValue = new[] { code, teamname, description, post };

                ResultModel<List<SalesInvoice>> indexData =
                    _toursService.GetSaleIndexData(index, conditionalFields, conditionalValue);


                ResultModel<int> indexDataCount =
                _toursService.GetSaleIndexDataCount(index, conditionalFields, conditionalValue);

                int result = _toursService.GetCount(TableName.Tours, "Id", new[] { "Tours.createdBy", }, new[] { userName });

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<Tours>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }


        }


        public ActionResult PurchaseIntegration(PurchaseInvoice vm)
        {
            ResultModel<PurchaseInvoice> resultData = new ResultModel<PurchaseInvoice>();

            TourRepo _repo = new TourRepo();
            List<SalesInvoice> emp = new List<SalesInvoice>();
            List<PurchaseInvoice> empData = new List<PurchaseInvoice>();
            SalesInvoice param = new SalesInvoice();
            PurchaseInvoice paramData = new PurchaseInvoice();


            ResultVM result = _repo.GetPurchaseData(vm);

            if (result.Status == "Success" && result.DataVM != null)
            {
                empData = JsonConvert.DeserializeObject<List<PurchaseInvoice>>(result.DataVM.ToString());
            }
            else
            {
                emp = null;
            }

            foreach (PurchaseInvoice data in empData)
            {
                resultData = _toursService.PurchaseInsert(data);
            }



            return Ok(resultData);
        }



        public async Task<IActionResult> PurchaseIndex()
        {
            return View();
        }
        public IActionResult _indexPurchase()
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
                string teamname = Request.Form["teamname"].ToString();
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
                index.orderDir = "desc";

                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);


                index.createdBy = userName;


                string[] conditionalFields = new[]
                {
                            "Tours.Code like",
                            "TeamName like",
                            "Tours.Description like",
                            "Tours.IsPost like"

                };

                string?[] conditionalValue = new[] { code, teamname, description, post };

                ResultModel<List<PurchaseInvoice>> indexData =
                    _toursService.GetPurchaseIndexData(index, conditionalFields, conditionalValue);


                ResultModel<int> indexDataCount =
                _toursService.GetPurchaseIndexDataCount(index, conditionalFields, conditionalValue);

                int result = _toursService.GetCount(TableName.Tours, "Id", new[] { "Tours.createdBy", }, new[] { userName });

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<Tours>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }


        }




    }
}
