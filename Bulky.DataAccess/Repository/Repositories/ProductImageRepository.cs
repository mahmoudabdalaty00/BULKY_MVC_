using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.Repositories
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
         private readonly ApplicationDbContext _db;

        public ProductImageRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }

        public void Update(ProductImage productImage)
        {
            _db.ProductImages.Update(productImage);
        }
    }

}
