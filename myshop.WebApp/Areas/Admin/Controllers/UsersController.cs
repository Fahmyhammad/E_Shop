using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myshop.Entities.Repository;
using myshop.Entities.ViewModels;
using myshop.myshop.DataAccess.Data;
using myshop.myshop.Entities.Models;
using myshop.Utilities;
using System.Security.Claims;

namespace myshop.WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class UsersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        public UsersController(AppDbContext db, IUnitOfWork unitOfWork)
        {
            _db = db;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userid = claim.Value;


            return View(_db.appUsers.Where(x=>x.Id != userid).ToList());
        }

        public IActionResult LockUnLock(string ? id)
        {
            var user = _db.appUsers.FirstOrDefault(x => x.Id == id);
            if (user == null)
                return NotFound();
            if(user.LockoutEnd == null || user.LockoutEnd <  DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now.AddYears(1);
            }
            else
            {
                user.LockoutEnd = DateTime.Now;
            }
            _db.SaveChanges();
            return RedirectToAction("Index", "Users", new { area = "Admin" });
        }

        [HttpGet]
        public IActionResult Delete(string? id)
        {
            if (id == null || id == "0")
            {
                return NotFound();
            }
            var result = _unitOfWork.AppUser.GetById(x=>x.Id == id);
            return View(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUsers(string? id)
        {
            var result = _unitOfWork.AppUser.GetById(x => x.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            _unitOfWork.AppUser.Remove(result);
            _unitOfWork.Complete();
            TempData["Delete"] = "Delete User Successfull";
            return RedirectToAction("Index");
        }

        public IActionResult Search(string? user)
        {
            var result = _unitOfWork.AppUser.Search(user);
            return View("Index", result);
        }

        public IActionResult GetContact()
        {
           var result= _unitOfWork.contact.GetAll().ToList();
            return View(result);
        }

      
    }
}
