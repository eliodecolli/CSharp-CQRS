using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeeGees_WriteNode.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Write_Shipments",
                columns: table => new
                {
                    ShipmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShipmentAddress = table.Column<string>(type: "text", nullable: false),
                    ShipmentName = table.Column<string>(type: "text", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeliveredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CurrentStatus = table.Column<string>(type: "text", nullable: false),
                    CurrentLocation = table.Column<string>(type: "text", nullable: false),
                    CustomerId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Write_Shipments", x => x.ShipmentId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Write_Shipments");
        }
    }
}
