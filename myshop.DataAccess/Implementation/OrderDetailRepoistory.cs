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
    public class OrderDetailRepoistory : GenericRepoistory<OrderDetail>, IOrderDetailRepository
    {
        private AppDbContext _db;
        public OrderDetailRepoistory(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void UpDate(OrderDetail orderDetail)
        {
            _db.orderDetails.Update(orderDetail);
        }
    }
}
