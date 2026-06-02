using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
#pragma warning disable CA1814

namespace BibliotecaRosa.Migrations;

public partial class AddEmprestimoPessoa : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Adiciona coluna QuantidadeDisponivel na tabela Livros
        migrationBuilder.AddColumn<int>(
            name: "QuantidadeDisponivel",
            table: "Livros",
            type: "int",
            nullable: false,
            defaultValue: 0);

        // Cria tabela Pessoas
        migrationBuilder.CreateTable(
            name: "Pessoas",
            columns: table => new
            {
                Id        = table.Column<int>(type: "int", nullable: false)
                                 .Annotation("SqlServer:Identity", "1, 1"),
                Nome      = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Email     = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Telefone  = table.Column<string>(type: "nvarchar(20)",  maxLength: 20,  nullable: false),
                CpfCnpj   = table.Column<string>(type: "nvarchar(20)",  maxLength: 20,  nullable: false),
                TipoPessoa = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Pessoas", x => x.Id);
            });

        // Cria tabela Emprestimos
        migrationBuilder.CreateTable(
            name: "Emprestimos",
            columns: table => new
            {
                Id                   = table.Column<int>(type: "int", nullable: false)
                                            .Annotation("SqlServer:Identity", "1, 1"),
                LivroId              = table.Column<int>(type: "int", nullable: false),
                PessoaId             = table.Column<int>(type: "int", nullable: false),
                DataEmprestimo       = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                DataDevolucao        = table.Column<DateTime>(type: "datetime2", nullable: true),
                DataDevolucaoPrevista = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                    name: "FK_Emprestimos_Pessoas_PessoaId",
                    column: x => x.PessoaId,
                    principalTable: "Pessoas",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Emprestimos_LivroId",
            table: "Emprestimos",
            column: "LivroId");

        migrationBuilder.CreateIndex(
            name: "IX_Emprestimos_PessoaId",
            table: "Emprestimos",
            column: "PessoaId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Emprestimos");
        migrationBuilder.DropTable(name: "Pessoas");
        migrationBuilder.DropColumn(name: "QuantidadeDisponivel", table: "Livros");
    }
}
