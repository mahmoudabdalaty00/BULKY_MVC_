using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulky.Models.Models
{
    public class Product : BaseEntity
    {

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }

        [Required]
        [Display(Name = "List Price")]
        [Range(1, 10000)]
        public double ListPrice { get; set; }

        [Required]
        [Display(Name = "price for 1-50")]
        [Range(1, 10000)]
        public double price { get; set; }

        [Required]
        [Display(Name = "Price for 50+")]
        [Range(1, 10000)]
        public double price50 { get; set; }

        [Required]
        [Display(Name = "Price for 100+")]
        [Range(1, 10000)]
        public double price100 { get; set; }

        [ValidateNever]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        [ValidateNever]
        public Category Category { get; set; }


        [NotMapped]
        public bool IsHidden { get; set; } = false;


        [ValidateNever]
        public List<ProductImage> ProductImages { get; set; }

    }
}
