using System.ComponentModel.DataAnnotations;

namespace BibliotecaRosa.Models.DTOs;

public class EmprestimoRequest
{
    [Required(ErrorMessage = "É necessário selecionar um usuário.")]
    public int UsuarioId { get; set; }

    [Required(ErrorMessage = "É necessário selecionar um livro.")]
    public int LivroId { get; set; }
}

public class EmprestimoResponse
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string UsuarioNome { get; set; } = string.Empty;
    public string UsuarioRole { get; set; } = string.Empty;
    public int LivroId { get; set; }
    public string LivroTitulo { get; set; } = string.Empty;
    public DateTime DataEmprestimo { get; set; }
    public DateTime? DataDevolucao { get; set; }
    public DateTime? DataDevolucaoPrevista { get; set; }
}

public class RelatorioLivroDto
{
    public int LivroId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public int TotalEmprestimos { get; set; }
}

public class EmprestimoAdminRequest
{
    [Required(ErrorMessage = "É necessário selecionar um usuário.")]
    public int UsuarioId { get; set; }

    [Required(ErrorMessage = "É necessário selecionar um livro.")]
    public int LivroId { get; set; }
}
