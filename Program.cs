// Program.cs — Biblioteca Rosa
//
// Registramos todos os serviços que serão usados pelos controllers,sempre pelo contrato (interface), não pela implementação direta.
// Isso facilita futuras trocas — por exemplo, de memória para banco de dados.
using BibliotecaRosa.Data;
using BibliotecaRosa.Middlewares;
using BibliotecaRosa.Repositories;
using BibliotecaRosa.Repositories.Interfaces;
using BibliotecaRosa.Services;
using BibliotecaRosa.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Repositórios ──────────────────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ILivroRepository, SqlLivroRepository>();

// ── Serviços ──────────────────────────────────────────────────────────────────
// Cada interface tem sua própria responsabilidade — auth, livros e diagnóstico são serviços separados e independentes entre si.
builder.Services.AddScoped<ILivroService,       LivroService>();
builder.Services.AddScoped<IAuthService,        AuthService>();
builder.Services.AddScoped<IDiagnosticoService, DiagnosticoService>();

// ── Controllers ───────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ── Swagger ───────────────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title       = "Biblioteca Rosa",
        Version     = "v1",
        Description = "Projeto – Sistemas Distribuídos (2026/1)"
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();
//── Migrations para o banco ───────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ── Pipeline de middlewares ───────────────────────────────────

// 1. Captura de erros — precisa ser o primeiro para pegar qualquer exceção
app.UseMiddleware<ExceptionHandlingMiddleware>();

// 2. Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biblioteca Rosa v1");
        c.RoutePrefix = "swagger";
    });
}

// 3. Pipeline HTTP
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger"))
   .ExcludeFromDescription();

app.Run();
