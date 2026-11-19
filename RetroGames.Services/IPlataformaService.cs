using RetroGames.Model;

namespace RetroGames.Services;

public interface IPlataformaService
{
    Task<IEnumerable<Plataforma>> ListarAsync();
    Task<Plataforma> BuscarPorIdAsync(int id);
    Task<Plataforma> InserirAsync(Plataforma plataforma);
    Task AlterarAsync(Plataforma plataforma);
    Task ExcluirAsync(int id);
}