using Blky.Utility;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BULKYWEB.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                if (User.Identity != null && User.Identity.IsAuthenticated)
                {
                    var claimIdentity = (ClaimsIdentity)User.Identity;
                    var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (!string.IsNullOrEmpty(userId))
                    {
                        var returnView = HttpContext.Session.GetInt32(SD.SessionCart);
                        if (returnView == null)
                        {
                            returnView = ( _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == userId)).Count();
                            HttpContext.Session.SetInt32(SD.SessionCart, returnView.Value);
                        }
                        return View(returnView);
                    }
                }

                HttpContext.Session.Clear();
                return View(0);
            }
            catch (Exception ex)
            {
                 
                Console.WriteLine($"Error in ShoppingCart ViewComponent: {ex.Message}");
                return View(0);
            }
        }





    }

}
