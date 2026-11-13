namespace RetroGames.Repository;

public class RegistroDuplicadoException : Exception
{
    public RegistroDuplicadoException(string mensagem) : base(mensagem) { }

    public RegistroDuplicadoException(string mensagem, Exception innerException) 
        : base(mensagem, innerException) { }
}