using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DepotContainer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDepotIdFromBlock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blocks",
                columns: table => new
                {
                    block_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    block_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    max_tiers = table.Column<int>(type: "int", nullable: false),
                    max_rows = table.Column<int>(type: "int", nullable: false),
                    max_bays = table.Column<int>(type: "int", nullable: false),
                    block_capacity = table.Column<int>(type: "int", nullable: false),
                    is_virtual = table.Column<bool>(type: "bit", nullable: false),
                    description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks", x => x.block_id);
                });

            migrationBuilder.CreateTable(
                name: "ContainerCategory",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    category_code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContainerCategory", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    cus_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    tax_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.cus_id);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    staff_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    staff_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    contact_phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    staff_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.staff_id);
                });

            migrationBuilder.CreateTable(
                name: "Slot",
                columns: table => new
                {
                    slot_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bay = table.Column<int>(type: "int", nullable: false),
                    row = table.Column<int>(type: "int", nullable: false),
                    tier = table.Column<int>(type: "int", nullable: false),
                    status_slot = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    block_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slot", x => x.slot_id);
                    table.ForeignKey(
                        name: "FK_Slot_Blocks_block_id",
                        column: x => x.block_id,
                        principalTable: "Blocks",
                        principalColumn: "block_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContainerISO",
                columns: table => new
                {
                    cont_iso_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iso_code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    length = table.Column<double>(type: "float", nullable: true),
                    height = table.Column<double>(type: "float", nullable: true),
                    width = table.Column<double>(type: "float", nullable: true),
                    maximum_weight = table.Column<double>(type: "float", nullable: true),
                    tare_weight = table.Column<double>(type: "float", nullable: true),
                    description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    category_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContainerISO", x => x.cont_iso_id);
                    table.ForeignKey(
                        name: "FK_ContainerISO_ContainerCategory_category_id",
                        column: x => x.category_id,
                        principalTable: "ContainerCategory",
                        principalColumn: "category_id");
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    booking_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    booking_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    cont_size = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    cont_quantity = table.Column<int>(type: "int", nullable: true),
                    operator_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    release_expire_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    cus_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.booking_id);
                    table.ForeignKey(
                        name: "FK_Booking_Customer_cus_id",
                        column: x => x.cus_id,
                        principalTable: "Customer",
                        principalColumn: "cus_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Container",
                columns: table => new
                {
                    cont_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cont_no = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    date_of_manufacture = table.Column<DateTime>(type: "datetime2", nullable: true),
                    operator_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    is_empty = table.Column<bool>(type: "bit", nullable: false),
                    weight = table.Column<double>(type: "float", nullable: true),
                    cont_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    time_in = table.Column<DateTime>(type: "datetime2", nullable: true),
                    time_out = table.Column<DateTime>(type: "datetime2", nullable: true),
                    cont_condition = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    cont_iso_id = table.Column<int>(type: "int", nullable: true),
                    slot_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Container", x => x.cont_id);
                    table.ForeignKey(
                        name: "FK_Container_ContainerISO_cont_iso_id",
                        column: x => x.cont_iso_id,
                        principalTable: "ContainerISO",
                        principalColumn: "cont_iso_id");
                    table.ForeignKey(
                        name: "FK_Container_Slot_slot_id",
                        column: x => x.slot_id,
                        principalTable: "Slot",
                        principalColumn: "slot_id");
                });

            migrationBuilder.CreateTable(
                name: "ContainerMovementHis",
                columns: table => new
                {
                    his_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    staff_id = table.Column<int>(type: "int", nullable: true),
                    slot_id = table.Column<int>(type: "int", nullable: true),
                    cont_id = table.Column<int>(type: "int", nullable: true),
                    move_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    status_his = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContainerMovementHis", x => x.his_id);
                    table.ForeignKey(
                        name: "FK_ContainerMovementHis_Container_cont_id",
                        column: x => x.cont_id,
                        principalTable: "Container",
                        principalColumn: "cont_id");
                    table.ForeignKey(
                        name: "FK_ContainerMovementHis_Slot_slot_id",
                        column: x => x.slot_id,
                        principalTable: "Slot",
                        principalColumn: "slot_id");
                    table.ForeignKey(
                        name: "FK_ContainerMovementHis_Staff_staff_id",
                        column: x => x.staff_id,
                        principalTable: "Staff",
                        principalColumn: "staff_id");
                });

            migrationBuilder.CreateTable(
                name: "Seal",
                columns: table => new
                {
                    seal_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    seal_no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    seal_owner = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    seal_applied_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    seal_removed_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    cont_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seal", x => x.seal_id);
                    table.ForeignKey(
                        name: "FK_Seal_Container_cont_id",
                        column: x => x.cont_id,
                        principalTable: "Container",
                        principalColumn: "cont_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EIR",
                columns: table => new
                {
                    eir_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EirNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EirType = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    regis_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    bat_no = table.Column<int>(type: "int", nullable: true),
                    staff_id = table.Column<int>(type: "int", nullable: true),
                    cus_id = table.Column<int>(type: "int", nullable: true),
                    booking_id = table.Column<int>(type: "int", nullable: true),
                    cont_id = table.Column<int>(type: "int", nullable: true),
                    plate_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    seal_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EIR", x => x.eir_id);
                    table.ForeignKey(
                        name: "FK_EIR_Booking_booking_id",
                        column: x => x.booking_id,
                        principalTable: "Booking",
                        principalColumn: "booking_id");
                    table.ForeignKey(
                        name: "FK_EIR_Container_cont_id",
                        column: x => x.cont_id,
                        principalTable: "Container",
                        principalColumn: "cont_id");
                    table.ForeignKey(
                        name: "FK_EIR_Customer_cus_id",
                        column: x => x.cus_id,
                        principalTable: "Customer",
                        principalColumn: "cus_id");
                    table.ForeignKey(
                        name: "FK_EIR_Seal_seal_id",
                        column: x => x.seal_id,
                        principalTable: "Seal",
                        principalColumn: "seal_id");
                    table.ForeignKey(
                        name: "FK_EIR_Staff_staff_id",
                        column: x => x.staff_id,
                        principalTable: "Staff",
                        principalColumn: "staff_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_cus_id",
                table: "Booking",
                column: "cus_id");

            migrationBuilder.CreateIndex(
                name: "IX_Container_cont_iso_id",
                table: "Container",
                column: "cont_iso_id");

            migrationBuilder.CreateIndex(
                name: "IX_Container_slot_id",
                table: "Container",
                column: "slot_id");

            migrationBuilder.CreateIndex(
                name: "IX_ContainerISO_category_id",
                table: "ContainerISO",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_ContainerMovementHis_cont_id",
                table: "ContainerMovementHis",
                column: "cont_id");

            migrationBuilder.CreateIndex(
                name: "IX_ContainerMovementHis_slot_id",
                table: "ContainerMovementHis",
                column: "slot_id");

            migrationBuilder.CreateIndex(
                name: "IX_ContainerMovementHis_staff_id",
                table: "ContainerMovementHis",
                column: "staff_id");

            migrationBuilder.CreateIndex(
                name: "IX_EIR_booking_id",
                table: "EIR",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "IX_EIR_cont_id",
                table: "EIR",
                column: "cont_id");

            migrationBuilder.CreateIndex(
                name: "IX_EIR_cus_id",
                table: "EIR",
                column: "cus_id");

            migrationBuilder.CreateIndex(
                name: "IX_EIR_seal_id",
                table: "EIR",
                column: "seal_id");

            migrationBuilder.CreateIndex(
                name: "IX_EIR_staff_id",
                table: "EIR",
                column: "staff_id");

            migrationBuilder.CreateIndex(
                name: "IX_Seal_cont_id",
                table: "Seal",
                column: "cont_id");

            migrationBuilder.CreateIndex(
                name: "IX_Slot_block_id",
                table: "Slot",
                column: "block_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContainerMovementHis");

            migrationBuilder.DropTable(
                name: "EIR");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Seal");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Container");

            migrationBuilder.DropTable(
                name: "ContainerISO");

            migrationBuilder.DropTable(
                name: "Slot");

            migrationBuilder.DropTable(
                name: "ContainerCategory");

            migrationBuilder.DropTable(
                name: "Blocks");
        }
    }
}
