using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models.ViewModels
{
    public class StoreVM
    {
        public Store Store { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> ProductList { get; set; }

        // Add this property to hold selected product IDs
        public List<int> SelectedProductIds { get; set; } = new List<int>();

        // Add this for product-specific data when editing
        [ValidateNever]
        public List<StoreProductVM> StoreProducts { get; set; } = new List<StoreProductVM>();
    }

    // Helper class for store-product specific data
    public class StoreProductVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int StockQuantity { get; set; }
        public decimal? StoreSpecificPrice { get; set; }
        public bool IsFeatured { get; set; }
    }
}
