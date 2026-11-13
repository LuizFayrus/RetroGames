using RetroGames.Model;

namespace RetroGames.Repository;

public interface IJogoRepository
{
    Task<IEnumerable<Jogo>> ListarAsync();
    Task<Jogo?> BuscarPorIdAsync(int id);
    Task<bool> ExisteComNomeNaPlataformaAsync(string nome, int plataformaId, int? idIgnorar = null);
    Task<int> InserirAsync(Jogo jogo);
    Task AlterarAsync(Jogo jogo);
    Task ExcluirAsync(int id);
}