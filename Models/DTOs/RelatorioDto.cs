namespace BibliotecaRosa.Models.DTOs;

public class RelatorioLivroDto
{
    public int LivroId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public int TotalEmprestimos { get; set; }
}
