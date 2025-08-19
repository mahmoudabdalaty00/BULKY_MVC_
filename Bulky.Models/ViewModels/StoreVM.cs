using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

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

}
