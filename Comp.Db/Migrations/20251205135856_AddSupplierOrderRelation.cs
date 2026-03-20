using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comp.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddSupplierOrderRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.Sql("DELETE FROM OrderPositions;");
            
            migrationBuilder.AddColumn<int>(
                name: "SupplierOrderId",
                table: "OrderPositions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderPositions_SupplierOrderId",
                table: "OrderPositions",
                column: "SupplierOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderPositions_SupplierOrders_SupplierOrderId",
                table: "OrderPositions",
                column: "SupplierOrderId",
                principalTable: "SupplierOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderPositions_SupplierOrders_SupplierOrderId",
                table: "OrderPositions");

            migrationBuilder.DropIndex(
                name: "IX_OrderPositions_SupplierOrderId",
                table: "OrderPositions");

            migrationBuilder.DropColumn(
                name: "SupplierOrderId",
                table: "OrderPositions");
        }
    }
}
