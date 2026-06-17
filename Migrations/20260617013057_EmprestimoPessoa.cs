using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BibliotecaRosa.Migrations
{
    /// <inheritdoc />
    public partial class EmprestimoPessoa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantidadeDisponivel",
                table: "Livros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Emprestimos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LivroId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    DataEmprestimo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataDevolucaoPrevista = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataDevolucao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emprestimos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Emprestimos_Livros_LivroId",
                        column: x => x.LivroId,
                        principalTable: "Livros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Emprestimos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Livros",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Isbn", "QuantidadeDisponivel" },
                values: new object[] { "9788533613379", 1 });

            migrationBuilder.UpdateData(
                table: "Livros",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Isbn", "QuantidadeDisponivel" },
                values: new object[] { "9788535914849", 1 });

            migrationBuilder.UpdateData(
                table: "Livros",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Isbn", "QuantidadeDisponivel" },
                values: new object[] { "9788503011996", 1 });

            migrationBuilder.UpdateData(
                table: "Livros",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Isbn", "QuantidadeDisponivel" },
                values: new object[] { "9788595081512", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Emprestimos_LivroId",
                table: "Emprestimos",
                column: "LivroId");

            migrationBuilder.CreateIndex(
                name: "IX_Emprestimos_UsuarioId",
                table: "Emprestimos",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emprestimos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropColumn(
                name: "QuantidadeDisponivel",
                table: "Livros");

            migrationBuilder.UpdateData(
                table: "Livros",
                keyColumn: "Id",
                keyValue: 1,
                column: "Isbn",
                value: "978-8533613379");

            migrationBuilder.UpdateData(
                table: "Livros",
                keyColumn: "Id",
                keyValue: 2,
                column: "Isbn",
                value: "978-8535914849");

            migrationBuilder.UpdateData(
                table: "Livros",
                keyColumn: "Id",
                keyValue: 3,
                column: "Isbn",
                value: "978-8503011996");

            migrationBuilder.UpdateData(
                table: "Livros",
                keyColumn: "Id",
                keyValue: 4,
                column: "Isbn",
                value: "978-8595081512");
        }
    }
}
