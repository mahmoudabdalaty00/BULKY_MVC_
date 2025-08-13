using Blky.Utility;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace BULKYWEB.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;


        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            
            var prodList = _unitOfWork.Product
                     .GetAll(includeProperties: "Category");

            IEnumerable<Product> ShownList;

            var shownList = prodList
                  //.Where(p => p.Category != null && !p.Category.IsHidden)
                  .ToList();

            return View(shownList);
        }


        public IActionResult Details(int id)
        {

            var prod = _unitOfWork.Product
                     .Get(u => u.Id == id, "Category");

            ShoppingCart cart = new()
            {
                Product = prod,
                Count = 5,
                ProductId = id,
            };

            return View(cart);
        }


        [HttpPost]
        [Authorize]
        public IActionResult Details([Bind("ProductId,Count")] ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            // shoppingCart.Id = 0; // Ensure Id is not set
            shoppingCart.ApplicationUserId = userId;

            // Validate input
            if (shoppingCart.Count <= 0 || shoppingCart.ProductId <= 0)
            {
                ModelState.AddModelError("", "Invalid cart data.");
                var prod = _unitOfWork.Product.Get(u => u.Id == shoppingCart.ProductId, "Category");
                shoppingCart.Product = prod;
                return View(shoppingCart);
            }

            var existingCart = _unitOfWork.ShoppingCart
                .Get(u => u.ApplicationUserId == userId && u.ProductId == shoppingCart.ProductId);
            if (existingCart != null)
            {
                existingCart.Count += shoppingCart.Count;
                try
                {
                    TempData["success"] = "Cart Updated Successfully";
                    _unitOfWork.Save();

                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Error saving shopping cart: {InnerException}", ex.InnerException?.Message);
                    ModelState.AddModelError("", "An error occurred while saving the cart.");
                    var prod = _unitOfWork.Product.Get(u => u.Id == shoppingCart.ProductId, "Category");
                    shoppingCart.Product = prod;
                    return View(shoppingCart);
                }
                _unitOfWork.ShoppingCart.Update(existingCart);
            }
            else
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                try
                {
                    TempData["success"] = "Cart Updated Successfully";
                    _unitOfWork.Save();

                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Error saving shopping cart: {InnerException}", ex.InnerException?.Message);
                    ModelState.AddModelError("", "An error occurred while saving the cart.");
                    var prod = _unitOfWork.Product.Get(u => u.Id == shoppingCart.ProductId, "Category");
                    shoppingCart.Product = prod;
                    return View(shoppingCart);
                }

            }


            HttpContext.Session.SetInt32(SD.SessionCart,
            _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == userId).Count());
            return RedirectToAction("Index");
        }
















        #region Privacy & Error
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}
