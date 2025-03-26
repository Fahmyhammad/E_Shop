using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myshop.Entities.Models;
using myshop.Entities.Repository;
using myshop.Entities.ViewModels;
using myshop.Utilities;
using Stripe;
using Stripe.Climate;

namespace myshop.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderViewModel OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<OrderHeader> orderHeaders;
            orderHeaders = _unitOfWork.OrderHeader.GetAll(x=>x.OrderStatus == SD.Approve &&  x.Product.Offer == null  ,IncludeWord: nameof(AppUser), "Product");
            return View(orderHeaders);

        }
        public IActionResult OrdersProccessing()
        {
            IEnumerable<OrderHeader> orderHeaders;
            orderHeaders = _unitOfWork.OrderHeader.GetAll(x => x.OrderStatus == SD.Proccessing, IncludeWord: nameof(AppUser));
            return View(orderHeaders);

        }
        public IActionResult OrdersCancelled()
        {
            IEnumerable<OrderHeader> orderHeaders;
            orderHeaders = _unitOfWork.OrderHeader.GetAll(x => x.OrderStatus == SD.Cancelled, IncludeWord: nameof(AppUser));
            return View(orderHeaders);

        }
        public IActionResult OrdersShipped()
        {
            IEnumerable<OrderHeader> orderHeaders;
            orderHeaders = _unitOfWork.OrderHeader.GetAll(x => x.OrderStatus == SD.Shipped, IncludeWord: nameof(AppUser));
            return View(orderHeaders);

        }
        public IActionResult OrdersPending()
        {
            IEnumerable<OrderHeader> orderHeaders;
            orderHeaders = _unitOfWork.OrderHeader.GetAll(x => x.OrderStatus == "Pending", IncludeWord: nameof(AppUser));
            return View(orderHeaders);

        }
        public IActionResult OrdersOffers()
        {
            IEnumerable<OrderHeader> orderHeaders;

            orderHeaders = _unitOfWork.OrderHeader.GetAll(x => x.Product.Offer > 0, IncludeWord: $"Product","AppUser");
           
            return View(orderHeaders);

        }
        public IActionResult Details(int orderid)
        {
            OrderViewModel orderVM = new OrderViewModel()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetById(x => x.Id == orderid , IncludeWord: "AppUser"),
                orderDetails = _unitOfWork.OrderDetail.GetAll(x=>x.OrderHeaderId == orderid , IncludeWord: "Product"),
               
            };
            return View(orderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderDetails()
        {
            var orderFromdb = _unitOfWork.OrderHeader.GetById(x => x.Id == OrderVM.OrderHeader.Id);
            orderFromdb.Name = OrderVM.OrderHeader.Name;
            orderFromdb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderFromdb.Adderss = OrderVM.OrderHeader.Adderss;
            orderFromdb.City = OrderVM.OrderHeader.City;

            if(OrderVM.OrderHeader.Carrier != null)
            {
                orderFromdb.Carrier = OrderVM.OrderHeader.Carrier;
            }

            if(OrderVM.OrderHeader.TrackingNumber != null)
            {
                orderFromdb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            }

            _unitOfWork.OrderHeader.UpDate(orderFromdb);
            _unitOfWork.Complete();

            TempData["Edit"] = "Edit Order Details";
            return RedirectToAction("Details", "Order", new { orderId = orderFromdb.Id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StartProccess()
        {

            _unitOfWork.OrderHeader.UpdateOrderStatus(OrderVM.OrderHeader.Id, SD.Proccessing, null);
            _unitOfWork.Complete();
           
            TempData["OrderProccessing"] = "Edit Order Status";
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StartShip()
        {
            var orderFromdb = _unitOfWork.OrderHeader.GetById(x => x.Id == OrderVM.OrderHeader.Id);
            orderFromdb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            orderFromdb.Carrier = OrderVM.OrderHeader.Carrier;
            orderFromdb.OrderStatus = SD.Shipped;
            orderFromdb.ShoppingDate = DateTime.Now;

            _unitOfWork.OrderHeader.UpDate(orderFromdb);
            _unitOfWork.Complete();

            TempData["OrderView"] = "order has Shiped Successfully";
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder()
        {
            var orderFromdb = _unitOfWork.OrderHeader.GetById(x => x.Id == OrderVM.OrderHeader.Id);

            if (orderFromdb == null)
            {
                TempData["Error"] = "Order not found.";
                return RedirectToAction("Index");
            }

            if (orderFromdb.PaymentStatus == SD.Approve)
            {
                if (orderFromdb.PaymentStatus == SD.Refund)
                {
                    TempData["CancelOrder"] = "Order has already been refunded.";
                }
                else
                {
                    var options = new RefundCreateOptions
                    {
                        Reason = RefundReasons.RequestedByCustomer,
                        PaymentIntent = orderFromdb.PaymentIntentId
                    };

                    var service = new RefundService();
                    try
                    {
                        Refund refund = service.Create(options);
                        orderFromdb.PaymentStatus = SD.Refund;
                        _unitOfWork.OrderHeader.UpdateOrderStatus(orderFromdb.Id, SD.Cancelled, SD.Refund);
                    }
                    catch (StripeException ex)
                    {
                        TempData["Error"] = $"Error processing refund: {ex.Message}";
                        return RedirectToAction("Details", new { orderId = OrderVM.OrderHeader.Id });
                    }
                }
            }
            else
            {
                _unitOfWork.OrderHeader.UpdateOrderStatus(orderFromdb.Id, SD.Cancelled, SD.Cancelled);
            }

            _unitOfWork.OrderHeader.UpDate(orderFromdb);
            _unitOfWork.Complete();
            TempData["CancelOrder"] = "Order has been cancelled successfully.";
            return RedirectToAction("Details", new { orderId = OrderVM.OrderHeader.Id });
        }


        public IActionResult Search(string? order)
        {
            var result = _unitOfWork.OrderHeader.Search(order);
            return View("Index", result);
        }
    }
}
