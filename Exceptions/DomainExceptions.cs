// Exceptions/DomainExceptions.cs
//
// Exceções personalizadas para erros do domínio da aplicação.
// Quando lançadas, o middleware as captura e devolve o status HTTP correto: RecursoNaoEncontrado → 404 | RegraDeNegocio → 400
namespace BibliotecaRosa.Exceptions;

public class RecursoNaoEncontradoException : Exception
{
    public RecursoNaoEncontradoException(string mensagem) : base(mensagem) { }
}

public class RegraDeNegocioException : Exception
{
    public RegraDeNegocioException(string mensagem) : base(mensagem) { }
}

public class ConflitoException : Exception
    {
        public ConflitoException(string mensagem) : base(mensagem) { }
    }