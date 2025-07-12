using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Biogenom.Nutrition.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nutrients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DailyNormMin = table.Column<decimal>(type: "numeric", nullable: false),
                    DailyNormMax = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nutrients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NutritionAssessments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<decimal>(type: "numeric", nullable: false),
                    Height = table.Column<decimal>(type: "numeric", nullable: false),
                    MealsPerDay = table.Column<int>(type: "integer", nullable: false),
                    VegetablesPerDay = table.Column<int>(type: "integer", nullable: false),
                    FruitsPerDay = table.Column<int>(type: "integer", nullable: false),
                    WaterIntake = table.Column<int>(type: "integer", nullable: false),
                    EatsBreakfast = table.Column<bool>(type: "boolean", nullable: false),
                    EatsFastFood = table.Column<bool>(type: "boolean", nullable: false),
                    EatsProcessedFood = table.Column<bool>(type: "boolean", nullable: false),
                    ActivityLevel = table.Column<int>(type: "integer", nullable: false),
                    SleepQuality = table.Column<int>(type: "integer", nullable: false),
                    StressLevel = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutritionAssessments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Supplements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Dosage = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    WhenToTake = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NutrientBalances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NutritionAssessmentId = table.Column<int>(type: "integer", nullable: false),
                    NutrientId = table.Column<int>(type: "integer", nullable: false),
                    CurrentValue = table.Column<decimal>(type: "numeric", nullable: false),
                    FromDiet = table.Column<decimal>(type: "numeric", nullable: false),
                    FromSupplements = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutrientBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NutrientBalances_Nutrients_NutrientId",
                        column: x => x.NutrientId,
                        principalTable: "Nutrients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NutrientBalances_NutritionAssessments_NutritionAssessmentId",
                        column: x => x.NutritionAssessmentId,
                        principalTable: "NutritionAssessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalizedSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NutritionAssessmentId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalizedSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalizedSets_NutritionAssessments_NutritionAssessmentId",
                        column: x => x.NutritionAssessmentId,
                        principalTable: "NutritionAssessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalizedSetSupplements",
                columns: table => new
                {
                    PersonalizedSetsId = table.Column<int>(type: "integer", nullable: false),
                    SupplementsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalizedSetSupplements", x => new { x.PersonalizedSetsId, x.SupplementsId });
                    table.ForeignKey(
                        name: "FK_PersonalizedSetSupplements_PersonalizedSets_PersonalizedSet~",
                        column: x => x.PersonalizedSetsId,
                        principalTable: "PersonalizedSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonalizedSetSupplements_Supplements_SupplementsId",
                        column: x => x.SupplementsId,
                        principalTable: "Supplements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NutrientBalances_NutrientId",
                table: "NutrientBalances",
                column: "NutrientId");

            migrationBuilder.CreateIndex(
                name: "IX_NutrientBalances_NutritionAssessmentId",
                table: "NutrientBalances",
                column: "NutritionAssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalizedSets_NutritionAssessmentId",
                table: "PersonalizedSets",
                column: "NutritionAssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalizedSetSupplements_SupplementsId",
                table: "PersonalizedSetSupplements",
                column: "SupplementsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NutrientBalances");

            migrationBuilder.DropTable(
                name: "PersonalizedSetSupplements");

            migrationBuilder.DropTable(
                name: "Nutrients");

            migrationBuilder.DropTable(
                name: "PersonalizedSets");

            migrationBuilder.DropTable(
                name: "Supplements");

            migrationBuilder.DropTable(
                name: "NutritionAssessments");
        }
    }
}
