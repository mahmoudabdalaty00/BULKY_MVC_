using Blky.Utility;
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
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        #region  Finished

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                shoppingCartsList = _unitOfWork.ShoppingCart
                .GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),

                orderHeader = new OrderHeader()
            };


            foreach (var cart in ShoppingCartVM.shoppingCartsList)
            {
                cart.Price = GetPriceBasedInQuentity(cart);
                ShoppingCartVM.orderHeader.OrderTotal += (cart.Price * cart.Count);
            }



            return View(ShoppingCartVM);
        }


        public IActionResult Summery()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                shoppingCartsList = _unitOfWork.ShoppingCart
                .GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),

                orderHeader = new OrderHeader()
            };

            ShoppingCartVM.orderHeader.ApplicationUser = _unitOfWork
                    .ApplicationUser.Get(u => u.Id == userId);


            ShoppingCartVM.orderHeader.Name = ShoppingCartVM.orderHeader.ApplicationUser.Name;
            ShoppingCartVM.orderHeader.PhoneNumber = ShoppingCartVM.orderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.orderHeader.StreetAddress = ShoppingCartVM.orderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.orderHeader.City = ShoppingCartVM.orderHeader.ApplicationUser.City;
            ShoppingCartVM.orderHeader.State = ShoppingCartVM.orderHeader.ApplicationUser.State;
            ShoppingCartVM.orderHeader.PostalCode = ShoppingCartVM.orderHeader.ApplicationUser.PostalCode;


            foreach (var cart in ShoppingCartVM.shoppingCartsList)
            {
                cart.Price = GetPriceBasedInQuentity(cart);
                ShoppingCartVM.orderHeader.OrderTotal += (cart.Price * cart.Count);
            }



            return View(ShoppingCartVM);
        }
        #endregion



        [HttpPost]
        [ActionName("Summery")]
        public IActionResult SummeryPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(); // Handle missing user ID
            }

            ShoppingCartVM.shoppingCartsList = _unitOfWork.ShoppingCart
                .GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product");

            ApplicationUser appUser = _unitOfWork
                .ApplicationUser.Get(u => u.Id == userId);

            if (appUser == null)
            {
                return NotFound("User not found");
            }

            // Ensure OrderHeader is initialized properly
            ShoppingCartVM.orderHeader.ApplicationUserId = userId;
            ShoppingCartVM.orderHeader.OrderDate = DateTime.Now; // Set required fields

            foreach (var cart in ShoppingCartVM.shoppingCartsList)
            {
                cart.Price = GetPriceBasedInQuentity(cart);
                ShoppingCartVM.orderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            if (appUser.CompanyId.GetValueOrDefault() == 0)
            {
                ShoppingCartVM.orderHeader.PaymentStatus = SD.PaymentStatusPending;
                ShoppingCartVM.orderHeader.OrderStatus = SD.StatusPending;
            }
            else
            {
                ShoppingCartVM.orderHeader.PaymentStatus = SD.PaymentStatusApprovedForDeployPayments;
                ShoppingCartVM.orderHeader.OrderStatus = SD.StatusApproved;
            }

            
                _unitOfWork.OrderHeader.Add(ShoppingCartVM.orderHeader);
                _unitOfWork.Save();

                foreach (var cart in ShoppingCartVM.shoppingCartsList)
                {
                    OrderDetail orderDetail = new OrderDetail
                    {
                        ProductId = cart.ProductId,
                        orderHeaderId = ShoppingCartVM.orderHeader.Id,
                        Price = cart.Price,
                        count = cart.Count
                    };
                    _unitOfWork.OrderDetail.Add(orderDetail);
                }
                _unitOfWork.Save();
                
            

            return RedirectToAction(nameof(OrderConfirmation), new
            {
                id = ShoppingCartVM.orderHeader.Id
            });
        }



        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }







        #region Finished
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
        #endregion


    }
}
