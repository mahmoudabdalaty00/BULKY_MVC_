using BULKYWEB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BULKYWEB.Data.Configuration
{
    public class CategorySeed : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
       new Category { Id = 1, Name = "Electronics", DisplayOrder = 1 },
            new Category { Id = 2, Name = "Books", DisplayOrder = 2 },
            new Category { Id = 3, Name = "Clothing", DisplayOrder = 3 },
            new Category { Id = 4, Name = "Home & Kitchen", DisplayOrder = 4 },
            new Category { Id = 5, Name = "Toys", DisplayOrder = 5 },
            new Category { Id = 6, Name = "Sports", DisplayOrder = 6 },
            new Category { Id = 7, Name = "Health", DisplayOrder = 7 },
            new Category { Id = 8, Name = "Beauty", DisplayOrder = 8 },
            new Category { Id = 9, Name = "Automotive", DisplayOrder = 9 },
            new Category { Id = 10, Name = "Music", DisplayOrder = 10 });
      
        }
    }
}
