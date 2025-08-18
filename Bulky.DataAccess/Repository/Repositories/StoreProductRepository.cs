using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;

namespace Bulky.DataAccess.Repository.Repositories
{
    public class StoreProductRepository : Repository<StoreProduct>, IStoreProductRepository
    {

        private readonly ApplicationDbContext _db;
        public StoreProductRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }

        public void AddProducttoStore(StoreProduct entry)
        {
            var exists =  _db.StoreProducts
                .Any(sp => sp.ProductId == entry.ProductId &&   sp.StoreId == entry.StoreId);

            if(!exists)
            {
                _db.StoreProducts.Add(entry);
            }
            else
            {
                throw new InvalidOperationException("Product already exists in this store.");
            }
        }

        public IEnumerable<StoreProduct> GetProductsByStore(int storeId)
        {
            var exists = _db.StoreProducts
                 .Where(sp => sp.StoreId == storeId)
                 .ToList();

            return exists;
        }

        public IEnumerable<StoreProduct> GetStoresByProduct(int productId)
        {
            var exists = _db.StoreProducts
                 .Where(sp => sp.ProductId == productId)
                 .ToList();

            return exists;
        }

        public void RemoveProductfromStore(StoreProduct entry)
        {
            var exists = _db.StoreProducts
                .Any(sp => sp.ProductId == entry.ProductId && sp.StoreId == entry.StoreId);

            if (!exists)
            {
                _db.StoreProducts.Remove(entry);
            }
            else
            {
                throw new InvalidOperationException("Product not exists in this store already.");
            }
        }

        public void Update(StoreProduct stPro)
        {
            var objFromDb = _db.StoreProducts.FirstOrDefault(sp =>
                sp.StoreId == stPro.StoreId && sp.ProductId == stPro.ProductId);

            if (objFromDb != null)
            {
                objFromDb.StockQuantity = stPro.StockQuantity;
                objFromDb.StoreSpecificPrice = stPro.StoreSpecificPrice;
                objFromDb.IsFeatured = stPro.IsFeatured;
                objFromDb.DateAdded = stPro.DateAdded;
                objFromDb.Store = stPro.Store;
                objFromDb.Product = stPro.Product;
            }

            _db.StoreProducts.Update(objFromDb);

        }
 
    
    }
}
