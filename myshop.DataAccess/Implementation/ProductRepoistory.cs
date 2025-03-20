using Microsoft.EntityFrameworkCore;
using myshop.Entities.Models;
using myshop.Entities.Repository;
using myshop.myshop.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Implementation
{
    public class ProductRepoistory : GenericRepoistory<Product>, IProductRepoistory
    {
        private AppDbContext _db;
        public ProductRepoistory(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product product)
        {
            var result = _db.Products.FirstOrDefault(x=>x.Id ==  product.Id);
            if(result != null)
            {
                result.Name = product.Name;
                result.Description = product.Description;
                result.Price = product.Price;
                result.BeforeDiscount = product.BeforeDiscount;
                result.Offer = product.Offer;
                result.Image = product.Image==null? result.Image:product.Image;
                result.CategoryId = product.CategoryId;
            }
        }
        public async Task<List<Product>> GetAllProducts()
        {
            var result = await _db.Products.Include(x=>x.Category).ToListAsync();
            return result;
        }

        public List<Product> Search(string? entity, string? IncludeWord = null)
        {
            return _db.Products.Include(x=>x.Category).Where(x=>x.Name.ToUpper().Contains(entity) || x.Description.ToUpper().Contains(entity)).ToList();
        }
    }
}
