using Blky.Utility;
using Bulky.DataAccess.Data;
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
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            var appUser = _context.ApplicationUsers
                .Include(ap => ap.Company)
                .ToList();

            return View(appUser);
        }


        public IActionResult RoleManagement(string userId)
        {

            string RoleId = _context.UserRoles.FirstOrDefault(u => u.UserId == userId).RoleId;

            RoleManagmentVM RoleVM = new RoleManagmentVM()
            {
                ApplicationUser = _context.ApplicationUsers.Include(u => u.Company).FirstOrDefault(u => u.Id == userId),
                RoleList = _context.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name
                }),
                CompanyList = _context.Companies.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };

            RoleVM.ApplicationUser.Role = _context.Roles.FirstOrDefault(u => u.Id == RoleId).Name;


            return View(RoleVM);
        }


        [HttpPost]
        public IActionResult RoleManagement(RoleManagmentVM RoleVM)
        {

            string RoleId = _context.UserRoles.FirstOrDefault(u => u.UserId == RoleVM.ApplicationUser.Id).RoleId;
            string oldRole = _context.Roles.FirstOrDefault(u => u.Id == RoleId).Name;

            if (!(RoleVM.ApplicationUser.Role == oldRole))
            {
                ApplicationUser applicationUser = _context.ApplicationUsers
                    .FirstOrDefault(ap => ap.Id == RoleVM.ApplicationUser.Id);


                if (RoleVM.ApplicationUser.Role == SD.Role_Company)
                {
                    applicationUser.CompanyId = RoleVM.ApplicationUser.CompanyId;
                }

                if (oldRole == SD.Role_Company)
                {
                    applicationUser.CompanyId = null;
                }


                _context.SaveChanges();


                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, RoleVM.ApplicationUser.Role).GetAwaiter().GetResult();

             }


            return RedirectToAction("Index");

        }

        #region Api Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var appUsers = _context.ApplicationUsers
                                  .Include(ap => ap.Company)
                                  .ToList();

            var userRoles = _context.UserRoles.ToList();
            var roles = _context.Roles.ToList();

            foreach (var appUser in appUsers)
            {
                // Find the UserRole where UserId matches appUser.Id
                var userRole = userRoles.FirstOrDefault(u => u.UserId == appUser.Id);

                // Assign the role name, default to empty string if no role is found
                appUser.Role = userRole != null
                    ? roles.FirstOrDefault(r => r.Id == userRole.RoleId)?.Name ?? ""
                    : "";

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


            var objDb = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);

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

            _context.SaveChanges();
            return Json(new
            {
                success = true,
                message = "Success Operation"
            });
        }

        #endregion

    }
}
