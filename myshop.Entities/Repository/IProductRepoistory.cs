using myshop.Entities.Models;
using myshop.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Repository
{
    public interface IProductRepoistory : IGenericRepoistory<Product>
    {
        void Update(Product product);
        Task<List<Product>> GetAllProducts();
        List<Product> Search(string? entity, string? IncludeWord = null);

    }
}
