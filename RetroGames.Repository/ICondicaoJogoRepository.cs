using RetroGames.Model;

namespace RetroGames.Repository;

public interface ICondicaoJogoRepository
{
    Task<IEnumerable<CondicaoJogo>> ListarAsync();
    Task<CondicaoJogo?> BuscarPorIdAsync(int id);
    Task<bool> ExisteComNomeAsync(string nome, int? idIgnorar = null);
    Task<int> InserirAsync(CondicaoJogo condicao);
    Task AlterarAsync(CondicaoJogo condicao);
    Task ExcluirAsync(int id);
}