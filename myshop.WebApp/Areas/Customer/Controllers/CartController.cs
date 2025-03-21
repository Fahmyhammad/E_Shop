﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using myshop.Entities.Models;
using myshop.Entities.Repository;
using myshop.Entities.ViewModels;
using myshop.Utilities;
using Stripe.Checkout;
using System.Security.Claims;

namespace myshop.WebApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartView CartView { get; set; }
        public static int countCart;

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
          //  countCart = _unitOfWork.ShoppingCart.GetAll().Count();
        }

        [HttpGet]
        public IActionResult GetCartCount()
        {
            int count = _unitOfWork.ShoppingCart.GetAll().Count();
            return Json(new { count });
        }
        public IActionResult Index()
        {
            var viewModel = new MainLayoutViewModel
            {
                Categories = _unitOfWork.Category.GetAll().ToList()
            };

            ViewBag.Cats = viewModel.Categories;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            CartView = new ShoppingCartView()
            {
                CartsList = _unitOfWork.ShoppingCart.GetAll(x => x.AppUserId == claim.Value, "Product")
            };

            foreach (var item in CartView.CartsList)
            {
                CartView.TotalCarts += (item.Count * item.Product.Price);
            }
            return View(CartView);
        }
        public IActionResult Plus(int cartid)
        {
            var viewModel = new MainLayoutViewModel
            {
                Categories = _unitOfWork.Category.GetAll().ToList()
            };

            ViewBag.Cats = viewModel.Categories;
            var ShoppingCart = _unitOfWork.ShoppingCart.GetById(x => x.Id == cartid);
            _unitOfWork.ShoppingCart.IncreaseCount(ShoppingCart, 1);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
        public IActionResult Minus(int cartid)
        {
            var ShoppingCart = _unitOfWork.ShoppingCart.GetById(x => x.Id == cartid);
            var viewModel = new MainLayoutViewModel
            {
                Categories = _unitOfWork.Category.GetAll().ToList()
            };

            ViewBag.Cats = viewModel.Categories;
            if (ShoppingCart.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(ShoppingCart);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _unitOfWork.ShoppingCart.decreaseCount(ShoppingCart, 1);
            }

            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int cartid)
        {
            var viewModel = new MainLayoutViewModel
            {
                Categories = _unitOfWork.Category.GetAll().ToList()
            };

            ViewBag.Cats = viewModel.Categories;
            var ShoppingCart = _unitOfWork.ShoppingCart.GetById(x => x.Id == cartid);
            _unitOfWork.ShoppingCart.Remove(ShoppingCart);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }

        public IActionResult Summary()
        {
            var viewModel = new MainLayoutViewModel
            {
                Categories = _unitOfWork.Category.GetAll().ToList()
            };

            ViewBag.Cats = viewModel.Categories;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            CartView = new ShoppingCartView()
            {
                CartsList = _unitOfWork.ShoppingCart.GetAll(x => x.AppUserId == claim.Value, "Product"),
                OrderHeader = new()
            };

            CartView.OrderHeader.AppUser = _unitOfWork.AppUser.GetById(x => x.Id == claim.Value);

            CartView.OrderHeader.Name = CartView.OrderHeader.AppUser.Name;
            CartView.OrderHeader.Adderss = CartView.OrderHeader.AppUser.Address;
            CartView.OrderHeader.City = CartView.OrderHeader.AppUser.City;
            CartView.OrderHeader.PhoneNumber = CartView.OrderHeader.AppUser.Phone;

            foreach (var item in CartView.CartsList)
            {
                CartView.TotalCarts += (item.Count * item.Product.Price);
            }
            return View(CartView);

        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult PostSummary(ShoppingCartView cartView)
        {
            try
            {
                var viewModel = new MainLayoutViewModel
                {
                    Categories = _unitOfWork.Category.GetAll().ToList()
                };

                ViewBag.Cats = viewModel.Categories;
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                cartView.CartsList = _unitOfWork.ShoppingCart.GetAll(x => x.AppUserId == claim.Value, "Product");

                cartView.OrderHeader.OrderStatus = "Pending";
                cartView.OrderHeader.PaymentStatus = "Pending";
                cartView.OrderHeader.OrderDate = DateTime.Now;
                cartView.OrderHeader.AppUserId = claim.Value;

                foreach (var item in cartView.CartsList)
                {
                    cartView.OrderHeader.TotalPrice += (item.Count * item.Product.Price);
                }
                _unitOfWork.OrderHeader.Add(cartView.OrderHeader);
                _unitOfWork.Complete();

                foreach (var item in cartView.CartsList)
                {
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        ProductId = item.ProductId,
                        OrderHeaderId = cartView.OrderHeader.Id,
                        Price = item.Product.Price,
                        Count = item.Count
                    };
                    _unitOfWork.OrderDetail.Add(orderDetail);
                    _unitOfWork.Complete();

                }
                var domain = "https://localhost:44364/";
                var options = new SessionCreateOptions
                {
                    LineItems = new List<SessionLineItemOptions>(),

                    Mode = "payment",
                    SuccessUrl = domain + $"customer/cart/Orderconfirmation?id={cartView.OrderHeader.Id}",
                    CancelUrl = domain + $"customer/cart/index",
                };

                foreach (var item in cartView.CartsList)
                {
                    var sessionlineoption = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Product.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name,                                
                            },
                        },
                        Quantity = item.Count,
                    };
                    options.LineItems.Add(sessionlineoption);
                }

                var service = new SessionService();
                Session session = service.Create(options);
                cartView.OrderHeader.SessionId = session.Id;
                _unitOfWork.Complete();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);


            }
            catch (Exception)
            {

                throw;
            }


        }

        public IActionResult Orderconfirmation(int id)
        {
            var viewModel = new MainLayoutViewModel
            {
                Categories = _unitOfWork.Category.GetAll().ToList()
            };

            ViewBag.Cats = viewModel.Categories;
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetById(x => x.Id == id);
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);

            if(session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeader.UpdateOrderStatus(id, SD.Approve, SD.Approve);
                orderHeader.PaymentIntentId = session.PaymentIntentId;
                _unitOfWork.Complete();
            }
            List<ShoppingCardModel> shoppingCards = _unitOfWork.ShoppingCart.GetAll(x => x.AppUserId == orderHeader.AppUserId).ToList();
            _unitOfWork.ShoppingCart.RemoveRange(shoppingCards);
            _unitOfWork.Complete();
            return View(id);
        }

    }
}
