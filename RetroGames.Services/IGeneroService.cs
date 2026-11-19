using RetroGames.Model;

namespace RetroGames.Services;

public interface IGeneroService
{
    Task<IEnumerable<Genero>> ListarAsync();
}