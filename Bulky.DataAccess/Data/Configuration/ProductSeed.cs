using Bulky.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bulky.DataAccess.Data.Configuration
{
    public class ProductSeed : IEntityTypeConfiguration<Product>
    {
        

        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData( 
                new Product { Id = 1, Name = "C# in Depth", Description = "Advanced guide to C# programming.", ISBN = "9781617294532", Author = "Jon Skeet", ListPrice = 59.99, price = 54.99, price50 = 49.99, price100 = 44.99,CategoryId=2 },
                new Product { Id = 2, Name = "ASP.NET Core in Action", Description = "Comprehensive guide to building web apps with ASP.NET Core.", ISBN = "9781617294617", Author = "Andrew Lock", ListPrice = 64.99, price = 59.99, price50 = 54.99, price100 = 49.99, CategoryId = 2 },
                new Product { Id = 3, Name = "Clean Code", Description = "A Handbook of Agile Software Craftsmanship.", ISBN = "9780132350884", Author = "Robert C. Martin", ListPrice = 49.99, price = 44.99, price50 = 39.99, price100 = 34.99, CategoryId = 2 },
                new Product { Id = 4, Name = "Design Patterns", Description = "Elements of Reusable Object-Oriented Software.", ISBN = "9780201633610", Author = "Erich Gamma et al.", ListPrice = 54.99, price = 49.99, price50 = 44.99, price100 = 39.99, CategoryId = 2 },
                new Product { Id = 5, Name = "The Pragmatic Programmer", Description = "Your Journey to Mastery.", ISBN = "9780135957059", Author = "Andrew Hunt & David Thomas", ListPrice = 44.99, price = 39.99, price50 = 34.99, price100 = 29.99, CategoryId = 2 },
                new Product { Id = 6, Name = "Pro Entity Framework Core 7", Description = "Best practices and performance optimization for EF Core.", ISBN = "9781484287486", Author = "Adam Freeman", ListPrice = 69.99, price = 64.99, price50 = 59.99, price100 = 54.99, CategoryId = 2 },
                new Product { Id = 7, Name = "JavaScript: The Good Parts", Description = "Deep dive into the best features of JavaScript.", ISBN = "9780596517748", Author = "Douglas Crockford", ListPrice = 39.99, price = 34.99, price50 = 29.99, price100 = 24.99, CategoryId = 2 },
                new Product { Id = 8, Name = "You Don’t Know JS Yet", Description = "Understanding JavaScript deeply.", ISBN = "9781098124045", Author = "Kyle Simpson", ListPrice = 34.99, price = 29.99, price50 = 24.99, price100 = 19.99, CategoryId = 2 },
                new Product { Id = 9, Name = "Learning SQL", Description = "Master SQL for data management and analysis.", ISBN = "9781492057611", Author = "Alan Beaulieu", ListPrice = 49.99, price = 44.99, price50 = 39.99, price100 = 34.99, CategoryId = 2 },
                new Product { Id = 10, Name = "Docker Deep Dive", Description = "Comprehensive Docker reference.", ISBN = "9781521822807", Author = "Nigel Poulton", ListPrice = 54.99, price = 49.99, price50 = 44.99, price100 = 39.99, CategoryId = 2 },
                new Product { Id = 11, Name = "Microservices Patterns", Description = "With examples in Java and Spring.", ISBN = "9781617294549", Author = "Chris Richardson", ListPrice = 64.99, price = 59.99, price50 = 54.99, price100 = 49.99, CategoryId = 2 },
                new Product { Id = 12, Name = "Head First Design Patterns", Description = "A brain-friendly guide to design patterns.", ISBN = "9781492078005", Author = "Eric Freeman", ListPrice = 59.99, price = 54.99, price50 = 49.99, price100 = 44.99, CategoryId = 2 },
                new Product { Id = 13, Name = "Kubernetes Up & Running", Description = "Dive into Kubernetes cluster management.", ISBN = "9781492046530", Author = "Brendan Burns", ListPrice = 64.99, price = 59.99, price50 = 54.99, price100 = 49.99, CategoryId = 2 },
                new Product { Id = 14, Name = "Python Crash Course", Description = "A hands-on introduction to programming.", ISBN = "9781593279288", Author = "Eric Matthes", ListPrice = 44.99, price = 39.99, price50 = 34.99, price100 = 29.99, CategoryId = 2 },
                new Product { Id = 15, Name = "Fluent Python", Description = "Clear, concise, and effective programming in Python.", ISBN = "9781492056355", Author = "Luciano Ramalho", ListPrice = 64.99, price = 59.99, price50 = 54.99, price100 = 49.99, CategoryId = 2 },
                new Product { Id = 16, Name = "Refactoring", Description = "Improving the design of existing code.", ISBN = "9780134757599", Author = "Martin Fowler", ListPrice = 69.99, price = 64.99, price50 = 59.99, price100 = 54.99, CategoryId = 2 },
                new Product { Id = 17, Name = "Domain-Driven Design", Description = "Tackling complexity in the heart of software.", ISBN = "9780321125217", Author = "Eric Evans", ListPrice = 74.99, price = 69.99, price50 = 64.99, price100 = 59.99, CategoryId = 2 },
                new Product { Id = 18, Name = "Agile Estimating and Planning", Description = "Mastering Agile project estimation.", ISBN = "9780131479418", Author = "Mike Cohn", ListPrice = 49.99, price = 44.99, price50 = 39.99, price100 = 34.99, CategoryId = 2 },
                new Product { Id = 19, Name = "Continuous Delivery", Description = "Reliable software releases through automation.", ISBN = "9780321601919", Author = "Jez Humble", ListPrice = 64.99, price = 59.99, price50 = 54.99, price100 = 49.99, CategoryId = 2 },
                new Product { Id = 20, Name = "Site Reliability Engineering", Description = "How Google runs production systems.", ISBN = "9781491929124", Author = "Betsy Beyer", ListPrice = 74.99, price = 69.99, price50 = 64.99, price100 = 59.99, CategoryId = 2 }

);
        }
    }
}
