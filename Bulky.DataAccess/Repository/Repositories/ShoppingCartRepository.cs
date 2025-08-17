using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;

namespace Bulky.DataAccess.Repository.Repositories
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }



        public void Update(ShoppingCart cart)
        {
            if (cart == null)
            {
                throw new ArgumentNullException(nameof(cart), "Shopping cart cannot be null.");
            }

            var existingCart = _context.ShoppingCarts
                .FirstOrDefault(c => c.Id == cart.Id);

            if (existingCart == null)
            {
                throw new InvalidOperationException($"Shopping cart with ID {cart.Id} not found.");
            }

            // Update properties
            existingCart.Count = cart.Count;
            existingCart.ProductId = cart.ProductId;
            existingCart.ApplicationUserId = cart.ApplicationUserId;

            _context.ShoppingCarts.Update(existingCart);
        }












    }
}
