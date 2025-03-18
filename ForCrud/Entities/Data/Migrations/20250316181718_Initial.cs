using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CountryName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(type: "uuid", nullable: false),
                    PersonName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    ReceiveNewsLetters = table.Column<bool>(type: "boolean", nullable: false),
                    TaxIdentificationNumber = table.Column<string>(type: "varchar(8)", nullable: true, defaultValue: "ABC12345")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                    table.CheckConstraint("CHK_TIN", "char_length(\"TaxIdentificationNumber\") = 8");
                    table.ForeignKey(
                        name: "FK_Persons_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId");
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryId", "CountryName" },
                values: new object[,]
                {
                    { new Guid("18f29207-6401-45c2-8820-e400b1c14461"), "Azerbaijan" },
                    { new Guid("272d4b54-ed10-4644-96cf-f0fce4b88bb1"), "USA" },
                    { new Guid("49e9d52e-32ef-4fd3-b35f-c66df8a8aad3"), "Australia" },
                    { new Guid("983bbc9d-3556-4942-8cbf-ad239c532ccd"), "Canada" },
                    { new Guid("a77f3c7a-72f0-4646-83d3-bbac2128bff3"), "UK" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonId", "Address", "CountryId", "DateOfBirth", "Email", "Gender", "PersonName", "ReceiveNewsLetters" },
                values: new object[,]
                {
                    { new Guid("1afa76c1-9c2d-49c1-95a1-4d3a8df91e3e"), "123 Main St, Anytown, USA", new Guid("272d4b54-ed10-4644-96cf-f0fce4b88bb1"), new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "john.doe@example.com", "Male", "John Doe", false },
                    { new Guid("7372ca1c-b740-419b-b72a-3b27a55724ce"), "456 Elm St, Othertown, USA", new Guid("a77f3c7a-72f0-4646-83d3-bbac2128bff3"), new DateTime(1985, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "jane.smith@example.com", "Female", "Jane Smith", false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Persons_CountryId",
                table: "Persons",
                column: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
