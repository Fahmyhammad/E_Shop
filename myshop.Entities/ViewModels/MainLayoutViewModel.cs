using myshop.Entities.Models;
using myshop.myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.ViewModels
{
    public class MainLayoutViewModel
    {
        public List<Category> Categories { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public List<ContactUs> Contacts {  get; set; }

    }
}
