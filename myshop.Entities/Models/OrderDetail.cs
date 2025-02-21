﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        public int OrderHeaderId { get; set; }  
        [ValidateNever]
        [NotMapped]
        public OrderHeader OrderHeader { get; set; }

        public int ProductId {  get; set; }
        [ValidateNever]
        public Product Product { get; set; }

        public int Count {  get; set; }

        public decimal Price {  get; set; }
    }
}
