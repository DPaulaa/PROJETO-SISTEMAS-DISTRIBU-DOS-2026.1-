using Microsoft.EntityFrameworkCore;
using BibliotecaRosa.Models;

namespace BibliotecaRosa.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Livro> Livros => Set<Livro>();
    public DbSet<Emprestimo> Emprestimos => Set<Emprestimo>();
    public DbSet<Pessoa> Pessoas => Set<Pessoa>();
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        // Ignora o aviso de dados dinâmicos do .NET 10
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region Configurações da Tabela Livro
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
            new Livro { Id = 1, Titulo = "O Senhor dos Anéis", Autor = "J.R.R. Tolkien", Isbn = "9788533613379", CreatedAt = new System.DateTime(2025, 1, 1, 0, 0, 0, System.DateTimeKind.Utc) }
        );
        #endregion

        #region Configurações da Tabela Usuário
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

        modelBuilder.Entity<Emprestimo>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Livro)
                  .WithMany()
                  .HasForeignKey("LivroId");

            entity.HasOne(e => e.Pessoa)
                  .WithMany()
                  .HasForeignKey("PessoaId");

            entity.Property(e => e.DataEmprestimo)
                  .HasDefaultValueSql("GETUTCDATE()");
        });

        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Nome)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(p => p.Email)
                  .HasMaxLength(100);

            entity.Property(p => p.Telefone)
                  .HasMaxLength(20);

            entity.Property(p => p.CpfCnpj)
                  .HasMaxLength(20);

            entity.Property(p => p.TipoPessoa)
                .HasConversion<string>()
                .IsRequired();
        });
    }
}