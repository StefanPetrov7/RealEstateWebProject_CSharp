using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationApplicationUserFavorite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFavorites_AspNetUsers_ApplicationUserId",
                table: "UserFavorites");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFavorites_Favorites_FavoriteId",
                table: "UserFavorites");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavorites_AspNetUsers_ApplicationUserId",
                table: "UserFavorites",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavorites_Favorites_FavoriteId",
                table: "UserFavorites",
                column: "FavoriteId",
                principalTable: "Favorites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFavorites_AspNetUsers_ApplicationUserId",
                table: "UserFavorites");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFavorites_Favorites_FavoriteId",
                table: "UserFavorites");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavorites_AspNetUsers_ApplicationUserId",
                table: "UserFavorites",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavorites_Favorites_FavoriteId",
                table: "UserFavorites",
                column: "FavoriteId",
                principalTable: "Favorites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
