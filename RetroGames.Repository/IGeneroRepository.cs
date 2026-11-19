using RetroGames.Model;

namespace RetroGames.Repository;

public interface IGeneroRepository
{
    Task<IEnumerable<Genero>> ListarAsync();
}