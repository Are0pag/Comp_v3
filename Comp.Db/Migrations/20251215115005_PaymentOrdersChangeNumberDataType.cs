using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comp.Db.Migrations
{
    /// <inheritdoc />
    public partial class PaymentOrdersChangeNumberDataType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "PaymentOrders",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "PaymentOrders",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
