using myshop.myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.ViewModels
{
    public class CategoryViewModel 
    {
        public IEnumerable<Category> Categories { get; set;}
    }
}
