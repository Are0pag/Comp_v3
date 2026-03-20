using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Comp.Db.Migrations
{
    /// <inheritdoc />
    public partial class ComplexSupplierOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderNumber",
                table: "SupplierOrders",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderedUnitsAmount",
                table: "SupplierOrders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "SupplierOrders",
                type: "TEXT",
                nullable: false,
                defaultValue: "NotPayed");

            migrationBuilder.AddColumn<decimal>(
                name: "PercentageOfTotalPayment",
                table: "SupplierOrders",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ReceiveStatus",
                table: "SupplierOrders",
                type: "TEXT",
                nullable: false,
                defaultValue: "NotReceived");

            migrationBuilder.AddColumn<int>(
                name: "ReceivedUnitsAmount",
                table: "SupplierOrders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalOrderCost",
                table: "SupplierOrders",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPayment",
                table: "SupplierOrders",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalVatAmount",
                table: "SupplierOrders",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "OrderPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PositionId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderQuantity = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    ReceivedQuantity = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    ReceiveStatus = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "NotReceived"),
                    UnitPrice = table.Column<decimal>(type: "TEXT", nullable: false, defaultValue: 0m),
                    TotalCost = table.Column<decimal>(type: "TEXT", nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderPositions_Components_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    PaymentPurpose = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentOrders_SupplierOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "SupplierOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderPositions_PositionId",
                table: "OrderPositions",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentOrders_OrderId",
                table: "PaymentOrders",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderPositions");

            migrationBuilder.DropTable(
                name: "PaymentOrders");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "SupplierOrders");

            migrationBuilder.DropColumn(
                name: "OrderedUnitsAmount",
                table: "SupplierOrders");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "SupplierOrders");

            migrationBuilder.DropColumn(
                name: "PercentageOfTotalPayment",
                table: "SupplierOrders");

            migrationBuilder.DropColumn(
                name: "ReceiveStatus",
                table: "SupplierOrders");

            migrationBuilder.DropColumn(
                name: "ReceivedUnitsAmount",
                table: "SupplierOrders");

            migrationBuilder.DropColumn(
                name: "TotalOrderCost",
                table: "SupplierOrders");

            migrationBuilder.DropColumn(
                name: "TotalPayment",
                table: "SupplierOrders");

            migrationBuilder.DropColumn(
                name: "TotalVatAmount",
                table: "SupplierOrders");
        }
    }
}
