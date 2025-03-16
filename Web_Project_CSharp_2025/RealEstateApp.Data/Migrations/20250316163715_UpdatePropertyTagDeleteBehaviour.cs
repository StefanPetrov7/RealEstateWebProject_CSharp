using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePropertyTagDeleteBehaviour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyTags_Properties_PropertyId",
                table: "PropertyTags");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyTags_Tags_TagId",
                table: "PropertyTags");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyTags_Properties_PropertyId",
                table: "PropertyTags",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyTags_Tags_TagId",
                table: "PropertyTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyTags_Properties_PropertyId",
                table: "PropertyTags");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyTags_Tags_TagId",
                table: "PropertyTags");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyTags_Properties_PropertyId",
                table: "PropertyTags",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyTags_Tags_TagId",
                table: "PropertyTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
