using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bulky.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class INtiProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListPrice = table.Column<double>(type: "float", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    price50 = table.Column<double>(type: "float", nullable: false),
                    price100 = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "Description", "ISBN", "ListPrice", "Name", "price", "price100", "price50" },
                values: new object[,]
                {
                    { 1, "Jon Skeet", "Advanced guide to C# programming.", "9781617294532", 59.990000000000002, "C# in Depth", 54.990000000000002, 44.990000000000002, 49.990000000000002 },
                    { 2, "Andrew Lock", "Comprehensive guide to building web apps with ASP.NET Core.", "9781617294617", 64.989999999999995, "ASP.NET Core in Action", 59.990000000000002, 49.990000000000002, 54.990000000000002 },
                    { 3, "Robert C. Martin", "A Handbook of Agile Software Craftsmanship.", "9780132350884", 49.990000000000002, "Clean Code", 44.990000000000002, 34.990000000000002, 39.990000000000002 },
                    { 4, "Erich Gamma et al.", "Elements of Reusable Object-Oriented Software.", "9780201633610", 54.990000000000002, "Design Patterns", 49.990000000000002, 39.990000000000002, 44.990000000000002 },
                    { 5, "Andrew Hunt & David Thomas", "Your Journey to Mastery.", "9780135957059", 44.990000000000002, "The Pragmatic Programmer", 39.990000000000002, 29.989999999999998, 34.990000000000002 },
                    { 6, "Adam Freeman", "Best practices and performance optimization for EF Core.", "9781484287486", 69.989999999999995, "Pro Entity Framework Core 7", 64.989999999999995, 54.990000000000002, 59.990000000000002 },
                    { 7, "Douglas Crockford", "Deep dive into the best features of JavaScript.", "9780596517748", 39.990000000000002, "JavaScript: The Good Parts", 34.990000000000002, 24.989999999999998, 29.989999999999998 },
                    { 8, "Kyle Simpson", "Understanding JavaScript deeply.", "9781098124045", 34.990000000000002, "You Don’t Know JS Yet", 29.989999999999998, 19.989999999999998, 24.989999999999998 },
                    { 9, "Alan Beaulieu", "Master SQL for data management and analysis.", "9781492057611", 49.990000000000002, "Learning SQL", 44.990000000000002, 34.990000000000002, 39.990000000000002 },
                    { 10, "Nigel Poulton", "Comprehensive Docker reference.", "9781521822807", 54.990000000000002, "Docker Deep Dive", 49.990000000000002, 39.990000000000002, 44.990000000000002 },
                    { 11, "Chris Richardson", "With examples in Java and Spring.", "9781617294549", 64.989999999999995, "Microservices Patterns", 59.990000000000002, 49.990000000000002, 54.990000000000002 },
                    { 12, "Eric Freeman", "A brain-friendly guide to design patterns.", "9781492078005", 59.990000000000002, "Head First Design Patterns", 54.990000000000002, 44.990000000000002, 49.990000000000002 },
                    { 13, "Brendan Burns", "Dive into Kubernetes cluster management.", "9781492046530", 64.989999999999995, "Kubernetes Up & Running", 59.990000000000002, 49.990000000000002, 54.990000000000002 },
                    { 14, "Eric Matthes", "A hands-on introduction to programming.", "9781593279288", 44.990000000000002, "Python Crash Course", 39.990000000000002, 29.989999999999998, 34.990000000000002 },
                    { 15, "Luciano Ramalho", "Clear, concise, and effective programming in Python.", "9781492056355", 64.989999999999995, "Fluent Python", 59.990000000000002, 49.990000000000002, 54.990000000000002 },
                    { 16, "Martin Fowler", "Improving the design of existing code.", "9780134757599", 69.989999999999995, "Refactoring", 64.989999999999995, 54.990000000000002, 59.990000000000002 },
                    { 17, "Eric Evans", "Tackling complexity in the heart of software.", "9780321125217", 74.989999999999995, "Domain-Driven Design", 69.989999999999995, 59.990000000000002, 64.989999999999995 },
                    { 18, "Mike Cohn", "Mastering Agile project estimation.", "9780131479418", 49.990000000000002, "Agile Estimating and Planning", 44.990000000000002, 34.990000000000002, 39.990000000000002 },
                    { 19, "Jez Humble", "Reliable software releases through automation.", "9780321601919", 64.989999999999995, "Continuous Delivery", 59.990000000000002, 49.990000000000002, 54.990000000000002 },
                    { 20, "Betsy Beyer", "How Google runs production systems.", "9781491929124", 74.989999999999995, "Site Reliability Engineering", 69.989999999999995, 59.990000000000002, 64.989999999999995 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
