using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models.Models
{
    public class StoreProduct
    {
      
        [Key, Column(Order = 0)]
        public int StoreId { get; set; } 
        [ForeignKey(nameof(StoreId))]
        public virtual Store Store { get; set; }

        [Key, Column(Order = 1)]
       
        public int ProductId { get; set; } 
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be non-negative")]
        public int StockQuantity { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }  

        public bool IsFeatured { get; set; } = false;

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be positive")]
        public decimal? StoreSpecificPrice { get; set; }
    }
}
