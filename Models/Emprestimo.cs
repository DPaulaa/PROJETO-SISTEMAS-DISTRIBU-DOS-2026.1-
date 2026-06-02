// Models/Emprestimo.cs
//
// CORREÇÃO: Livro e Pessoa eram inicializados com "new Livro()" / "new Pessoa()".
// O EF Core não distingue "objeto não carregado" de "objeto vazio" quando a
// propriedade tem um valor default diferente de null — isso impedia o Include()
// de funcionar corretamente. As propriedades agora são nullable com null! (null-forgiving)
// para indicar que o EF Core é responsável por preenchê-las.

namespace BibliotecaRosa.Models;

public class Emprestimo
{
    public int Id { get; set; }

    // EF Core preenche estas propriedades via Include() — não inicializar com new()
    public Livro Livro { get; set; } = null!;
    public Pessoa Pessoa { get; set; } = null!;

    public DateTime DataEmprestimo { get; set; } = DateTime.UtcNow;
    public DateTime? DataDevolucao { get; set; }
    public DateTime? DataDevolucaoPrevista { get; set; }
}
