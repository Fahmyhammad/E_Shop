using myshop.Entities.Repository;
using myshop.myshop.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            Category = new CategoryRepoistory(db);
            Products = new ProductRepoistory(db);
            ShoppingCart = new ShoppingCartRepoistory(db);
            OrderHeader = new OrderHeaderRepoistory(db);
            OrderDetail = new OrderDetailRepoistory(db);
            AppUser = new AppUserRepoistory(db);
            contact = new ContactRepository(db);
        }
        public ICategoryRepository Category { get; private set; }

        public IProductRepoistory Products { get; private set; }

        public IShoppingCartRepository ShoppingCart {  get; private set; }

        public IOrderHeaderRepository OrderHeader { get; private set; }

        public IOrderDetailRepository OrderDetail { get; private set; }

        public IAppUserRepository AppUser { get; private set; }

        public IContactRepository contact { get; private set; }

        public int Complete()
        {
            return _db.SaveChanges();
        }

        public void Dispose()
        {
           _db.Dispose();
        }
    }
}
