using System.Transactions;
using RetroGames.Model;
using RetroGames.Repository;

namespace RetroGames.Services;

public class JogoService : IJogoService
{
    private readonly IRepositorySet _repositorio;

    public JogoService(IRepositorySet repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<JogoDetalheServiceModel>> ListarAsync(int? generoId = null, int? plataformaId = null, string? nome = null)
    {
        var jogos = (await _repositorio.Jogo.ListarAsync()).ToList();

        if (plataformaId.HasValue)
            jogos = jogos.Where(j => j.PlataformaId == plataformaId.Value).ToList();

        if (!string.IsNullOrWhiteSpace(nome))
            jogos = jogos.Where(j => j.Nome.Contains(nome.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();

        if (generoId.HasValue)
        {
            var todosVinculos = await _repositorio.JogoGenero.ListarTodosAsync();
            var idsJogosDoGenero = todosVinculos
                .Where(jg => jg.IdGenero == generoId.Value)
                .Select(jg => jg.IdJogo)
                .ToHashSet();

            jogos = jogos.Where(j => idsJogosDoGenero.Contains(j.Id)).ToList();
        }

        return await MontarDetalhesAsync(jogos);
    }

    public async Task<JogoDetalheServiceModel> BuscarPorIdAsync(int id)
    {
        var jogo = await _repositorio.Jogo.BuscarPorIdAsync(id);
        if (jogo is null)
            throw new BusinessRuleException(BusinessErrors.JOGO_NAO_ENCONTRADO);

        var detalhes = await MontarDetalhesAsync([jogo]);
        return detalhes.First();
    }

    public async Task<JogoDetalheServiceModel> InserirAsync(Jogo jogo, IEnumerable<int> idsGeneros)
    {
        var listaGeneros = idsGeneros.Distinct().ToList();
        await ValidarReferenciasAsync(jogo.PlataformaId, listaGeneros);

        jogo.Nome = jogo.Nome.Trim();

        using (var transacao = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            jogo.Id = await _repositorio.Jogo.InserirAsync(jogo);
            await _repositorio.JogoGenero.InserirAsync(jogo.Id, listaGeneros);
            transacao.Complete();
        }

        return await BuscarPorIdAsync(jogo.Id);
    }

    public async Task AlterarAsync(Jogo jogo, IEnumerable<int> idsGeneros)
    {
        var existente = await _repositorio.Jogo.BuscarPorIdAsync(jogo.Id);
        if (existente is null)
            throw new BusinessRuleException(BusinessErrors.JOGO_NAO_ENCONTRADO);

        var listaGeneros = idsGeneros.Distinct().ToList();
        await ValidarReferenciasAsync(jogo.PlataformaId, listaGeneros);

        jogo.Nome = jogo.Nome.Trim();

        using (var transacao = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            await _repositorio.Jogo.AlterarAsync(jogo);
            await _repositorio.JogoGenero.RemoverPorJogoAsync(jogo.Id);
            await _repositorio.JogoGenero.InserirAsync(jogo.Id, listaGeneros);
            transacao.Complete();
        }
    }

    public async Task DesativarAsync(int id)
    {
        var existente = await _repositorio.Jogo.BuscarPorIdAsync(id);
        if (existente is null)
            throw new BusinessRuleException(BusinessErrors.JOGO_NAO_ENCONTRADO);

        await _repositorio.Jogo.ExcluirAsync(id);
    }

    private async Task ValidarReferenciasAsync(int plataformaId, List<int> idsGeneros)
    {
        var plataforma = await _repositorio.Plataforma.BuscarPorIdAsync(plataformaId);
        if (plataforma is null)
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NAO_ENCONTRADA);

        if (idsGeneros.Count > 0)
        {
            var todosGeneros = await _repositorio.Genero.ListarAsync();
            var idsValidos = todosGeneros.Select(g => g.Id).ToHashSet();

            if (idsGeneros.Any(id => !idsValidos.Contains(id)))
                throw new BusinessRuleException(BusinessErrors.GENERO_NAO_ENCONTRADO);
        }
    }

    private async Task<List<JogoDetalheServiceModel>> MontarDetalhesAsync(List<Jogo> jogos)
    {
        if (jogos.Count == 0)
            return [];

        var idsJogos = jogos.Select(j => j.Id).ToList();
        var plataformas = (await _repositorio.Plataforma.ListarAsync()).ToDictionary(p => p.Id, p => p.Nome);
        var todosGeneros = (await _repositorio.Genero.ListarAsync()).ToDictionary(g => g.Id);
        var vinculos = await _repositorio.JogoGenero.ListarPorJogosAsync(idsJogos);

        var generosPorJogo = vinculos
            .GroupBy(v => v.IdJogo)
            .ToDictionary(
                g => g.Key,
                g => g.Select(v => todosGeneros.GetValueOrDefault(v.IdGenero))
                      .Where(gen => gen is not null)
                      .Cast<Genero>()
                      .ToList()
            );

        return jogos.Select(jogo => new JogoDetalheServiceModel
        {
            Jogo = jogo,
            NomePlataforma = plataformas.GetValueOrDefault(jogo.PlataformaId) ?? jogo.Plataforma?.Nome ?? string.Empty,
            Generos = generosPorJogo.GetValueOrDefault(jogo.Id) ?? []
        }).ToList();
    }
}