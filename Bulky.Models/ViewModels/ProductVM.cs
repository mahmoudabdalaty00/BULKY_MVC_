using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bulky.Models.ViewModels
{
    public class ProductVM
    {
        public Product product { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> listItems { get; set; }
        
        
        [ValidateNever]
        public IEnumerable<SelectListItem> StoreList { get; set; }

        public List<int> SelectedStoreIds { get; set; } = new List<int>();

        [ValidateNever]
        public List<StoreProductVM> StoreProducts { get; set; } = new List<StoreProductVM>();
    }

     
}
