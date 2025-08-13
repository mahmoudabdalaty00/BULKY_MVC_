using Blky.Utility;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
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
        public IActionResult SummeryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.shoppingCartsList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product");

            ShoppingCartVM.orderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartVM.orderHeader.ApplicationUserId = userId;

            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);


            foreach (var cart in ShoppingCartVM.shoppingCartsList)
            {
                cart.Price = GetPriceBasedInQuentity(cart);
                ShoppingCartVM.orderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                //it is a regular customer 
                ShoppingCartVM.orderHeader.PaymentStatus = SD.PaymentStatusPending;
                ShoppingCartVM.orderHeader.OrderStatus = SD.StatusPending;
            }
            else
            {
                //it is a company user
                ShoppingCartVM.orderHeader.PaymentStatus = SD.PaymentStatusApprovedForDeployPayments;
                ShoppingCartVM.orderHeader.OrderStatus = SD.StatusApproved;
            }
            _unitOfWork.OrderHeader.Add(ShoppingCartVM.orderHeader);
            _unitOfWork.Save();
            foreach (var cart in ShoppingCartVM.shoppingCartsList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    orderHeaderId = ShoppingCartVM.orderHeader.Id,
                    Price = cart.Price,
                    count = cart.Count
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }

            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                //it is a regular customer account and we need to capture payment
                //stripe logic
                var domain = "https://localhost:7036/";
                var options = new SessionCreateOptions
                {
                    SuccessUrl = domain + $"Customer/ShoppingCart/OrderConfirmation?id={ShoppingCartVM.orderHeader.Id}",
                    CancelUrl = domain + "customer/cart/index",
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                };

                foreach (var item in ShoppingCartVM.shoppingCartsList)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100), // $20.50 => 2050
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name
                            }
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLineItem);
                }


                var service = new SessionService();
                Session session = service.Create(options);
                _unitOfWork.OrderHeader.UpdateStripePayment(ShoppingCartVM.orderHeader.Id, session.Id, session.PaymentIntentId);
                _unitOfWork.Save();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);

            }

            return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVM.orderHeader.Id });
        }





        //public IActionResult OrderConfirmation(int id)
        //{
        //    OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(o => o.Id == id);

        //    if (orderHeader.PaymentStatus != SD.PaymentStatusApprovedForDeployPayments)
        //    {
        //        var service = new SessionService();
        //        Session session = service.Get(orderHeader.SessionId);

        //        if (session.PaymentStatus.ToLower() == "paid")
        //        {
        //            _unitOfWork.OrderHeader.UpdateStripePayment(id, session.Id, session.PaymentIntentId);
        //            _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
        //            _unitOfWork.Save();
        //        }

        //    }

        //    List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart
        //               .GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();

        //    _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
        //    _unitOfWork.Save();



        //    return View(id);
        //}


        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(o => o.Id == id);
            if (orderHeader == null)
            {
                Console.WriteLine($"OrderConfirmation - OrderHeader with ID {id} not found.");
                return NotFound();
            }

            if (orderHeader.PaymentStatus != SD.PaymentStatusApprovedForDeployPayments)
            {
                var service = new SessionService();
                // Expand PaymentIntent to ensure PaymentIntentId is available
                var sessionOptions = new SessionGetOptions
                {
                    Expand = new List<string> { "payment_intent" }
                };
                Session session = service.Get(orderHeader.SessionId, sessionOptions);
                if (session == null)
                {
                    Console.WriteLine($"OrderConfirmation - Session with ID {orderHeader.SessionId} not found.");
                    return BadRequest();
                }

                Console.WriteLine($"OrderConfirmation - Session ID: {session.Id}, PaymentIntentId: {session.PaymentIntentId}, PaymentStatus: {session.PaymentStatus}");

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    string paymentIntentId = session.PaymentIntentId;
                    if (string.IsNullOrEmpty(paymentIntentId))
                    {
                        
                        var paymentIntentService = new PaymentIntentService();
                        var paymentIntentOptions = new PaymentIntentListOptions
                        {
                            Customer = session.CustomerId, 
                            Limit = 1
                            
                        };
                        var paymentIntent = paymentIntentService.List(paymentIntentOptions).FirstOrDefault();
                        if (paymentIntent != null)
                        {
                            paymentIntentId = paymentIntent.Id;
                        }
                        else
                        {
                            Console.WriteLine($"OrderConfirmation - No PaymentIntent found for Session ID {session.Id}.");
                        }
                    }

                    if (!string.IsNullOrEmpty(paymentIntentId))
                    {
                        _unitOfWork.OrderHeader.UpdateStripePayment(id, session.Id, paymentIntentId);
                        _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                        _unitOfWork.Save();
                    }

                    else
                    {
                        Console.WriteLine($"OrderConfirmation - PaymentIntentId is still null for Session ID {session.Id}.");
                    }
                }
                else
                {
                    Console.WriteLine($"OrderConfirmation - PaymentStatus is {session.PaymentStatus}, not 'paid'.");
                }
            }

            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart
                .GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();

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
