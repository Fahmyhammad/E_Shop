﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using myshop.myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Models
{
    public class Product
    {
       
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [ValidateNever]
        public string Image { get; set; }
        [Required]
        public decimal Price {  get; set; }
        public decimal? BeforeDiscount {  get; set; }
        public decimal? Offer {  get; set; }

        [Required]
        [DisplayName("Category")]
        public int CategoryId {  get; set; }
       
        public Category Category { get; set; }

    }
}
