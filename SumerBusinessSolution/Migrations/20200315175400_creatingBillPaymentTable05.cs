using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SumerBusinessSolution.Migrations
{
    public partial class creatingBillPaymentTable05 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TempProdImg",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImgFile = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempProdImg", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WhType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(nullable: false),
                    ContactName = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    PhoneNo = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProdInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProdCode = table.Column<string>(nullable: false),
                    ProdName = table.Column<string>(nullable: false),
                    ProdDescription = table.Column<string>(nullable: true),
                    ImgFile = table.Column<string>(nullable: true),
                    ProdCategory = table.Column<string>(nullable: true),
                    RetailPrice = table.Column<double>(nullable: false),
                    WholePrice = table.Column<double>(nullable: false),
                    CreatedById = table.Column<string>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdInfo_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Warehouse",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WhCode = table.Column<string>(nullable: false),
                    WhName = table.Column<string>(nullable: false),
                    WhLocation = table.Column<string>(nullable: true),
                    TypeId = table.Column<int>(nullable: false),
                    CreatedById = table.Column<string>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Warehouse_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Warehouse_WhType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "WhType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillHeader",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustId = table.Column<int>(nullable: true),
                    Status = table.Column<string>(nullable: false),
                    TotalAmt = table.Column<double>(nullable: false),
                    Discount = table.Column<double>(nullable: false),
                    TotalNetAmt = table.Column<double>(nullable: false),
                    PaidAmt = table.Column<double>(nullable: false),
                    CreatedDataTime = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillHeader_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillHeader_Customer_CustId",
                        column: x => x.CustId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustAcc",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustId = table.Column<int>(nullable: false),
                    Paid = table.Column<double>(nullable: false),
                    Debt = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustAcc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustAcc_Customer_CustId",
                        column: x => x.CustId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProdImg",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImgFile = table.Column<string>(nullable: false),
                    ProdId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdImg", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdImg_ProdInfo_ProdId",
                        column: x => x.ProdId,
                        principalTable: "ProdInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IncomingGood",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WhId = table.Column<int>(nullable: true),
                    ProdId = table.Column<int>(nullable: true),
                    Qty = table.Column<double>(nullable: false),
                    UOM = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingGood", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomingGood_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncomingGood_ProdInfo_ProdId",
                        column: x => x.ProdId,
                        principalTable: "ProdInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomingGood_Warehouse_WhId",
                        column: x => x.WhId,
                        principalTable: "Warehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvStockQty",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WhId = table.Column<int>(nullable: true),
                    ProdId = table.Column<int>(nullable: false),
                    Qty = table.Column<double>(nullable: false),
                    UOM = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvStockQty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvStockQty_ProdInfo_ProdId",
                        column: x => x.ProdId,
                        principalTable: "ProdInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvStockQty_Warehouse_WhId",
                        column: x => x.WhId,
                        principalTable: "Warehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProdId = table.Column<int>(nullable: true),
                    WhId = table.Column<int>(nullable: true),
                    Qty = table.Column<double>(nullable: false),
                    TransType = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvTransaction_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvTransaction_ProdInfo_ProdId",
                        column: x => x.ProdId,
                        principalTable: "ProdInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvTransaction_Warehouse_WhId",
                        column: x => x.WhId,
                        principalTable: "Warehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvTransferHeader",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromWhId = table.Column<int>(nullable: true),
                    ToWhId = table.Column<int>(nullable: true),
                    TransferStatus = table.Column<string>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: false),
                    ApprovedById = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvTransferHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvTransferHeader_AspNetUsers_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvTransferHeader_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvTransferHeader_Warehouse_FromWhId",
                        column: x => x.FromWhId,
                        principalTable: "Warehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvTransferHeader_Warehouse_ToWhId",
                        column: x => x.ToWhId,
                        principalTable: "Warehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BillItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<int>(nullable: false),
                    ProdId = table.Column<int>(nullable: true),
                    Qty = table.Column<double>(nullable: false),
                    UnitPrice = table.Column<double>(nullable: false),
                    TotalAmt = table.Column<double>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillItems_BillHeader_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "BillHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillItems_ProdInfo_ProdId",
                        column: x => x.ProdId,
                        principalTable: "ProdInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BillPayment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillHeaderId = table.Column<int>(nullable: false),
                    CustId = table.Column<int>(nullable: true),
                    PaidAmt = table.Column<double>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillPayment_BillHeader_BillHeaderId",
                        column: x => x.BillHeaderId,
                        principalTable: "BillHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillPayment_Customer_CustId",
                        column: x => x.CustId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvTransfer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<int>(nullable: false),
                    ProdId = table.Column<int>(nullable: true),
                    Qty = table.Column<double>(nullable: false),
                    UOM = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvTransfer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvTransfer_InvTransferHeader_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "InvTransferHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvTransfer_ProdInfo_ProdId",
                        column: x => x.ProdId,
                        principalTable: "ProdInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BillHeader_CreatedById",
                table: "BillHeader",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BillHeader_CustId",
                table: "BillHeader",
                column: "CustId");

            migrationBuilder.CreateIndex(
                name: "IX_BillItems_HeaderId",
                table: "BillItems",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_BillItems_ProdId",
                table: "BillItems",
                column: "ProdId");

            migrationBuilder.CreateIndex(
                name: "IX_BillPayment_BillHeaderId",
                table: "BillPayment",
                column: "BillHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_BillPayment_CustId",
                table: "BillPayment",
                column: "CustId");

            migrationBuilder.CreateIndex(
                name: "IX_CustAcc_CustId",
                table: "CustAcc",
                column: "CustId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CreatedById",
                table: "Customer",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingGood_CreatedById",
                table: "IncomingGood",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingGood_ProdId",
                table: "IncomingGood",
                column: "ProdId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingGood_WhId",
                table: "IncomingGood",
                column: "WhId");

            migrationBuilder.CreateIndex(
                name: "IX_InvStockQty_ProdId",
                table: "InvStockQty",
                column: "ProdId");

            migrationBuilder.CreateIndex(
                name: "IX_InvStockQty_WhId",
                table: "InvStockQty",
                column: "WhId");

            migrationBuilder.CreateIndex(
                name: "IX_InvTransaction_CreatedById",
                table: "InvTransaction",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_InvTransaction_ProdId",
                table: "InvTransaction",
                column: "ProdId");

            migrationBuilder.CreateIndex(
                name: "IX_InvTransaction_WhId",
                table: "InvTransaction",
                column: "WhId");

            migrationBuilder.CreateIndex(
                name: "IX_InvTransfer_HeaderId",
                table: "InvTransfer",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_InvTransfer_ProdId",
                table: "InvTransfer",
                column: "ProdId");

            migrationBuilder.CreateIndex(
                name: "IX_InvTransferHeader_ApprovedById",
                table: "InvTransferHeader",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_InvTransferHeader_CreatedById",
                table: "InvTransferHeader",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_InvTransferHeader_FromWhId",
                table: "InvTransferHeader",
                column: "FromWhId");

            migrationBuilder.CreateIndex(
                name: "IX_InvTransferHeader_ToWhId",
                table: "InvTransferHeader",
                column: "ToWhId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdImg_ProdId",
                table: "ProdImg",
                column: "ProdId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdInfo_CreatedById",
                table: "ProdInfo",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouse_CreatedById",
                table: "Warehouse",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouse_TypeId",
                table: "Warehouse",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BillItems");

            migrationBuilder.DropTable(
                name: "BillPayment");

            migrationBuilder.DropTable(
                name: "CustAcc");

            migrationBuilder.DropTable(
                name: "IncomingGood");

            migrationBuilder.DropTable(
                name: "InvStockQty");

            migrationBuilder.DropTable(
                name: "InvTransaction");

            migrationBuilder.DropTable(
                name: "InvTransfer");

            migrationBuilder.DropTable(
                name: "ProdImg");

            migrationBuilder.DropTable(
                name: "TempProdImg");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "BillHeader");

            migrationBuilder.DropTable(
                name: "InvTransferHeader");

            migrationBuilder.DropTable(
                name: "ProdInfo");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Warehouse");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "WhType");
        }
    }
}
