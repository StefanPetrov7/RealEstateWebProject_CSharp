using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RealEstateApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Importance", "Name" },
                values: new object[,]
                {
                    { new Guid("25a30cf6-43fe-4cbe-9f35-f71d40eba0d3"), null, "Floor" },
                    { new Guid("9b0b4ad3-4069-45a4-b5e6-74ae05ba9295"), null, "Expensive" },
                    { new Guid("afb72774-282e-4c0a-9f5d-5b604776e124"), null, "Constructed" },
                    { new Guid("f4402520-89c6-4dcc-a2dc-2e4d25572188"), null, "Size" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("25a30cf6-43fe-4cbe-9f35-f71d40eba0d3"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("9b0b4ad3-4069-45a4-b5e6-74ae05ba9295"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("afb72774-282e-4c0a-9f5d-5b604776e124"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("f4402520-89c6-4dcc-a2dc-2e4d25572188"));
        }
    }
}
