// Migrations/20260101000000_Inicial.cs
// Migration que cria a tabela Livros no banco de dados.
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
#pragma warning disable CA1814

namespace BibliotecaRosa.Migrations;

public partial class Inicial : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Livros",
            columns: table => new
            {
                Id        = table.Column<int>(type: "int", nullable: false)
                                 .Annotation("SqlServer:Identity", "1, 1"),
                Titulo    = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Autor     = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Isbn      = table.Column<string>(type: "nvarchar(20)",  maxLength: 20,  nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Livros", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Livros_Isbn",
            table: "Livros",
            column: "Isbn",
            unique: true);

        migrationBuilder.InsertData(
            table: "Livros",
            columns: new[] { "Id", "Titulo", "Autor", "Isbn", "CreatedAt" },
            values: new object[,]
            {
                { 1, "O Senhor dos Anéis",  "J.R.R. Tolkien",           "978-8533613379", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                { 2, "1984",                "George Orwell",             "978-8535914849", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                { 3, "Dom Casmurro",        "Machado de Assis",          "978-8503011996", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                { 4, "O Pequeno Príncipe",  "Antoine de Saint-Exupéry", "978-8595081512", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Livros");
    }
}
