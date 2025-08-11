using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Product product)
        {
            var pro = _context.Products.Find(product.Id);

            if (pro != null)
            {
                // Update properties individually to avoid replacing the tracked entity reference
                pro.Name = product.Name;
                pro.Description = product.Description;
                pro.ISBN = product.ISBN;
                pro.Author = product.Author;
                pro.ListPrice = product.ListPrice;
                pro.price = product.price;
                pro.price50 = product.price50;
                pro.price100 = product.price100;
                pro.CategoryId = product.CategoryId;

                if (product.ImageUrl != null)
                {
                    pro.ImageUrl = product.ImageUrl;
                }
                _context.Products.Update(pro);
                _context.SaveChanges();
            }
        }
    }
}
