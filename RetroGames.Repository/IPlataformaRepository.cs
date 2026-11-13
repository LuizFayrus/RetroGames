using RetroGames.Model;

namespace RetroGames.Repository;

public interface IPlataformaRepository
{
    Task<IEnumerable<Plataforma>> ListarAsync();
    Task<Plataforma?> BuscarPorIdAsync(int id);
    Task<bool> ExisteComNomeAsync(string nome, int? idIgnorar = null);
    Task<bool> PossuiJogosAsync(int id);
    Task<int> InserirAsync(Plataforma plataforma);
    Task AlterarAsync(Plataforma plataforma);
    Task ExcluirAsync(int id);
}