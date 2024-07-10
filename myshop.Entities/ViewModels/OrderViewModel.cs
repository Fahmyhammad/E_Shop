﻿using myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.ViewModels
{
    public class OrderViewModel
    {
        public OrderHeader OrderHeader {  get; set; }
        public IEnumerable<OrderDetail> orderDetails { get; set; }

    }
}
