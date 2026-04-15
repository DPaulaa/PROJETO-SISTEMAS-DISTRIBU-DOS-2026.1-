var builder = WebApplication.CreateBuilder(args);

// Registro dos serviços necessários
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Biblioteca Rosa",
        Version = "v1",
        Description = "Projeto – Sistemas Distribuidos (2026/1)"
    });
});

var app = builder.Build();

// Ativa o Swagger apenas em desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biblioteca Rosa v1");
        c.RoutePrefix = "swagger"; // Acesse em /swagger
    });
}

var livros = new List<Livro>
{
    new Livro(1, "O Senhor dos Anéis", "J.R.R. Tolkien", "978-8533613379", DateTime.UtcNow.AddDays(-180)),
    new Livro(2, "1984", "George Orwell", "978-8535914849", DateTime.UtcNow.AddDays(-120)),
    new Livro(3, "Dom Casmurro", "Machado de Assis", "978-8503011996", DateTime.UtcNow.AddDays(-90)),
    new Livro(4, "O Pequeno Príncipe", "Antoine de Saint-Exupéry", "978-8595081512", DateTime.UtcNow.AddDays(-60)),
};

var nextId = livros.Max(l => l.Id) + 1;

app.MapGet("/", () => "Biblioteca API v1.0");

app.MapGet("/livros", () => Results.Ok(livros))
    .WithName("GetAllLivros")
    .WithTags("Livros")
    .WithSummary("Lista todos os livros da biblioteca");

app.MapGet("/livros/{id}", (int id) =>
{
    var livro = livros.FirstOrDefault(l => l.Id == id);
    if (livro is null)
        return Results.NotFound(new { message = $"Livro {id} não encontrado." });
    return Results.Ok(livro);
})
.WithName("GetLivroById")
.WithTags("Livros")
.WithSummary("Busca livro por ID");

app.MapPost("/livros", (LivroRequest request) =>
{
    // Validação básica
    if (string.IsNullOrWhiteSpace(request.Titulo))
        return Results.BadRequest(new { message = "O campo 'titulo' é obrigatório." });

    if (string.IsNullOrWhiteSpace(request.Autor))
        return Results.BadRequest(new { message = "O campo 'autor' é obrigatório." });

    var livro = new Livro(
        nextId++, 
        request.Titulo.Trim(), 
        request.Autor.Trim(), 
        request.Isbn?.Trim() ?? "Sem ISBN", 
        DateTime.UtcNow
    );
    
    livros.Add(livro);
    
    // 201 Created + Location header apontando para o novo recurso
    return Results.Created($"/livros/{livro.Id}", livro);
})
.WithName("CreateLivro")
.WithTags("Livros")
.WithSummary("Adiciona um novo livro à biblioteca");

app.MapPut("/livros/{id}", (int id, LivroRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Titulo))
        return Results.BadRequest(new { message = "O campo 'titulo' é obrigatório." });
    
    if (string.IsNullOrWhiteSpace(request.Autor))
        return Results.BadRequest(new { message = "O campo 'autor' é obrigatório." });
    
    var index = livros.FindIndex(l => l.Id == id);
    if (index < 0)
        return Results.NotFound(new { message = $"Livro {id} não encontrado." });
    
    // Mantém o CreatedAt original — atualiza apenas os campos editáveis
    livros[index] = livros[index] with 
    { 
        Titulo = request.Titulo.Trim(),
        Autor = request.Autor.Trim(),
        Isbn = request.Isbn?.Trim() ?? livros[index].Isbn
    };
    
    return Results.Ok(livros[index]);
})
.WithName("UpdateLivro")
.WithTags("Livros")
.WithSummary("Atualiza os dados de um livro existente");

app.MapDelete("/livros/{id}", (int id) =>
{
    var livro = livros.FirstOrDefault(l => l.Id == id);
    if (livro is null)
        return Results.NotFound(new { message = $"Livro {id} não encontrado." });
    
    livros.Remove(livro);
    return Results.NoContent(); // 204 — sucesso sem corpo
})
.WithName("DeleteLivro")
.WithTags("Livros")
.WithSummary("Remove um livro da biblioteca");

app.Run();

// Records
record Livro(int Id, string Titulo, string Autor, string Isbn, DateTime CreatedAt);
record LivroRequest(string Titulo, string Autor, string Isbn);
