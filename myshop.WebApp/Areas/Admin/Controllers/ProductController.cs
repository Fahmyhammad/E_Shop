using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using myshop.Entities.Models;
using myshop.Entities.Repository;
using myshop.Entities.ViewModels;
using myshop.myshop.DataAccess.Data;
using myshop.myshop.Entities.Models;
using System.Data;
using System.Runtime.CompilerServices;

namespace myshop.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {

        private IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _host;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment host)
        {
            _host = host;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var result =await _unitOfWork.Products.GetAllProducts();
            
            return View(result);
        }
        //public IActionResult GetData()
        //{
        //    var products = _unitOfWork.Products.GetAll(IncludeWord: "Category");
        //    return Json(new { data = products });
        //}

        [HttpGet]
        public IActionResult Create()
        {
            ProductViewModel productView = new ProductViewModel()
            {
                Product = new Product(),

                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()

                })
            };
            return View(productView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductViewModel model)
        {
            if (model != null)
            {
                string fileName = string.Empty;
                if (model.file != null)
                {
                    string uploads = Path.Combine(_host.WebRootPath, @"Images\Products");
                    fileName = Guid.NewGuid() + model.file.FileName;
                    string fullPath = Path.Combine(uploads, fileName);
                    model.file.CopyTo(new FileStream(fullPath, FileMode.Create));
                    model.Product.Image = fileName;
                }
                Product productmodel = new Product()
                {
                    Name = model.Product.Name,
                    Description = model.Product.Description,
                    Price = model.Product.Price,
                    Image = model.Product.Image,
                    CategoryId = model.Product.CategoryId,
                    
                };
                _unitOfWork.Products.Add(productmodel);
                _unitOfWork.Complete();
                TempData["Create"] = "Create Product";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            ProductViewModel productView = new ProductViewModel()
            {

                Product = _unitOfWork.Products.GetById(x => x.Id == id),

                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()

                })
            };
            return View(productView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductViewModel model)
        {

            if (model != null)
            {
                string fileName = string.Empty;
                if (model.file != null)
                {
                    string uploads = Path.Combine(_host.WebRootPath, @"Images\Products");
                    fileName = Guid.NewGuid() + model.file.FileName;
                    string fullPath = Path.Combine(uploads, fileName);
                    if (model.Product.Image != null)
                    {
                        var oldImage = Path.Combine(uploads, model.Product.Image.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }

                    model.file.CopyTo(new FileStream(fullPath, FileMode.Create));
                    model.Product.Image = fileName;

                }

                _unitOfWork.Products.Update(model.Product);
                _unitOfWork.Complete();


                TempData["Edit"] = "Edit Product";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var result = _unitOfWork.Products.GetById(x => x.Id == id);
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProduct(int? id)
        {
            var result = _unitOfWork.Products.GetById(x => x.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            _unitOfWork.Products.Remove(result);
            _unitOfWork.Complete();
            TempData["Delete"] = "Delete Product Successfull";
            return RedirectToAction("Index");
        }

        public IActionResult Search(string? product)
        {
            var result = _unitOfWork.Products.Search(product);

            return View("Index", result);
        }
       
    }
}
