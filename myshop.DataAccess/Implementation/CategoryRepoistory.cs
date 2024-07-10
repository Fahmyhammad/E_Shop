using myshop.Entities.Repository;
using myshop.myshop.DataAccess.Data;
using myshop.myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Implementation
{
    public class CategoryRepoistory : GenericRepoistory<Category>,ICategoryRepository
    {
        private AppDbContext _db;
        public CategoryRepoistory(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public List<Category> Search(string? entity)
        {
            return _db.Categories.Where(x => x.Name.ToUpper().Contains(entity)).ToList();
        }

        public void UpDate(Category category)
        {
           var result = _db.Categories.FirstOrDefault(x=>x.Id == category.Id);
            if(result != null)
            {
                result.Name = category.Name;
                result.Description = category.Description;
                result.CreatedTime = DateTime.Now;
            }
        }
    }
}
