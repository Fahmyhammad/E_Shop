using Microsoft.AspNetCore.Mvc;
using myshop.DataAccess.Implementation;
using myshop.Entities.Models;
using myshop.Entities.Repository;
using myshop.Entities.ViewModels;
using myshop.myshop.DataAccess.Data;

namespace myshop.WebApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ContactController : Controller
    {
        private readonly IUnitOfWork _db;
        public ContactController(IUnitOfWork db)
        {
            _db = db;
        }
        public IActionResult ContactUs()
        {
            var viewModel = new MainLayoutViewModel
            {
                Categories = _db.Category.GetAll().ToList()
            };

            ViewBag.Cats = viewModel.Categories;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveContact(ContactUs contact)
        {
            var viewModel = new MainLayoutViewModel
            {
                Categories = _db.Category.GetAll().ToList()
            };

            ViewBag.Cats = viewModel.Categories;
            if (ModelState.IsValid)
            {
                _db.contact.Add(contact);
                _db.Complete();
                TempData["Create"] = "Create Message";
                return RedirectToAction("Index", "Home");
            }
            return View(contact);
        }

       
    }
}
