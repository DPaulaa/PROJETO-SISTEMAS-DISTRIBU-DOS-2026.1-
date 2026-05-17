namespace LivrariaRosa.Models.DTOs.Responses;

public class ApiResponse<T>
{
    public bool Sucesso { get; set; }
    public T? Dados { get; set; }
    public string? Mensagem { get; set; }
    public IEnumerable<string>? Erros { get; set; }

    public static ApiResponse<T> Ok(T dados) =>
        new() { Sucesso = true, Dados = dados };

    public static ApiResponse<T> Falha(string mensagem, IEnumerable<string>? erros = null) =>
        new() { Sucesso = false, Mensagem = mensagem, Erros = erros };
}
