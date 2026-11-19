using RetroGames.Model;
using RetroGames.Repository;

namespace RetroGames.Services;

public class GeneroService : IGeneroService
{
    private readonly IRepositorySet _repositorio;

    public GeneroService(IRepositorySet repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<Genero>> ListarAsync()
    {
        return await _repositorio.Genero.ListarAsync();
    }
}