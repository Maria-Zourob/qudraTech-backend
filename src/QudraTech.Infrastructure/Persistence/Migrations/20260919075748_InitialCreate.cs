using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QudraTech.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InitiativeCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitiativeCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Initiatives",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InitiativeCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetGroupAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetGroupEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiatives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Initiatives_InitiativeCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "InitiativeCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InitiativeActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InitiativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlannedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitiativeActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InitiativeActivities_Initiatives_InitiativeId",
                        column: x => x.InitiativeId,
                        principalTable: "Initiatives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InitiativeBeneficiaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InitiativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InternalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgeGroupMin = table.Column<int>(type: "int", nullable: false),
                    AgeGroupMax = table.Column<int>(type: "int", nullable: false),
                    Camp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitiativeBeneficiaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InitiativeBeneficiaries_Initiatives_InitiativeId",
                        column: x => x.InitiativeId,
                        principalTable: "Initiatives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InitiativeKpis",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InitiativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Definition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Baseline = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Target = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Actual = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectionMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Frequency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponsiblePerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitiativeKpis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InitiativeKpis_Initiatives_InitiativeId",
                        column: x => x.InitiativeId,
                        principalTable: "Initiatives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InitiativeObjectives",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InitiativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitiativeObjectives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InitiativeObjectives_Initiatives_InitiativeId",
                        column: x => x.InitiativeId,
                        principalTable: "Initiatives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KpiUpdates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InitiativeKpiId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KpiUpdates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KpiUpdates_InitiativeKpis_InitiativeKpiId",
                        column: x => x.InitiativeKpiId,
                        principalTable: "InitiativeKpis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KpiEvidences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KpiUpdateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KpiEvidences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KpiEvidences_KpiUpdates_KpiUpdateId",
                        column: x => x.KpiUpdateId,
                        principalTable: "KpiUpdates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InitiativeActivities_InitiativeId",
                table: "InitiativeActivities",
                column: "InitiativeId");

            migrationBuilder.CreateIndex(
                name: "IX_InitiativeBeneficiaries_InitiativeId",
                table: "InitiativeBeneficiaries",
                column: "InitiativeId");

            migrationBuilder.CreateIndex(
                name: "IX_InitiativeKpis_InitiativeId",
                table: "InitiativeKpis",
                column: "InitiativeId");

            migrationBuilder.CreateIndex(
                name: "IX_InitiativeObjectives_InitiativeId",
                table: "InitiativeObjectives",
                column: "InitiativeId");

            migrationBuilder.CreateIndex(
                name: "IX_Initiatives_CategoryId",
                table: "Initiatives",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_KpiEvidences_KpiUpdateId",
                table: "KpiEvidences",
                column: "KpiUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_KpiUpdates_InitiativeKpiId",
                table: "KpiUpdates",
                column: "InitiativeKpiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InitiativeActivities");

            migrationBuilder.DropTable(
                name: "InitiativeBeneficiaries");

            migrationBuilder.DropTable(
                name: "InitiativeObjectives");

            migrationBuilder.DropTable(
                name: "KpiEvidences");

            migrationBuilder.DropTable(
                name: "KpiUpdates");

            migrationBuilder.DropTable(
                name: "InitiativeKpis");

            migrationBuilder.DropTable(
                name: "Initiatives");

            migrationBuilder.DropTable(
                name: "InitiativeCategories");
        }
    }
}
