using myshop.Entities.Models;
using myshop.myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Repository
{
    public interface IContactRepository : IGenericRepoistory<ContactUs>
    {
        List<ContactUs> Search(string? entity);
    }
}
