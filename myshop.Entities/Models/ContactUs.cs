using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Models
{
    public class ContactUs
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Full Name")]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Message { get; set; }


    }
}
