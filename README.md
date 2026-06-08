# Biblioteca Rosa 📚

Sistema de gerenciamento de empréstimos de livros — Projeto de Sistemas Distribuídos (2026/1).

---

## Arquitetura

```
Controllers/        → Camada HTTP: recebe requisições, retorna respostas
Services/           → Regras de negócio
  Interfaces/       → Contratos dos serviços
  Validation/       → Validações de domínio (empréstimos)
Repositories/       → Acesso ao banco de dados (EF Core)
  Interfaces/       → Contratos dos repositórios
Models/             → Entidades e DTOs
  DTOs/             → Objetos de transferência (Request/Response)
Migrations/         → Histórico de schema do banco
Data/               → AppDbContext
Middlewares/        → Tratamento global de exceções
Enums/              → Enumerações (Role)
Exceptions/         → Exceções de domínio customizadas
```

**Stack:** .NET 10 · Entity Framework Core · SQL Server · JWT Bearer · BCrypt · Swagger

---

## Autenticação JWT

### Fluxo

1. `POST /api/v1/auth/login` com `{ email, senha }`
2. API retorna um token JWT
3. Use o token no header: `Authorization: Bearer <token>`
4. Token expira em 8 horas

### No Swagger

Clique em **Authorize** (cadeado) → Cole o token → **Authorize**.

### Configuração (User Secrets — produção)

```bash
dotnet user-secrets set "Jwt:Key" "sua-chave-secreta-forte-minimo-32-chars"
```

> ⚠️ Nunca commite a chave JWT real no repositório.

---

## Endpoints

### 🔐 Autenticação (`/api/v1/auth`)

| Método | Rota | Descrição | Auth |
|--------|------|-----------|------|
| POST | `/login` | Gera token JWT | Público |
| POST | `/registrar` | Cadastra novo usuário | Público |

### 👤 Usuários (`/api/v1/usuarios`)

| Método | Rota | Descrição | Auth |
|--------|------|-----------|------|
| GET | `/` | Lista todos os usuários | Admin |
| GET | `/{id}` | Busca usuário por ID | Admin ou próprio |
| PUT | `/{id}` | Atualiza usuário | Admin ou próprio |
| DELETE | `/{id}` | Remove usuário | Admin |
| GET | `/{id}/emprestimos` | Empréstimos de um usuário | Admin |

### 📖 Livros (`/api/v1/livros`)

| Método | Rota | Descrição | Auth |
|--------|------|-----------|------|
| GET | `/` | Lista todos os livros | Autenticado |
| GET | `/{id}` | Busca livro por ID | Autenticado |
| POST | `/` | Cadastra livro | Admin |
| PUT | `/{id}` | Atualiza livro | Admin |
| DELETE | `/{id}` | Remove livro | Admin |

### 📋 Empréstimos (`/api/v1/emprestimos`)

| Método | Rota | Descrição | Auth |
|--------|------|-----------|------|
| GET | `/meus-emprestimos` | Empréstimos ativos do usuário logado | Autenticado |
| GET | `/historico` | Histórico: usuário vê só o seu, admin vê tudo | Autenticado |
| GET | `/{id}` | Busca empréstimo por ID | Autenticado |
| POST | `/emprestar` | Realiza um empréstimo | Autenticado |
| PUT | `/devolver/{id}` | Registra devolução | Autenticado |
| GET | `/admin/todos` | Lista todos (incluindo devolvidos) | Admin |
| PUT | `/admin/{id}/forcar-devolucao` | Força devolução | Admin |
| GET | `/admin/relatorio/mais-emprestados` | Livros mais emprestados | Admin |

---

## Regras de Negócio

### Limites de empréstimos por perfil

| Perfil | Limite ativo | Prazo de devolução |
|--------|-------------|-------------------|
| Aluno | 3 | 10 dias |
| Professor | 5 | 30 dias |
| Administrador | Sem limite | 30 dias |

### Validações de empréstimo

- ✅ Livro deve ter `QuantidadeDisponivel > 0`
- ✅ Usuário não pode ter o mesmo livro emprestado duas vezes simultaneamente
- ✅ Usuário não pode ultrapassar seu limite de empréstimos ativos
- ✅ Empréstimos são imutáveis (sem delete físico)

### Permissões especiais do Admin

- Pode emprestar livros para qualquer usuário (passando `UsuarioId` no body)
- Pode forçar devolução de qualquer empréstimo
- Acessa histórico completo de todos os usuários
- Visualiza relatório de livros mais emprestados

---

## Usuários de teste (seed)

| Nome | E-mail | Senha | Role |
|------|--------|-------|------|
| Admin Rosa | admin@rosa.com | admin123 | Administrador |
| Professor Girafales | professor@rosa.com | admin123 | Professor |
| Aluno Chaves | aluno@rosa.com | admin123 | Aluno |

> Senha padrão: `admin123` (hash BCrypt pré-gerado no seed)

---

## Como rodar localmente

```bash
# Restaurar dependências
dotnet restore

# Configurar User Secrets (JWT)
dotnet user-secrets set "Jwt:Key" "chave-secreta-forte-minimo-32-chars!!"

# Rodar (migrations são aplicadas automaticamente)
dotnet run

# Acessar Swagger
# http://localhost:5000/swagger
```
