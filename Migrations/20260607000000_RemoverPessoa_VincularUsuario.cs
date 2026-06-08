using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotecaRosa.Migrations
{
    /// <inheritdoc />
    public partial class RemoverPessoa_VincularUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove FK antiga de PessoaId (se existir)
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'Emprestimos' AND COLUMN_NAME = 'PessoaId'
                )
                BEGIN
                    -- Remove FK constraint se existir
                    DECLARE @fk NVARCHAR(200)
                    SELECT @fk = fk.name
                    FROM sys.foreign_keys fk
                    JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
                    JOIN sys.columns c ON fkc.parent_column_id = c.column_id AND c.object_id = fkc.parent_object_id
                    WHERE OBJECT_NAME(fkc.parent_object_id) = 'Emprestimos' AND c.name = 'PessoaId'

                    IF @fk IS NOT NULL
                        EXEC('ALTER TABLE Emprestimos DROP CONSTRAINT ' + @fk)

                    ALTER TABLE Emprestimos DROP COLUMN PessoaId
                END
            ");

            // Adiciona UsuarioId se não existir
            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'Emprestimos' AND COLUMN_NAME = 'UsuarioId'
                )
                BEGIN
                    ALTER TABLE Emprestimos ADD UsuarioId INT NOT NULL DEFAULT 1
                    ALTER TABLE Emprestimos ADD CONSTRAINT FK_Emprestimos_Usuarios_UsuarioId
                        FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id) ON DELETE NO ACTION
                END
            ");

            // Adiciona LivroId explícita se não existir (antes era shadow property)
            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'Emprestimos' AND COLUMN_NAME = 'LivroId'
                )
                BEGIN
                    ALTER TABLE Emprestimos ADD LivroId INT NOT NULL DEFAULT 1
                    ALTER TABLE Emprestimos ADD CONSTRAINT FK_Emprestimos_Livros_LivroId
                        FOREIGN KEY (LivroId) REFERENCES Livros(Id) ON DELETE NO ACTION
                END
            ");

            // Remove tabela Pessoas se existir
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Pessoas')
                BEGIN
                    DROP TABLE Pessoas
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Pessoas')
                BEGIN
                    CREATE TABLE Pessoas (
                        Id INT PRIMARY KEY IDENTITY,
                        Nome NVARCHAR(100) NOT NULL,
                        Email NVARCHAR(100),
                        Telefone NVARCHAR(20),
                        CpfCnpj NVARCHAR(20),
                        TipoPessoa NVARCHAR(50) NOT NULL
                    )
                END
            ");
        }
    }
}
