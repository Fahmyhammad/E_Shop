using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Models
{
    public class ShoppingCardModel
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
        [Range(1, 100, ErrorMessage = "Only 1 to 100 can be entered")]
        public int Count { get; set; }

        public string AppUserId {  get; set; }
        [ForeignKey("AppUserId")]
        [ValidateNever]
        public AppUser appUser {  get; set; }

    }
}
