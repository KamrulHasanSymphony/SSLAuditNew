using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.Oragnogram;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using ShampanERP.Persistence;

namespace SSLAudit.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]

	public class OragnogramController : Controller
    {

		private readonly ApplicationDbContext _applicationDb;
		private readonly IOragnogramService _oragnogramService;

		public OragnogramController(ApplicationDbContext applicationDb, IOragnogramService oragnogramService)
		{

			_applicationDb = applicationDb;
			_oragnogramService = oragnogramService;

		}

		public IActionResult Index()
        {
            return View();
        }


		public ActionResult GetEmployees()
		{

			ResultModel<List<object>> chart = _oragnogramService.GetEmployeesData();

			List<object> chartData = chart.Data;

			return Ok(chartData);

		}


	}
}
