using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BibliotecaRosa.Migrations
{
    /// <inheritdoc />
    public partial class CriarEstruturaFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SenhaHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Email", "Nome", "Role", "SenhaHash" },
                values: new object[,]
                {
                    { 1, "admin@rosa.com", "Admin Rosa", 1, "$2a$11$eImiTXuWV5M729BOn7gMvO8V8L09Fvsh1F3A.Z3rYdM5m1YmXoRFe" },
                    { 2, "professor@rosa.com", "Professor Girafales", 2, "$2a$11$eImiTXuWV5M729BOn7gMvO8V8L09Fvsh1F3A.Z3rYdM5m1YmXoRFe" },
                    { 3, "aluno@rosa.com", "Aluno Chaves", 3, "$2a$11$eImiTXuWV5M729BOn7gMvO8V8L09Fvsh1F3A.Z3rYdM5m1YmXoRFe" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}