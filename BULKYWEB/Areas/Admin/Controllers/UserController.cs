using Blky.Utility;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BULKYWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(IUnitOfWork context, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = context;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            var appUser = _unitOfWork.ApplicationUser.GetAll(includeProperties: "Company")
                .ToList();

            return View(appUser);
        }


        public IActionResult RoleManagement(string userId)
        {

            RoleManagmentVM RoleVM = new RoleManagmentVM()
            {
                ApplicationUser = _unitOfWork.ApplicationUser
                .Get(u => u.Id == userId,includeProperties: "Company"),


                RoleList = _roleManager.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name
                }),
                CompanyList = _unitOfWork.Company.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };

            RoleVM.ApplicationUser.Role = _userManager
                .GetRolesAsync(_unitOfWork.ApplicationUser.Get(u => u.Id == userId))
                .GetAwaiter().GetResult().FirstOrDefault();


            return View(RoleVM);
        }


        [HttpPost]
        public IActionResult RoleManagement(RoleManagmentVM RoleVM)
        {

            string oldRole = RoleVM.ApplicationUser.Role = _userManager
                .GetRolesAsync(_unitOfWork.ApplicationUser.Get(u => u.Id == RoleVM.ApplicationUser.Id))
                .GetAwaiter().GetResult().FirstOrDefault();

            ApplicationUser applicationUser = _unitOfWork.ApplicationUser
                    .Get(ap => ap.Id == RoleVM.ApplicationUser.Id);

            if (!(RoleVM.ApplicationUser.Role == oldRole))
            {
                


                if (RoleVM.ApplicationUser.Role == SD.Role_Company)
                {
                    applicationUser.CompanyId = RoleVM.ApplicationUser.CompanyId;
                }

                if (oldRole == SD.Role_Company)
                {
                    applicationUser.CompanyId = null;
                }

                 _unitOfWork.ApplicationUser.Update(applicationUser);
                _unitOfWork.Save();


                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, RoleVM.ApplicationUser.Role).GetAwaiter().GetResult();

            }
            else
            {
                if(oldRole ==SD.Role_Company && applicationUser.CompanyId  != RoleVM.ApplicationUser.CompanyId)
                {
                    applicationUser.CompanyId = RoleVM.ApplicationUser.CompanyId;
                    _unitOfWork.ApplicationUser.Update(applicationUser);
                    _unitOfWork.Save();
                }
            }


                return RedirectToAction("Index");

        }

        #region Api Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var appUsers = _unitOfWork.ApplicationUser.GetAll(includeProperties:"Company")
                                  .ToList();
        
            foreach (var appUser in appUsers)
            {
         
 
                // Assign the role name, default to empty string if no role is found
                appUser.Role =   _userManager.GetRolesAsync (appUser)
                    .GetAwaiter ().GetResult().FirstOrDefault() ??  "" ;
                    
 
                // Handle null Company
                if (appUser.Company == null)
                {
                    appUser.Company = new Company
                    {
                        Name = ""
                    };
                }
            }

            return Json(new { data = appUsers });
        }









        [HttpPost]
        public IActionResult LockUnLock([FromBody] string id)
        {


            var objDb = _unitOfWork.ApplicationUser.Get(u => u.Id == id);

            if (objDb == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Failed To Deleting"
                });
            }

            if (objDb.LockoutEnd != null && objDb.LockoutEnd > DateTime.Now)
            {
                objDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }

            _unitOfWork.Save();
            return Json(new
            {
                success = true,
                message = "Success Operation"
            });
        }

        #endregion

    }
}
