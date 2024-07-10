using Microsoft.EntityFrameworkCore;
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
    public class OrderHeaderRepoistory : GenericRepoistory<OrderHeader>,IOrderHeaderRepository
    {
        private AppDbContext _db;
        public OrderHeaderRepoistory(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public List<OrderHeader> Search(string? entity)
        {
            return _db.orderHeaders.Include(x=>x.AppUser).Where(x=>x.Name.ToUpper().Contains(entity) || x.OrderStatus.ToUpper().Contains(entity) ||x.PhoneNumber.ToUpper().Contains(entity) || x.City.ToUpper().Contains(entity)).ToList();

        }

        public void UpDate(OrderHeader orderHeader)
        {
           _db.orderHeaders.Update(orderHeader);
        }

        public void UpdateOrderStatus(int id, string OrderStatus, string PaymentStatus)
        {
            var orderFromDb = _db.orderHeaders.FirstOrDefault(x => x.Id == id);
            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = OrderStatus;
                orderFromDb.PaymentDate = DateTime.Now;
                if(PaymentStatus != null)
                {
                    orderFromDb.PaymentStatus = PaymentStatus;
                }
            }
        }
    }
}
