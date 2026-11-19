using RetroGames.Model;

namespace RetroGames.Repository;

public class JogoGeneroRepository : RepositoryBase, IJogoGeneroRepository
{
    public JogoGeneroRepository(RepositoryConfig config) : base(config)
    {
    }

    public async Task<IEnumerable<JogoGenero>> ListarTodosAsync()
    {
        var lista = new List<JogoGenero>();
        const string sql = "SELECT co_jogo, co_genero FROM tb_jogo_genero;";

        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, sql);
        await using var leitor = await comando.ExecuteReaderAsync();

        while (await leitor.ReadAsync())
        {
            lista.Add(new JogoGenero
            {
                IdJogo = leitor.GetInt32("co_jogo"),
                IdGenero = leitor.GetInt32("co_genero")
            });
        }

        return lista;
    }

    public async Task<IEnumerable<JogoGenero>> ListarPorJogosAsync(IEnumerable<int> idsJogos)
    {
        var ids = idsJogos.Distinct().ToList();
        var lista = new List<JogoGenero>();

        if (ids.Count == 0)
            return lista;

        var parametros = ids.Select((_, i) => $"@id{i}").ToList();
        var sql = $"SELECT co_jogo, co_genero FROM tb_jogo_genero WHERE co_jogo IN ({string.Join(", ", parametros)});";

        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, sql);

        for (int i = 0; i < ids.Count; i++)
        {
            AdicionarParametro(comando, parametros[i], ids[i]);
        }

        await using var leitor = await comando.ExecuteReaderAsync();
        while (await leitor.ReadAsync())
        {
            lista.Add(new JogoGenero
            {
                IdJogo = leitor.GetInt32("co_jogo"),
                IdGenero = leitor.GetInt32("co_genero")
            });
        }

        return lista;
    }

    public async Task InserirAsync(int idJogo, IEnumerable<int> idsGeneros)
    {
        const string sql = "INSERT INTO tb_jogo_genero (co_jogo, co_genero) VALUES (@coJogo, @coGenero);";

        await using var conexao = await AbrirConexaoAsync();

        foreach (var idGenero in idsGeneros.Distinct())
        {
            await using var comando = CriarComando(conexao, sql);
            AdicionarParametro(comando, "@coJogo", idJogo);
            AdicionarParametro(comando, "@coGenero", idGenero);
            await ExecutarGravacaoAsync(comando);
        }
    }

    public async Task RemoverPorJogoAsync(int idJogo)
    {
        const string sql = "DELETE FROM tb_jogo_genero WHERE co_jogo = @coJogo;";

        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, sql);
        AdicionarParametro(comando, "@coJogo", idJogo);
        await ExecutarGravacaoAsync(comando);
    }
}