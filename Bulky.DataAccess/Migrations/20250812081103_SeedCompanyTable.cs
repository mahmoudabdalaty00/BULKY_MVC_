using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bulky.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedCompanyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "City", "Email", "Name", "PhoneNumber", "PostalCode", "State", "StreetAddress" },
                values: new object[,]
                {
                    { 1, "Springfield", "contact@acmesolutions.com", "Acme Solutions", "217-555-0101", "62701", "IL", "123 Maple St" },
                    { 2, "Denver", "info@brightfuturetech.com", "Bright Future Tech", "303-555-0202", "80202", "CO", "456 Oak Ave" },
                    { 3, "Austin", "support@nexusenterprises.com", "Nexus Enterprises", "512-555-0303", "78701", "TX", "789 Pine Rd" },
                    { 4, "Seattle", "hello@horizoninnovations.com", "Horizon Innovations", "206-555-0404", "98101", "WA", "101 Birch Ln" },
                    { 5, "Boston", "contact@pinnaclesystems.com", "Pinnacle Systems", "617-555-0505", "02108", "MA", "234 Cedar Dr" },
                    { 6, "Chicago", "info@summitconsulting.com", "Summit Consulting", "312-555-0606", "60601", "IL", "567 Elm St" },
                    { 7, "Portland", "support@vanguardsolutions.com", "Vanguard Solutions", "503-555-0707", "97201", "OR", "890 Walnut Ave" },
                    { 8, "Miami", "contact@apexdynamics.com", "Apex Dynamics", "305-555-0808", "33101", "FL", "321 Spruce Way" },
                    { 9, "Phoenix", "info@quantumventures.com", "Quantum Ventures", "602-555-0909", "85001", "AZ", "654 Sycamore St" },
                    { 10, "Atlanta", "hello@coreinnovations.com", "Core Innovations", "404-555-1010", "30301", "GA", "987 Magnolia Blvd" },
                    { 11, "Houston", "support@strivetech.com", "Strive Technologies", "713-555-1111", "77002", "TX", "147 Chestnut Dr" },
                    { 12, "San Francisco", "contact@optimagroup.com", "Optima Group", "415-555-1212", "94102", "CA", "258 Willow Ln" },
                    { 13, "New York", "info@nextgensolutions.com", "NextGen Solutions", "212-555-1313", "10001", "NY", "369 Laurel St" },
                    { 14, "Los Angeles", "support@eliteenterprises.com", "Elite Enterprises", "323-555-1414", "90001", "CA", "741 Poplar Ave" },
                    { 15, "Dallas", "hello@fusiondynamics.com", "Fusion Dynamics", "214-555-1515", "75201", "TX", "852 Ash Rd" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 15);
        }
    }
}
