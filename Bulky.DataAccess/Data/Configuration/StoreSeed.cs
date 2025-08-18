using Bulky.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bulky.DataAccess.Data.Configuration
{
    public class StoreSeed : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.HasData(
                 new Store
                 {
                     Id = 1,
                     Name = "Main Electronics Store",
                     Description = "Specialized in computers and accessories",
                     Address = "123 Tech Street",
                     City = "Cairo",
                     State = "Giza",
                     Country = "Egypt",
                     PostalCode = "12511",
                     PhoneNumber = "01012345678",
                     Email = "main@store.com",
                     CreatedAt = new DateTime(2025, 8, 18, 12, 0, 0)  ,
                     IsActive = true
                 },
                        new Store
                        {
                            Id = 2,
                            Name = "Mobile World",
                            Description = "Smartphones and gadgets",
                            Address = "456 Mobile Avenue",
                            City = "Giza",
                            State = "Giza",
                            Country = "Egypt",
                            PostalCode = "12622",
                            PhoneNumber = "01098765432",
                            Email = "mobile@store.com",
                            CreatedAt = new DateTime(2025, 8, 18, 12, 0, 0)   ,
                            IsActive = true
                        },
                        new Store
                        {
                            Id = 3,
                            Name = "Appliance Center",
                            Description = "Home appliances and electronics",
                            Address = "789 Home Road",
                            City = "Alexandria",
                            State = "Alexandria",
                            Country = "Egypt",
                            PostalCode = "21511",
                            PhoneNumber = "01055554444",
                            Email = "appliance@store.com",
                            CreatedAt = new DateTime(2025, 8, 18, 12, 0, 0),
                            IsActive = true
                        }
                         );
        }
    }
}
