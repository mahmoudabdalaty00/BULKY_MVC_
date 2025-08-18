using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;

namespace Bulky.DataAccess.Repository.Repositories
{
    public class StoreRepository : Repository<Store>, IStoreRepository
    {
        private readonly ApplicationDbContext _context;
        public StoreRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Store> GetStoresWithProducts()
        {
          var StPro = _context.Stores
               .Where(s => s.IsActive)
               .Select(s => new Store
               {
                   Id = s.Id,
                   Name = s.Name,                  
                   StoreProducts = s.StoreProducts
               }).ToList();


            return StPro;
        }

        public void Update(Store store)
        {
            var objFromDb = _context.Stores.
                FirstOrDefault(s => s.Id == store.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = store.Name;
                objFromDb.Description = store.Description;
                objFromDb.Address = store.Address;

                objFromDb.Country = store.Country;
                objFromDb.City = store.City;
                objFromDb.State = store.State;
                objFromDb.PostalCode = store.PostalCode;

                objFromDb.PhoneNumber = store.PhoneNumber;
                objFromDb.Email = store.Email;
                objFromDb.IsActive = store.IsActive;



                objFromDb.StoreProducts = store.StoreProducts;

                objFromDb.UpdatedAt = DateTime.UtcNow;

            }                                 

                _context.Stores.Update(objFromDb);
            }
        }
    }
 
