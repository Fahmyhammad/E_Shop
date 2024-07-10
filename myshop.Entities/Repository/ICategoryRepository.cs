using myshop.myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Repository
{
    public interface ICategoryRepository : IGenericRepoistory<Category>
    {
        void UpDate(Category category);

        List<Category> Search(string? entity);
    }
}
