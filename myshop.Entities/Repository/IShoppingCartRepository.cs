using myshop.Entities.Models;
using myshop.myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Repository
{
    public interface IShoppingCartRepository : IGenericRepoistory<ShoppingCardModel>
    {
        int IncreaseCount(ShoppingCardModel shoppingCard, int count);
        int decreaseCount(ShoppingCardModel shoppingCard, int count);
    }
}
