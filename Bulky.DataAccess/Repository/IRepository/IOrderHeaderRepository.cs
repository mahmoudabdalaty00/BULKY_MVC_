using Bulky.Models.Models;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader header);
        void UpdateStatus(int id , string orderStatus , string? paymentStatus = null);
        void UpdateStripePayment(int id , string sessionId , string paymentIntentId );
    }
}
