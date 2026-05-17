# Biblioteca Rosa — API REST

Projeto Final da disciplina de **Sistemas Distribuídos (2026/1)**  
Professor: Alexandre Montanha — UniBH

---

## Sobre o Projeto

API REST para gerenciamento do acervo de livros da Biblioteca Rosa.  
Desenvolvida em ASP.NET Core com arquitetura em camadas, persistência em Azure SQL e deploy automatizado no Azure App Service via GitHub Actions.

---

## Stack Tecnológica

| Camada        | Tecnologia                              |
|---------------|-----------------------------------------|
| Framework     | ASP.NET Core 10 — Web API (Controllers) |
| ORM           | Entity Framework Core 9                 |
| Banco (prod)  | Azure SQL Database (PaaS — DTU Basic)   |
| Banco (dev)   | SQL Server LocalDB                      |
| Documentação  | Swagger / OpenAPI (Swashbuckle)         |
| Deploy        | Azure App Service via GitHub Actions    |

---

## Arquitetura

O projeto adota **Arquitetura em Camadas** com separação clara de responsabilidades:

```
LivrariaRosa/
├── Controllers/          # Camada de apresentação — enxuta, sem regras de negócio
├── Services/             # Camada de aplicação — regras de negócio
│   └── Interfaces/       # Contratos (DIP — Dependency Inversion)
├── Repositories/         # Camada de acesso a dados
│   └── Interfaces/       # Contratos (DIP)
├── Models/
│   ├── Entities/         # Entidades do domínio (mapeadas pelo EF Core)
│   └── DTOs/
│       ├── Requests/     # O que o cliente envia
│       └── Responses/    # O que a API retorna (sem expor campos internos)
├── Data/                 # DbContext + Migrations
├── Middlewares/          # Tratamento global de exceções
└── .github/workflows/    # CI/CD — GitHub Actions → Azure
```

**Princípios aplicados:** SRP, DIP (via interfaces + DI nativa do ASP.NET Core),  
Repository Pattern, DTO separado de entidade, Soft Delete, paginação, envelope de resposta padronizado.

---

## Endpoints

### Livros

| Método   | Rota                    | Descrição                        |
|----------|-------------------------|----------------------------------|
| `GET`    | `/api/v1/livros`        | Lista livros com paginação       |
| `GET`    | `/api/v1/livros/{id}`   | Busca livro por ID               |
| `POST`   | `/api/v1/livros`        | Adiciona novo livro              |
| `PUT`    | `/api/v1/livros/{id}`   | Atualiza livro existente         |
| `DELETE` | `/api/v1/livros/{id}`   | Remove livro (soft delete)       |

### Paginação

```
GET /api/v1/livros?pagina=1&tamanhoPagina=10
```

### Formato de Resposta (envelope padronizado)

```json
{
  "sucesso": true,
  "dados": { ... },
  "mensagem": null,
  "erros": null
}
```

---

## Como Executar Localmente

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server LocalDB (instalado com o Visual Studio) **ou** SQL Server Express

### 1. Configurar a connection string

Copie o arquivo de exemplo e preencha com suas credenciais locais:

```bash
cp appsettings.Development.json.example appsettings.Development.json
```

O arquivo `appsettings.Development.json` **não deve ser commitado** (está no `.gitignore`).  
Connection string padrão para LocalDB:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=LivrariaRosaDB;Trusted_Connection=True;"
  }
}
```

### 2. Aplicar Migrations

```bash
dotnet ef database update
```

### 3. Executar

```bash
dotnet run
```

Acesse: **http://localhost:5117/swagger**

---

## Migrations

```bash
# Criar nova migration
dotnet ef migrations add NomeDaMigration

# Aplicar ao banco
dotnet ef database update

# Reverter última migration
dotnet ef migrations remove
```

---

## Deploy no Azure

O deploy é automatizado via GitHub Actions (`.github/workflows/main_livrariarosa.yml`).  
A cada push na branch `main`, o pipeline:

1. Restaura dependências
2. Compila em modo Release
3. Executa testes
4. Publica e faz deploy no Azure App Service `livrariaRosa`

### Secrets necessários no GitHub

| Secret | Descrição |
|--------|-----------|
| `AZUREAPPSERVICE_CLIENTID_...` | Client ID da Managed Identity |
| `AZUREAPPSERVICE_TENANTID_...` | Tenant ID do Azure AD |
| `AZUREAPPSERVICE_SUBSCRIPTIONID_...` | Subscription ID |

### Connection String em Produção

Configure a connection string diretamente no Azure App Service:  
**Configuration → Connection strings → `DefaultConnection`**

> ⚠️ **Nunca commite a senha real no repositório.** Use variáveis de ambiente ou Azure Key Vault em produção.

---

## Exemplos de Requisição

### Criar livro

```http
POST /api/v1/livros
Content-Type: application/json

{
  "titulo": "Clean Code",
  "autor": "Robert C. Martin",
  "isbn": "978-0132350884"
}
```

**Resposta (201 Created):**
```json
{
  "sucesso": true,
  "dados": {
    "id": 5,
    "titulo": "Clean Code",
    "autor": "Robert C. Martin",
    "isbn": "978-0132350884",
    "createdAt": "2026-05-16T00:00:00Z"
  }
}
```

### Erro de validação

```http
POST /api/v1/livros
Content-Type: application/json

{ "titulo": "" }
```

**Resposta (400 Bad Request):**
```json
{
  "sucesso": false,
  "mensagem": "Dados inválidos.",
  "erros": [
    "O campo 'titulo' é obrigatório.",
    "O campo 'autor' é obrigatório."
  ]
}
```
