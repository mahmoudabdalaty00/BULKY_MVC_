using Bulky.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bulky.DataAccess.Data.Configuration
{
    public class CompanySeed : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasData(
                new Company { Id = 1, Name = "Acme Solutions", StreetAddress = "123 Maple St", City = "Springfield", State = "IL", PostalCode = "62701", PhoneNumber = "217-555-0101", Email = "contact@acmesolutions.com" },
            new Company { Id = 2, Name = "Bright Future Tech", StreetAddress = "456 Oak Ave", City = "Denver", State = "CO", PostalCode = "80202", PhoneNumber = "303-555-0202", Email = "info@brightfuturetech.com" },
            new Company { Id = 3, Name = "Nexus Enterprises", StreetAddress = "789 Pine Rd", City = "Austin", State = "TX", PostalCode = "78701", PhoneNumber = "512-555-0303", Email = "support@nexusenterprises.com" },
            new Company { Id = 4, Name = "Horizon Innovations", StreetAddress = "101 Birch Ln", City = "Seattle", State = "WA", PostalCode = "98101", PhoneNumber = "206-555-0404", Email = "hello@horizoninnovations.com" },
            new Company { Id = 5, Name = "Pinnacle Systems", StreetAddress = "234 Cedar Dr", City = "Boston", State = "MA", PostalCode = "02108", PhoneNumber = "617-555-0505", Email = "contact@pinnaclesystems.com" },
            new Company { Id = 6, Name = "Summit Consulting", StreetAddress = "567 Elm St", City = "Chicago", State = "IL", PostalCode = "60601", PhoneNumber = "312-555-0606", Email = "info@summitconsulting.com" },
            new Company { Id = 7, Name = "Vanguard Solutions", StreetAddress = "890 Walnut Ave", City = "Portland", State = "OR", PostalCode = "97201", PhoneNumber = "503-555-0707", Email = "support@vanguardsolutions.com" },
            new Company { Id = 8, Name = "Apex Dynamics", StreetAddress = "321 Spruce Way", City = "Miami", State = "FL", PostalCode = "33101", PhoneNumber = "305-555-0808", Email = "contact@apexdynamics.com" },
            new Company { Id = 9, Name = "Quantum Ventures", StreetAddress = "654 Sycamore St", City = "Phoenix", State = "AZ", PostalCode = "85001", PhoneNumber = "602-555-0909", Email = "info@quantumventures.com" },
            new Company { Id = 10, Name = "Core Innovations", StreetAddress = "987 Magnolia Blvd", City = "Atlanta", State = "GA", PostalCode = "30301", PhoneNumber = "404-555-1010", Email = "hello@coreinnovations.com" },
            new Company { Id = 11, Name = "Strive Technologies", StreetAddress = "147 Chestnut Dr", City = "Houston", State = "TX", PostalCode = "77002", PhoneNumber = "713-555-1111", Email = "support@strivetech.com" },
            new Company { Id = 12, Name = "Optima Group", StreetAddress = "258 Willow Ln", City = "San Francisco", State = "CA", PostalCode = "94102", PhoneNumber = "415-555-1212", Email = "contact@optimagroup.com" },
            new Company { Id = 13, Name = "NextGen Solutions", StreetAddress = "369 Laurel St", City = "New York", State = "NY", PostalCode = "10001", PhoneNumber = "212-555-1313", Email = "info@nextgensolutions.com" },
            new Company { Id = 14, Name = "Elite Enterprises", StreetAddress = "741 Poplar Ave", City = "Los Angeles", State = "CA", PostalCode = "90001", PhoneNumber = "323-555-1414", Email = "support@eliteenterprises.com" },
            new Company { Id = 15, Name = "Fusion Dynamics", StreetAddress = "852 Ash Rd", City = "Dallas", State = "TX", PostalCode = "75201", PhoneNumber = "214-555-1515", Email = "hello@fusiondynamics.com" });
        }
    }

}
