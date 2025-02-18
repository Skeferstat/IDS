using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IdsServer.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Basket",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RawBasket = table.Column<string>(type: "TEXT", maxLength: 50000, nullable: false),
                    HookUrl = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    LastUpdate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Basket", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FakeArticle",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ArticleNumber = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    NetPrice = table.Column<decimal>(type: "TEXT", precision: 2, nullable: false),
                    OfferPrice = table.Column<decimal>(type: "TEXT", precision: 2, nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FakeArticle", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "FakeArticle",
                columns: new[] { "Id", "ArticleNumber", "Description", "ImageUrl", "Name", "NetPrice", "OfferPrice" },
                values: new object[,]
                {
                    { new Guid("c3e89e64-9f00-4dc0-8adf-55223601d9f6"), "24485918350056", "Bresaola picanha landjaeger prosciutto ball tip burgdoggen pork belly. Swine ham hock salami spare ribs turducken venison tri-tip biltong t-bone prosciutto jowl beef ribs turkey beef shank. Hamburger picanha strip steak, pork chop buffalo tenderloin corned beef ball tip jowl sirloin bacon pork. Ribeye kevin shankle, tongue short loin beef ribs pancetta tenderloin tri-tip short ribs. Strip steak brisket rump, boudin ham landjaeger chuck spare ribs.", "https://placehold.co/500x400", "Bresaola picanha landjaeger", 100.12m, 123.00m },
                    { new Guid("fd7aed7f-7ef6-409b-a34f-da22a663aae5"), "1085922350000", "Bacon ipsum dolor amet filet mignon shank drumstick sausage, cow sirloin rump corned beef tongue chicken kevin. Salami ham corned beef, kevin cow fatback bresaola sausage jerky hamburger tail capicola. Biltong pork jerky fatback venison pig. Jowl strip steak short ribs ham spare ribs, swine jerky pork belly frankfurter alcatra pork picanha boudin tongue beef. Doner meatloaf ribeye prosciutto tri-tip capicola. Landjaeger ham hock swine corned beef ground round tail biltong salami pastrami pork belly. Jerky shank landjaeger biltong, doner porchetta chicken pancetta shankle shoulder sausage pork chop corned beef burgdoggen.", "https://placehold.co/500x400", "filet mignon shank drumstick", 200.67m, 237.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Basket");

            migrationBuilder.DropTable(
                name: "FakeArticle");
        }
    }
}
