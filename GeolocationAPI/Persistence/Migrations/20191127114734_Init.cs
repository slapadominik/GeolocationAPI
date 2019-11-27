using Microsoft.EntityFrameworkCore.Migrations;

namespace GeolocationAPI.Persistence.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeolocationData",
                columns: table => new
                {
                    IpAddress = table.Column<string>(nullable: false),
                    IpAddressType = table.Column<string>(nullable: true),
                    ContinentCode = table.Column<string>(maxLength: 2, nullable: true),
                    ContinentName = table.Column<string>(nullable: true),
                    CountryCode = table.Column<string>(maxLength: 2, nullable: true),
                    CountryName = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    RegionCode = table.Column<string>(maxLength: 10, nullable: true),
                    RegionName = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    Longitude = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeolocationData", x => x.IpAddress);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeolocationData");
        }
    }
}
