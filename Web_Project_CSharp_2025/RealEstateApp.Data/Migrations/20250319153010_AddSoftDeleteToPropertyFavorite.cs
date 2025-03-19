using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToPropertyFavorite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PropertyFavorites",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PropertyFavorites");
        }
    }
}
