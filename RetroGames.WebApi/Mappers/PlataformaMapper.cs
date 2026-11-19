using RetroGames.Model;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.Mappers;

public static class PlataformaMapper
{
    public static ListarPlataformaResponseApiModel ParaResponse(this Plataforma plataforma)
    {
        return new ListarPlataformaResponseApiModel
        {
            Id = plataforma.Id,
            Nome = plataforma.Nome,
            Fabricante = plataforma.Fabricante,
            AnoLancamento = plataforma.AnoLancamento
        };
    }

    public static List<ListarPlataformaResponseApiModel> ParaResponse(this IEnumerable<Plataforma> plataformas)
    {
        return plataformas.Select(p => p.ParaResponse()).ToList();
    }
}