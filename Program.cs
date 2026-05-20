using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using LivrariaRosa.Data;
using LivrariaRosa.Middlewares;
using LivrariaRosa.Repositories;
using LivrariaRosa.Repositories.Interfaces;
using LivrariaRosa.Services;
using LivrariaRosa.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ── Banco de Dados (Azure SQL via EF Core) ──────────────────────────────────
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não encontrada.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// ── Injeção de Dependência (DIP — depende de abstrações) ────────────────────
builder.Services.AddScoped<ILivroRepository, LivroRepository>();
builder.Services.AddScoped<ILivroService, LivroService>();

// ── Controllers ─────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ── Swagger / OpenAPI ────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "Biblioteca Rosa — API",
        Version     = "v1",
        Description = "API REST para gerenciamento de acervo de livros.\n\n" +
                      "Projeto Final — Sistemas Distribuídos (2026/1)\n" +
                      "Professor: Alexandre Montanha"
    });

    // Inclui os comentários XML gerados pelo compilador no Swagger
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// ── Middleware global de tratamento de exceções (DEVE ser o primeiro) ────────
app.UseMiddleware<ExceptionHandlingMiddleware>();

// ── Migrations automáticas na inicialização ──────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ── Swagger (ativo em todos os ambientes para avaliação) ─────────────────────
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biblioteca Rosa v1");
    c.RoutePrefix = "swagger";
    c.DocumentTitle = "Biblioteca Rosa — API Docs";
});

// ── Pipeline HTTP ─────────────────────────────────────────────────────────────
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Rota raiz redireciona para o Swagger
app.MapGet("/", () => Results.Redirect("/swagger"))
   .ExcludeFromDescription();

app.Run();

// Necessário para que WebApplicationFactory encontre o ponto de entrada nos testes
public partial class Program { }
