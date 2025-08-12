using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BULKYWEB.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                shoppingCartsList = _unitOfWork.ShoppingCart
                .GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),

            };


            foreach (var cart in ShoppingCartVM.shoppingCartsList)
            {
                cart.Price = GetPriceBasedInQuentity(cart);
                ShoppingCartVM.OrderTotal += (cart.Price * cart.Count);
            }



            return View(ShoppingCartVM);
        }


        public IActionResult Summery()
        {
            return View();
        }

        public IActionResult Plus(int id)
        {
            var cartDb = _unitOfWork.ShoppingCart
                     .Get(c => c.Id == id);

            cartDb.Count += 1;

            _unitOfWork.ShoppingCart.Update(cartDb);
            _unitOfWork.Save();
            return RedirectToAction("Index");

        }


        public IActionResult Minus(int id)
        {
            var cartDb = _unitOfWork.ShoppingCart
                     .Get(c => c.Id == id);

            if (cartDb.Count == 1)
            {
                _unitOfWork.ShoppingCart.Remove(cartDb);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                cartDb.Count -= 1;

                _unitOfWork.ShoppingCart.Update(cartDb);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }


        }


        public IActionResult Remove(int id)
        {
            var cartDb = _unitOfWork.ShoppingCart
                     .Get(c => c.Id == id);

            _unitOfWork.ShoppingCart.Remove(cartDb);
            _unitOfWork.Save();
            return RedirectToAction("Index");

        }


        private double GetPriceBasedInQuentity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.price;
            }
            else
            {
                if (shoppingCart.Count <= 100)
                {
                    return shoppingCart.Product.price50;
                }
                else
                {
                    return shoppingCart.Product.price100;
                }
            }
        }
   
    
    
    }
}
