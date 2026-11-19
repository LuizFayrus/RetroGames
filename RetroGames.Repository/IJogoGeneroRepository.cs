using RetroGames.Model;

namespace RetroGames.Repository;

public interface IJogoGeneroRepository
{
    Task<IEnumerable<JogoGenero>> ListarTodosAsync();
    Task<IEnumerable<JogoGenero>> ListarPorJogosAsync(IEnumerable<int> idsJogos);
    Task InserirAsync(int idJogo, IEnumerable<int> idsGeneros);
    Task RemoverPorJogoAsync(int idJogo);
}