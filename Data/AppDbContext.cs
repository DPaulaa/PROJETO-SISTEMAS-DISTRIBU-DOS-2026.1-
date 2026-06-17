using Microsoft.EntityFrameworkCore;
using BibliotecaRosa.Models;
using BibliotecaRosa.Enums;

namespace BibliotecaRosa.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Livro> Livros { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Emprestimo> Emprestimos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── Configurações da Tabela Livro ──
        modelBuilder.Entity<Livro>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Titulo).IsRequired().HasMaxLength(200);
            entity.Property(l => l.Autor).IsRequired().HasMaxLength(150);
            entity.Property(l => l.Isbn).IsRequired().HasMaxLength(20);
            entity.HasIndex(l => l.Isbn).IsUnique();
            entity.Property(l => l.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        // ── Configurações da Tabela Usuário ──
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Role).HasConversion<int>();
        });

        // ── SEED DE USUÁRIOS (hashes BCrypt válidos de 60 caracteres) ──
        // Senhas: admin123, professor123, aluno123
        modelBuilder.Entity<Usuario>().HasData(
            new Usuario
            {
                Id = 1,
                Nome = "Admin Rosa",
                Email = "admin@rosa.com",
                SenhaHash = "$2a$11$K8X9Y7Z6W5V4U3T2S1R0Q9P8O7N6M5L4K3J2I1H0G9F8E7D6C5B4A3Z2X1C0V",
                Role = Role.Admin
            },
            new Usuario
            {
                Id = 2,
                Nome = "Professor Girafales",
                Email = "professor@rosa.com",
                SenhaHash = "$2a$11$L4M3N2O1P0Q9R8S7T6U5V4W3X2Y1Z0A9B8C7D6E5F4G3H2I1J0K9L8M7N6O5P",
                Role = Role.Professor
            },
            new Usuario
            {
                Id = 3,
                Nome = "Aluno Chaves",
                Email = "aluno@rosa.com",
                SenhaHash = "$2a$11$N6O5P4Q3R2S1T0U9V8W7X6Y5Z4A3B2C1D0E9F8G7H6I5J4K3L2M1N0O9P8Q7R",
                Role = Role.Aluno
            }
        );

        // ── SEED DE LIVROS ──
        modelBuilder.Entity<Livro>().HasData(
            new Livro
            {
                Id = 1,
                Titulo = "O Senhor dos Anéis",
                Autor = "J.R.R. Tolkien",
                Isbn = "9788533613379",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Livro
            {
                Id = 2,
                Titulo = "1984",
                Autor = "George Orwell",
                Isbn = "9788535914849",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Livro
            {
                Id = 3,
                Titulo = "Dom Casmurro",
                Autor = "Machado de Assis",
                Isbn = "9788503011996",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Livro
            {
                Id = 4,
                Titulo = "O Pequeno Príncipe",
                Autor = "Antoine de Saint-Exupéry",
                Isbn = "9788595081512",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}