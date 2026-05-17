using Microsoft.EntityFrameworkCore;
using LivrariaRosa.Models.Entities;

namespace LivrariaRosa.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Livro> Livros { get; set; }

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
                  .HasMaxLength(20);

            entity.Property(l => l.CreatedAt)
                  .HasDefaultValueSql("GETUTCDATE()");

            // Soft delete: filtro global que exclui registros deletados das queries
            entity.HasQueryFilter(l => l.Ativo);
        });

        // Seed de dados iniciais
        modelBuilder.Entity<Livro>().HasData(
            new Livro { Id = 1, Titulo = "O Senhor dos Anéis",  Autor = "J.R.R. Tolkien",           Isbn = "978-8533613379", CreatedAt = new DateTime(2025, 11, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Livro { Id = 2, Titulo = "1984",                 Autor = "George Orwell",             Isbn = "978-8535914849", CreatedAt = new DateTime(2026, 1,  1, 0, 0, 0, DateTimeKind.Utc) },
            new Livro { Id = 3, Titulo = "Dom Casmurro",         Autor = "Machado de Assis",          Isbn = "978-8503011996", CreatedAt = new DateTime(2026, 2,  1, 0, 0, 0, DateTimeKind.Utc) },
            new Livro { Id = 4, Titulo = "O Pequeno Príncipe",   Autor = "Antoine de Saint-Exupéry", Isbn = "978-8595081512", CreatedAt = new DateTime(2026, 3,  1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
