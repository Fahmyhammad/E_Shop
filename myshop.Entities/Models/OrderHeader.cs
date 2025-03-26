using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Models
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }
        public string AppUserId { get; set; }
        [ValidateNever]
        public AppUser AppUser { get; set; }
        [ValidateNever]
        public Product Product { get; set; }
        public int? ProductId { get; set; }
        public DateTime OrderDate {  get; set; }

        public DateTime ShoppingDate { get; set; }

        public decimal TotalPrice {  get; set; }
        public string? OrderStatus {  get; set; }
        public string? PaymentStatus {  get; set; }
        public string? TrackingNumber {  get; set; }
        public string? Carrier {  get; set; }
        public DateTime PaymentDate { get; set; }

        
        public string? SessionId { get;set; }
        public string? PaymentIntentId { get; set;}

        //Data Of User
        public string Name {  get; set; }
        public string Adderss {  get; set; }
        public string City {  get; set; }
        public string PhoneNumber {  get; set; }
    }
}
