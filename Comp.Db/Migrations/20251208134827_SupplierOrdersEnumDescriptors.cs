using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comp.Db.Migrations
{
    /// <inheritdoc />
    public partial class SupplierOrdersEnumDescriptors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "OrderStatusEnumValue",
                table: "SupplierOrders",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "VatStatusEnumValue",
                table: "SupplierOrders",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderStatusEnumValue",
                table: "SupplierOrders");

            migrationBuilder.DropColumn(
                name: "VatStatusEnumValue",
                table: "SupplierOrders");
        }
    }
}
