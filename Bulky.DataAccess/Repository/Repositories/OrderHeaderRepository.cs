using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;

namespace Bulky.DataAccess.Repository.Repositories
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }



        public void Update(OrderHeader orderHeader)
        {

            _context.OrderHeaders.Update(orderHeader);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDb = _context.OrderHeaders
                     .FirstOrDefault(x => x.Id == id);

            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if (!string.IsNullOrWhiteSpace(paymentStatus))
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }


        }

        public void UpdateStripePayment(int id, string sessionId, string paymentIntentId)
        {
            var orderFromDb = _context.OrderHeaders
                                 .FirstOrDefault(x => x.Id == id);

            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                orderFromDb.SessionId = sessionId;
            }

            if (!string.IsNullOrWhiteSpace(paymentIntentId))
            {
                orderFromDb.PaymentIntentId = paymentIntentId;
                orderFromDb.PaymentDate = DateTime.UtcNow;
            }



        }
    }


}
