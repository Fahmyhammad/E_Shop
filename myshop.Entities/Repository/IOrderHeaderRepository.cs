using myshop.Entities.Models;
using myshop.myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Repository
{
    public interface IOrderHeaderRepository : IGenericRepoistory<OrderHeader>
    {
        void UpDate(OrderHeader orderHeader);
        void UpdateOrderStatus(int id, string OrderStatus, string PaymentStatus);
        List<OrderHeader> Search(string? entity);

    }
}
