using myshop.Entities.Models;
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
    public class ShoppingCartRepoistory : GenericRepoistory<ShoppingCardModel>,IShoppingCartRepository
    {
        private AppDbContext _db;
        public ShoppingCartRepoistory(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public int decreaseCount(ShoppingCardModel shoppingCard, int count)
        {
            shoppingCard.Count -= count;
            return shoppingCard.Count;
        }

        public int IncreaseCount(ShoppingCardModel shoppingCard, int count)
        {
            shoppingCard.Count += count;
            return shoppingCard.Count;
        }
    }
}
