using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Shampan.Core.Interfaces.Services;
using Shampan.Core.Interfaces.Services.Audit;
using Shampan.Core.Interfaces.Services.Branch;
using Shampan.Core.Interfaces.Services.Company;
using Shampan.Core.Interfaces.Services.Settings;
using Shampan.Core.Interfaces.Services.User;
using Shampan.Core.Interfaces.Services.UsersPermission;
using Shampan.Models;
using Shampan.Models.AuditModule;
using Shampan.Services.Branch;
using Shampan.Services.Tour;
using Shampan.Services.UserProfil;
using Shampan.Services.UsersPermission;
using ShampanERP.Models;
using ShampanERP.Persistence;
using SSLAudit.Controllers;
using StackExchange.Exceptional;
using System.Security.Claims;

namespace SageERP.Controllers
{
    [ServiceFilter(typeof(UserMenuActionFilter))]
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _applicationDb;
        private readonly ICommonService _commonService;
        private readonly DbConfig _dbConfig;
        private readonly IConfiguration _configuration;
        private readonly IUsersPermissionService _usersPermissionService;
        private readonly IAuditMasterService _auditMasterService;
        private readonly ISettingsService _settingsService;



        public UserProfileController(ApplicationDbContext applicationDb, IConfiguration configuration,
            ICommonService commonService,
              UserManager<ApplicationUser> userManager, DbConfig dbConfig, IUsersPermissionService usersPermissionService,
              IAuditMasterService auditMasterService, ISettingsService settingsService)
        {
            _userManager = userManager;
            _applicationDb = applicationDb;

            _commonService = commonService;
            _dbConfig = dbConfig;


            _usersPermissionService = usersPermissionService;
            _auditMasterService = auditMasterService;
            _configuration = configuration;
            _settingsService = settingsService;

        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            ModelState.Clear();
            UserProfile vm = new UserProfile();
            CommonVM param = new CommonVM();
            UserProfileRepo _repo = new UserProfileRepo();
            List<EmployeeInfoVM> emp = new List<EmployeeInfoVM>();

            ResultModel<string> companyCode = _settingsService.GetSettingsValue(new[] { "SettingGroup", "SettingName" }, new[] { "Company", "Code" });


            if (companyCode.Data.ToUpper() == "BRAC")
            {
                ResultVM result = _repo.GetEmployeeCodeData(param);
                if (result.Status == "Success" && result.DataVM != null)
                {
                    emp = JsonConvert.DeserializeObject<List<EmployeeInfoVM>>(result.DataVM.ToString());
                }
                else
                {
                    emp = null;
                }
                ViewBag.EmployeeList = new SelectList(emp, "EmployeeId", "EmpName");
            }

            ViewBag.CompanyCode = companyCode.Data.ToUpper();
            vm.Operation = "add";
            return View("CreateEdit", vm);
        }
        public IActionResult Edit(string id)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id == id);
            UserProfile vm = new UserProfile();
            vm.UserName = user.UserName;

            vm.Operation = "update";
            return View("ChangePassword", vm);
        }

        public IActionResult _index(string userName)
        {

            try
            {
                var search = Request.Form["search[value]"].FirstOrDefault();
                var users = _userManager.Users;


                if (!string.IsNullOrEmpty(userName))
                {
                    users = users.Where(u => u.UserName.Contains(userName));
                }
                if (!string.IsNullOrEmpty(search))
                {
                    users = users.Where(u => u.UserName.Contains(search));
                }

                var result = users.Count();
                var namesList = users.ToList();
                List<ApplicationUser> data = namesList;

                string draw = Request.Form["draw"].ToString();

                return Ok(new { data = data, draw = draw, recordsTotal = result, recordsFiltered = result });
            }

            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok();

            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateEditAsync(UserProfile model)
        {
            ResultModel<UserProfile> result = new ResultModel<UserProfile>();
            try
            {
                string SageDbName = _dbConfig.SageDbName;
                string DbName = _dbConfig.DbName;

                model.FullName = "NA";


                var claims = new List<Claim>
                {
                      new Claim("Database", DbName),
                      new Claim("SageDatabase", SageDbName),
                };


                if (model.Operation == "update")
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);

                    if (user == null)
                    {
                        result.Message = "User not found.";
                        return Ok(result);
                    }
                    if (string.IsNullOrEmpty(model.Password) && string.IsNullOrEmpty(model.ConfirmPassword))
                    {

                        user.Email = model.Email;
                        user.PhoneNumber = model.PhoneNumber;
                        user.SageUserName = "-";
                        user.PFNo = model.PFNo;
                        user.Designation = model.Designation;
                        user.ProfileName = model.ProfileName;
                        if (model.BranchID == null)
                        {
                            user.BranchName = "-";
                        }
                        else
                        {
                            user.BranchName = model.BranchID;
                        }

                        var updateResult = await _userManager.UpdateAsync(user);
                        if (!updateResult.Succeeded)
                        {
                            result.Message = "Failed to update user profile.";
                            return Ok(result);
                        }



                        if (model.Attachments != null)
                        {
                            UserProfileAttachments up = _usersPermissionService.GetUserIdByName(model.UserName);
                            model.UserId = up.UserId;
                            model.UserName = model.UserName;
                            _usersPermissionService.ImageUpdate(model);

                        }


                        result.Status = Status.Success;
                        result.Message = "User profile updated successfully.";
                        result.Data = model;
                        return Ok(result);
                    }

                    var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.Password);

                    if (!changePasswordResult.Succeeded)
                    {
                        result.Message = "Failed to change the password.";
                        return Ok(result);
                    }


                    result.Status = Status.Success;
                    result.Message = "Password successfully updated.";
                    result.Data = model;


                    return Ok(result);
                }
                else
                {
                    model.SageUserName = "-";
                    model.IsArchive = false;
                    string Branch = "-";

                    if (model.Password != model.ConfirmPassword)
                    {
                        result.Message = "Passwords do not match.";
                        return Ok(result);
                    }

                    var _user = new ApplicationUser { UserName = model.UserName, PhoneNumber = model.PhoneNumber, Email = model.Email, SageUserName = model.SageUserName, Designation = model.Designation, PFNo = model.PFNo, ProfileName = model.ProfileName, BranchName = Branch, IsArchive = model.IsArchive, EmployeeId = model.EmployeeId, FullName = "NA", NormalizedName = "NA", NormalizedPassword = "NA" };

                    var _result = await _userManager.CreateAsync(_user, model.Password);


                    if (!_result.Succeeded)
                    {
                        foreach (var error in _result.Errors)
                        {
                            result.Message = error.Description;
                            return Ok(result);
                        }
                    }
                    var userClaimsresult = await _userManager.AddClaimsAsync(_user, claims);

                    if (model.Attachments != null)
                    {
                        model.UserId = _user.Id;
                        model.UserName = _user.UserName;
                        _usersPermissionService.ImageInsert(model);

                    }

                    result.Status = Status.Success;
                    result.Message = "Successfully Saved";
                    result.Data = model;

                }

            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);
        }

        public ActionResult Profile(string id)
        {

            var user = _userManager.Users.SingleOrDefault(x => x.Id == id);
            UserProfile vm = new UserProfile();
            vm.UserName = user.UserName;
            vm.PhoneNumber = user.PhoneNumber;
            vm.SageUserName = user.SageUserName;
            vm.Email = user.Email;
            vm.PFNo = user.PFNo;
            vm.Designation = user.Designation;
            vm.ProfileName = user.ProfileName;
            vm.BranchID = user.BranchName;
            vm.Operation = "update";
            vm.IsArchive = user.IsArchive;

            vm.AttachmentsList = _usersPermissionService.GetAllImage(new[] { "UserId" }, new[] { id.ToString() }).Data;

            return View("EditProfile", vm);
        }

        public ActionResult ChangePassword()
        {
            string userName = User.Identity.Name;
            var UserDetails = _usersPermissionService.GetUserIdByName(userName);
            var user = _userManager.Users.SingleOrDefault(x => x.Id == UserDetails.UserId);
            UserProfile vm = new UserProfile();
            vm.UserName = user.UserName;
            vm.Operation = "update";
            return View("ChangePassword", vm);
        }

        public ActionResult DeActiveUser(UserProfile master)
        {
            ResultModel<UserProfile> result = new ResultModel<UserProfile>();
            try
            {
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                master.Audit.LastUpdateBy = user.UserName;
                master.Audit.LastUpdateOn = DateTime.Now;
                master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                result = _usersPermissionService.UserProfileDeActivate(master);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }
            return Ok(result);
        }

        public ActionResult ActiveUser(UserProfile master)
        {
            ResultModel<UserProfile> result = new ResultModel<UserProfile>();
            try
            {
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                master.Audit.LastUpdateBy = user.UserName;
                master.Audit.LastUpdateOn = DateTime.Now;
                master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                result = _usersPermissionService.UserProfileActivate(master);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }
            return Ok(result);
        }

        [HttpPost]
        public ActionResult GetUserProfileData([FromBody] CommonVM param)
        {

            UserProfileRepo _repo = new UserProfileRepo();

            List<EmployeeInfoVM> emp = new List<EmployeeInfoVM>();

            ResultVM result = _repo.GetUserProfileData(param);

            if (result.Status == "Success" && result.DataVM != null)
            {
                emp = JsonConvert.DeserializeObject<List<EmployeeInfoVM>>(result.DataVM.ToString());
            }
            else
            {
                emp = null;
            }
            return Ok(emp);
        }

    }
}
