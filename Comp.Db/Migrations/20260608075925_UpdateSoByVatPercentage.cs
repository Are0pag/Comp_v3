using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comp.Db.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSoByVatPercentage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "VatPercentage",
                table: "SupplierOrders",
                type: "REAL",
                nullable: false,
                defaultValue: 22f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VatPercentage",
                table: "SupplierOrders");
        }
    }
}
