using Blky.Utility;
using Bulky.DataAccess.Data;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BULKYWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext unitOfWork)
        {
            _context = unitOfWork;
        }

        public IActionResult Index()
        {
            var appUser = _context.ApplicationUsers
                .Include(ap => ap.Company)
                .ToList();




            return View(appUser);
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









        [HttpDelete]
        public IActionResult Delete(int? id)
        {

            return Json(new
            {
                success = true,
                message = "Success To Deleting"
            });
        }

        #endregion

    }
}
