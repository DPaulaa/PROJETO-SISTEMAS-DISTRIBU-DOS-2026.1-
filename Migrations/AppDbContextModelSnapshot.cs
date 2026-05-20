// Migrations/AppDbContextModelSnapshot.cs

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BibliotecaRosa.Migrations;

[DbContext(typeof(AppDbContextModelSnapshot))]
partial class AppDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "9.0.5")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("BibliotecaRosa.Models.Livro", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

            b.Property<string>("Autor").IsRequired().HasMaxLength(150).HasColumnType("nvarchar(150)");
            b.Property<DateTime>("CreatedAt").ValueGeneratedOnAdd().HasColumnType("datetime2").HasDefaultValueSql("GETUTCDATE()");
            b.Property<string>("Isbn").IsRequired().HasMaxLength(20).HasColumnType("nvarchar(20)");
            b.Property<string>("Titulo").IsRequired().HasMaxLength(200).HasColumnType("nvarchar(200)");

            b.HasKey("Id");
            b.HasIndex("Isbn").IsUnique();
            b.ToTable("Livros");

            b.HasData(
                new { Id = 1, Autor = "J.R.R. Tolkien",           CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), Isbn = "978-8533613379", Titulo = "O Senhor dos Anéis"  },
                new { Id = 2, Autor = "George Orwell",             CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), Isbn = "978-8535914849", Titulo = "1984"                },
                new { Id = 3, Autor = "Machado de Assis",          CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), Isbn = "978-8503011996", Titulo = "Dom Casmurro"        },
                new { Id = 4, Autor = "Antoine de Saint-Exupéry", CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), Isbn = "978-8595081512", Titulo = "O Pequeno Príncipe"  });
        });
#pragma warning restore 612, 618
    }
}
