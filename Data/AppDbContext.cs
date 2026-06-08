using Microsoft.EntityFrameworkCore;
using BibliotecaRosa.Models;

namespace BibliotecaRosa.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Livro> Livros => Set<Livro>();
    public DbSet<Emprestimo> Emprestimos => Set<Emprestimo>();
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Livro
        modelBuilder.Entity<Livro>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Titulo).IsRequired().HasMaxLength(200);
            entity.Property(l => l.Autor).IsRequired().HasMaxLength(150);
            entity.Property(l => l.Isbn).IsRequired().HasMaxLength(20);
            entity.HasIndex(l => l.Isbn).IsUnique();
            entity.Property(l => l.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        modelBuilder.Entity<Livro>().HasData(
            new Livro { Id = 1, Titulo = "O Senhor dos Anéis", Autor = "J.R.R. Tolkien", Isbn = "9788533613379", CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
        #endregion

        #region Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
        });

        string hashFixoPreGerado = "$2a$11$eImiTXuWV5M729BOn7gMvO8V8L09Fvsh1F3A.Z3rYdM5m1YmXoRFe";

        modelBuilder.Entity<Usuario>().HasData(
            new Usuario { Id = 1, Nome = "Admin Rosa", Email = "admin@rosa.com", SenhaHash = hashFixoPreGerado, Role = BibliotecaRosa.Enums.Role.Administrador },
            new Usuario { Id = 2, Nome = "Professor Girafales", Email = "professor@rosa.com", SenhaHash = hashFixoPreGerado, Role = BibliotecaRosa.Enums.Role.Professor },
            new Usuario { Id = 3, Nome = "Aluno Chaves", Email = "aluno@rosa.com", SenhaHash = hashFixoPreGerado, Role = BibliotecaRosa.Enums.Role.Aluno }
        );
        #endregion

        #region Emprestimo
        modelBuilder.Entity<Emprestimo>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Livro)
                  .WithMany()
                  .HasForeignKey(e => e.LivroId);

            entity.HasOne(e => e.Usuario)
                  .WithMany()
                  .HasForeignKey(e => e.UsuarioId);

            entity.Property(e => e.DataEmprestimo)
                  .HasDefaultValueSql("GETUTCDATE()");
        });
        #endregion
    }
}
