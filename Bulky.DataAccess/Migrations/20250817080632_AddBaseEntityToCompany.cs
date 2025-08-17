using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bulky.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseEntityToCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Companies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Companies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Companies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "CreatedBy", "DisplayOrder", "IsHidden", "UpdatedAt", "UpdatedBy" },
                values: new object[] { null, null, 0, false, null, null });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "CreatedBy", "DisplayOrder", "IsHidden", "UpdatedAt", "UpdatedBy" },
                values: new object[] { null, null, 0, false, null, null });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "CreatedBy", "DisplayOrder", "IsHidden", "UpdatedAt", "UpdatedBy" },
                values: new object[] { null, null, 0, false, null, null });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "CreatedBy", "DisplayOrder", "IsHidden", "UpdatedAt", "UpdatedBy" },
                values: new object[] { null, null, 0, false, null, null });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "CreatedBy", "DisplayOrder", "IsHidden", "UpdatedAt", "UpdatedBy" },
                values: new object[] { null, null, 0, false, null, null });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "CreatedBy", "DisplayOrder", "IsHidden", "UpdatedAt", "UpdatedBy" },
                values: new object[] { null, null, 0, false, null, null });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "CreatedBy", "DisplayOrder", "IsHidden", "UpdatedAt", "UpdatedBy" },
                values: new object[] { null, null, 0, false, null, null });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "CreatedBy", "DisplayOrder", "IsHidden", "UpdatedAt", "UpdatedBy" },
                values: new object[] { null, null, 0, false, null, null });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "CreatedBy", "DisplayOrder", "IsHidden", "UpdatedAt", "UpdatedBy" },
                values: new object[] { null, null, 0, false, null, null });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "CreatedBy", "DisplayOrder", "IsHidden", "UpdatedAt", "UpdatedBy" },
                values: new object[] { null, null, 0, false, null, null });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "CreatedBy", "DisplayOrder", "IsHidden", "UpdatedAt", "UpdatedBy" },
                values: new object[] { null, null, 0, false, null, null });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "CreatedBy", "DisplayOrder", "IsHidden", "UpdatedAt", "UpdatedBy" },
                values: new object[] { null, null, 0, false, null, null });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "CreatedBy", "DisplayOrder", "IsHidden", "UpdatedAt", "UpdatedBy" },
                values: new object[] { null, null, 0, false, null, null });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "CreatedBy", "DisplayOrder", "IsHidden", "UpdatedAt", "UpdatedBy" },
                values: new object[] { null, null, 0, false, null, null });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "CreatedBy", "DisplayOrder", "IsHidden", "UpdatedAt", "UpdatedBy" },
                values: new object[] { null, null, 0, false, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Companies");
        }
    }
}
