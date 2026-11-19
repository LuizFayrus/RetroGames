using RetroGames.Model;

namespace RetroGames.Services;

public interface IJogoService
{
    Task<IEnumerable<JogoDetalheServiceModel>> ListarAsync(int? generoId = null, int? plataformaId = null, string? nome = null);
    Task<JogoDetalheServiceModel> BuscarPorIdAsync(int id);
    Task<JogoDetalheServiceModel> InserirAsync(Jogo jogo, IEnumerable<int> idsGeneros);
    Task AlterarAsync(Jogo jogo, IEnumerable<int> idsGeneros);
    Task DesativarAsync(int id);
}