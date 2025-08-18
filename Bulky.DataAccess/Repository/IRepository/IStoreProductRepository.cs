using Bulky.Models.Models;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IStoreProductRepository : IRepository<StoreProduct>
    {
        void Update(StoreProduct StPro);        
        void AddProducttoStore(StoreProduct entry);
        void RemoveProductfromStore(StoreProduct entry);
        IEnumerable<StoreProduct> GetProductsByStore(int storeId);
        IEnumerable<StoreProduct> GetStoresByProduct(int productId);
    }
}
