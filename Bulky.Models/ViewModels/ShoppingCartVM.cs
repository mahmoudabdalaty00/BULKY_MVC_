using Bulky.Models.Models;

namespace Bulky.Models.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> shoppingCartsList { get; set; }
        public OrderHeader orderHeader { get; set; }
    }
}
