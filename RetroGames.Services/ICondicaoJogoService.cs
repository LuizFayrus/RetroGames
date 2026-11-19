using RetroGames.Model;

namespace RetroGames.Services;

public interface ICondicaoJogoService
{
    Task<IEnumerable<CondicaoJogo>> ListarAsync();
}