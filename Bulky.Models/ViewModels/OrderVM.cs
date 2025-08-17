using Bulky.Models.Models;

namespace Bulky.Models.ViewModels
{
    public class OrderVM
    {
        public OrderHeader orderHeader { get; set; }
        public IEnumerable<OrderDetail> orderDetail { get; set; }
    }
}
