using Blky.Utility;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Security.Claims;

namespace BULKYWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int orderId)
        {
            OrderVM = new()
            {
                orderHeader = _unitOfWork.OrderHeader.Get(o => o.Id == orderId, includeProperties: "ApplicationUser"),
                orderDetail = _unitOfWork.OrderDetail.GetAll(o => o.orderHeaderId == orderId, includeProperties: "Product"),

            };

            return View(OrderVM);
        }



        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult UpdateOrderDetails()
        {

            var orderHeaderDb = _unitOfWork.OrderHeader
                .Get(o => o.Id == OrderVM.orderHeader.Id, includeProperties: "ApplicationUser");

            orderHeaderDb.Name = OrderVM.orderHeader.Name;
            orderHeaderDb.PhoneNumber = OrderVM.orderHeader.PhoneNumber;
            orderHeaderDb.StreetAddress = OrderVM.orderHeader.StreetAddress;
            orderHeaderDb.City = OrderVM.orderHeader.City;
            orderHeaderDb.State = OrderVM.orderHeader.State;
            orderHeaderDb.PostalCode = OrderVM.orderHeader.PostalCode;

            if (!string.IsNullOrEmpty(OrderVM.orderHeader.Carrier))
            {
                orderHeaderDb.Carrier = OrderVM.orderHeader.Carrier;
            }
            if (!string.IsNullOrEmpty(OrderVM.orderHeader.TrackingNumber))
            {
                orderHeaderDb.TrackingNumber = OrderVM.orderHeader.TrackingNumber;
            }

            _unitOfWork.OrderHeader.Update(orderHeaderDb);
            _unitOfWork.Save();


            TempData["success"] = "Order Details Updated Successfully";



            return RedirectToAction(nameof(Details), new
            {
                orderId = orderHeaderDb.Id
            });
        }



        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult StartProcessing()
        {
            _unitOfWork.OrderHeader.UpdateStatus(OrderVM.orderHeader.Id, SD.StatusProcessing);
            _unitOfWork.Save();
            TempData["Success"] = "Order Details Updated Successfully.";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.orderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult ShipOrder()
        {

            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.orderHeader.Id);
            orderHeader.TrackingNumber = OrderVM.orderHeader.TrackingNumber;
            orderHeader.Carrier = OrderVM.orderHeader.Carrier;
            orderHeader.OrderStatus = SD.StatusShipped;
            orderHeader.ShippingDate = DateTime.Now;
            if (orderHeader.PaymentStatus == SD.PaymentStatusApprovedForDeployPayments)
            {
                orderHeader.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
            }

            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Save();
            TempData["Success"] = "Order Shipped Successfully.";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.orderHeader.Id });
        }



        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult Cancelled()
        {
            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.orderHeader.Id);

            if (orderHeader.PaymentStatus == SD.PaymentStatusApproved)
            {
                var options = new RefundCreateOptions
                {
                   Reason = RefundReasons.RequestedByCustomer,
                   PaymentIntent = orderHeader.PaymentIntentId,
                };

                var service = new RefundService();
                Refund refund = service.Create(options);

                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusRefunded);
                _unitOfWork.Save();
            }
            else
            {
                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusCancelled);
            }


            TempData["Success"] = "Order Cancelled Successfully.";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.orderHeader.Id });

        }









        #region Api Calls
        public IActionResult GetAll(string status)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            IEnumerable<OrderHeader> objOrderHeaders;


            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                objOrderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();
            }
            else
            {
                objOrderHeaders = _unitOfWork.OrderHeader.GetAll
                    (o => o.ApplicationUserId == userId
                , includeProperties: "ApplicationUser").ToList();
            }

            switch (status)
            {
                case "pending":
                    objOrderHeaders = objOrderHeaders.Where(u => u.PaymentStatus == SD.PaymentStatusApprovedForDeployPayments);
                    break;
                case "inprocess":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusProcessing);
                    break;
                case "completed":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                default:
                    break;

            }


            return Json(new { data = objOrderHeaders });
        }


        #endregion

    }
}
