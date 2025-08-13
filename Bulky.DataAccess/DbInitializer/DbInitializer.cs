using Blky.Utility;
using Bulky.DataAccess.Data;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void Initialize()
        {




            //migration if they are not applied 
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }


            //create roles if they are not created 

            try
            {
                if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                 
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                 
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                 
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();


                    //if roles are not created , then we will create admin user as well 
                    _userManager.CreateAsync(new ApplicationUser
                    {
                        UserName = "admin@Spark.com",
                        Email = "admin@Spark.com",
                        Name = "mahmoud",
                        PhoneNumber = "1234567890",
                        StreetAddress = "sfgiosfghj",
                        State = "ufg",
                        City = "hidpd",
                        PostalCode = "hjk",

                    }, "Admin@123")
                        .GetAwaiter().GetResult();


                    ApplicationUser appUser = _context.ApplicationUsers.FirstOrDefault(ap => ap.Email == "admin@Spark.com");
                    _userManager.AddToRoleAsync(appUser, SD.Role_Admin).GetAwaiter().GetResult();



                }

                return;
            }

            catch (Exception ex)
            {

            }



            return;
        }
    }
}
