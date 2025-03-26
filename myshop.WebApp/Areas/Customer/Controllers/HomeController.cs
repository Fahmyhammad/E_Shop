using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myshop.Entities.Models;
using myshop.Entities.Repository;
using myshop.Entities.ViewModels;
using myshop.Utilities;
using System.Security.Claims;
using X.PagedList;

namespace myshop.WebApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(int? page)
        {
            var viewModel = new MainLayoutViewModel
            {
                Categories = _unitOfWork.Category.GetAll().ToList()
            };
            ViewBag.Cats = viewModel.Categories;


            var pageNumber = page ?? 1;
            int PageSize = 8;

            var products = _unitOfWork.Products.GetAll(x=>x.Offer == null || x.Offer== 0).ToPagedList(pageNumber , PageSize);
            return View(products);
        }
        [HttpGet]
        public IActionResult Details(int Productid)
        {
            var viewModel = new MainLayoutViewModel
            {
                Categories = _unitOfWork.Category.GetAll().ToList()
            };

            ViewBag.Cats = viewModel.Categories;

            ShoppingCardModel cardModel = new ShoppingCardModel()
            {

                ProductId = Productid,
                Product = _unitOfWork.Products.GetById(v => v.Id == Productid, "Category"),
                Count = 1
            };
            return View(cardModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCardModel shoppingCard)
        {
           

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCard.AppUserId = claim.Value;

            ShoppingCardModel Cartobj = _unitOfWork.ShoppingCart.GetById(x => x.AppUserId == claim.Value && x.ProductId == shoppingCard.ProductId);

            if(Cartobj == null)
            {
                _unitOfWork.ShoppingCart.Add(shoppingCard);
                HttpContext.Session.SetInt32(SD.SessionKey,
                    _unitOfWork.ShoppingCart.GetAll(x => x.AppUserId == claim.Value).ToList().Count()
                    );
                _unitOfWork.Complete();
            }
            else
            {
                _unitOfWork.ShoppingCart.IncreaseCount(Cartobj, shoppingCard.Count);
                _unitOfWork.Complete();
            }

            return RedirectToAction("Index");
        }

      
        public async Task<IActionResult> productListByCat(int categoryId)
        {

            var viewModel = new MainLayoutViewModel
            {
                Products = _unitOfWork.Products.GetAll().Where(x=>x.CategoryId== categoryId).ToList() as IEnumerable<Product>,
                Categories = _unitOfWork.Category.GetAll().ToList()
            };

            ViewBag.Cats = viewModel.Categories;
            return View(viewModel.Products);
        }

        public IActionResult ProductOffer(int? page)
        {
            var viewModel = new MainLayoutViewModel
            {
                Categories = _unitOfWork.Category.GetAll().ToList()
            };
            ViewBag.Cats = viewModel.Categories;


            var pageNumber = page ?? 1;
            int PageSize = 8;

            var products = _unitOfWork.Products.GetAll().Where(x=>x.Offer > 0).ToPagedList(pageNumber, PageSize);
            return View(products);
        }
	}
}
