using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myshop.Entities.Repository;
using myshop.myshop.DataAccess.Data;
using myshop.myshop.Entities.Models;
using System.Runtime.CompilerServices;

namespace myshop.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var result = _unitOfWork.Category.GetAll();
            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                // _db.Categories.Add(category);
                _unitOfWork.Category.Add(category);
                // _db.SaveChanges();
                _unitOfWork.Complete();
                TempData["Create"] = "Create Category";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //  var result = _db.Categories.Find(id);
            var result = _unitOfWork.Category.GetById(x => x.Id == id);
            return View(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                // _db.Categories.Update(category);
                _unitOfWork.Category.UpDate(category);
                //_db.SaveChanges();
                _unitOfWork.Complete();
                TempData["Edit"] = "Edit Category";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            // var result = _db.Categories.Find(id);
            var result = _unitOfWork.Category.GetById(x => x.Id == id);
            return View(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int? id)
        {
            //var result = _db.Categories.Find(id);
            var result = _unitOfWork.Category.GetById(x => x.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            // _db.Categories.Remove(result);
            _unitOfWork.Category.Remove(result);
            //_db.SaveChanges();
            _unitOfWork.Complete();
            TempData["Delete"] = "Delete Category Successfull";
            return RedirectToAction("Index");
        }

        public IActionResult Search(string? catrgory)
        {
            var result = _unitOfWork.Category.Search(catrgory);
            return View("Index", result);
        }
    }
}
