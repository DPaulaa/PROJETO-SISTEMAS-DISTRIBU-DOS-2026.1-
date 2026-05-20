<h1 align="center" style="color:#c2559c;">Biblioteca Rosa</h1>

<p align="center">
  API REST para controle básico de acervo de uma biblioteca universitária.<br/>
  Desenvolvida em <strong>ASP.NET Core (.NET 10)</strong> como projeto prático da disciplina de <strong>Sistemas Distribuídos — UniBH 2026/1</strong>.
</p>

---

## O que a API faz

A Biblioteca Rosa expõe endpoints HTTP para gerenciar o catálogo de livros de uma biblioteca. Através dela é possível listar, cadastrar, editar e remover livros, além de um endpoint protegido por token para operações autenticadas.

**Funcionalidades implementadas:**
- CRUD completo de livros (`GET`, `POST`, `PUT`, `DELETE`)
- Validação de ISBN duplicado ao cadastrar
- Endpoint protegido com autenticação via header `Authorization: Basic <token>`
- Tratamento centralizado de erros (middleware global)
- Endpoint de diagnóstico com informações do servidor
- Documentação interativa via Swagger (`/swagger`)

---

## Endpoints principais

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/api/v1/livros` | Lista todos os livros |
| GET | `/api/v1/livros/{id}` | Busca um livro por ID |
| POST | `/api/v1/livros` | Cadastra um novo livro |
| PUT | `/api/v1/livros/{id}` | Atualiza um livro existente |
| DELETE | `/api/v1/livros/{id}` | Remove um livro |
| GET | `/api/v1/secure/livros` | Lista livros (requer token) |
| GET | `/api/v1/debug/info` | Informações do servidor |
| GET | `/api/v1/debug/crash` | Simula um erro 500 (apenas para testes) |

Para acessar o endpoint seguro, envie o header:
```
Authorization: Basic <token configurado no appsettings.json>
```

---

## Estrutura do projeto

```
BibliotecaRosa/
├── Controllers/        # Recebem as requisições HTTP e devolvem respostas
├── Services/           # Regras de negócio (validações, lógica)
│   └── Interfaces/     # Contratos que definem o que cada serviço faz
├── Repositories/       # Acesso aos dados (atualmente em memória)
│   └── Interfaces/
├── Models/             # Representação dos dados
│   └── DTOs/           # Formatos de entrada (Request) e saída (Response)
├── Middlewares/        # Tratamento global de erros
├── Exceptions/         # Exceções personalizadas do domínio
└── Program.cs          # Configuração e inicialização da aplicação
```

> **Nota:** Os dados são armazenados em memória e se perdem ao reiniciar o servidor. Para persistência real, basta implementar um repositório com EF Core e trocar o registro no `Program.cs`.

---

## Princípios aplicados (SOLID)

O projeto foi estruturado seguindo os princípios SOLID como exercício prático:

- **S** — Cada classe tem uma única responsabilidade (controller só trata HTTP, service só aplica regras, repository só acessa dados)
- **O** — Novas funcionalidades podem ser adicionadas sem modificar o que já existe (ex: novo tipo de auth cria uma nova classe, não altera a existente)
- **L** — Qualquer implementação de repositório ou serviço pode ser substituída sem quebrar o restante
- **I** — Interfaces separadas por responsabilidade (`ILivroService`, `IAuthService`, `IDiagnosticoService`)
- **D** — Controllers e serviços dependem de interfaces, não de implementações concretas

---

## Alunos

| Matrícula | Nome |
|-----------|------|
| 1221141558 | Lucas Figueiredo de Almeida Castilho Soares |
| 125111410617 | Livia Steise Gaspar Diniz |
| 125111382859 | Augusto Felipe de Paula Coimbra |
| 125111385813 | Bernardo de Paula Dias |
| 125111401298 | Henrique Márcio Dias Alves |
| 125111404838 | Luiz Guilherme Vilaça de Moraes |
