using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comp.Db.Migrations
{
    /// <inheritdoc />
    public partial class SupplierOrdersEnumDescriptors2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "PaymentStatusEnumValue",
                table: "SupplierOrders",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentStatusEnumValue",
                table: "SupplierOrders");
        }
    }
}
