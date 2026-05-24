// Data/AppDbContext.cs
//
// É aqui que o EF Core conversa com o banco de dados.
// O DbSet<Livro> representa a tabela [Livros] no SQL Server do Azure.
using BibliotecaRosa.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaRosa.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Livro> Livros => Set<Livro>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Livro>(entity =>
        {
            entity.HasKey(l => l.Id);

            entity.Property(l => l.Titulo)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(l => l.Autor)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(l => l.Isbn)
                  .IsRequired()
                  .HasMaxLength(20);

            // ISBN único — o banco rejeita duplicatas no nível do storage
            entity.HasIndex(l => l.Isbn)
                  .IsUnique();

            entity.Property(l => l.CreatedAt)
                  .HasDefaultValueSql("GETUTCDATE()");
        });

        // Dados iniciais inseridos automaticamente na primeira migration
        modelBuilder.Entity<Livro>().HasData(
            new Livro { Id = 1, Titulo = "O Senhor dos Anéis", Autor = "J.R.R. Tolkien", Isbn = "9788533613379", CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Livro { Id = 2, Titulo = "1984", Autor = "George Orwell", Isbn = "9788535914849", CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Livro { Id = 3, Titulo = "Dom Casmurro", Autor = "Machado de Assis", Isbn = "9788503011996", CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Livro { Id = 4, Titulo = "O Pequeno Príncipe", Autor = "Antoine de Saint-Exupéry", Isbn = "9788595081512", CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
