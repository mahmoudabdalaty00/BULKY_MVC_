using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulky.Models.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int orderHeaderId { get; set; }
        [ForeignKey(nameof(orderHeaderId))]
        [ValidateNever]
        public OrderHeader orderHeader { get; set; }


        [Required]
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        [ValidateNever]
        public Product  Product { get; set; }


        public int count { get; set; }

        public double Price { get; set; }
    }
}
