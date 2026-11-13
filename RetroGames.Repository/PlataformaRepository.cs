using MySqlConnector;
using RetroGames.Model;

namespace RetroGames.Repository;

public class PlataformaRepository : RepositoryBase, IPlataformaRepository
{
    public PlataformaRepository(RepositoryConfig config) : base(config) { }

    public async Task<IEnumerable<Plataforma>> ListarAsync()
    {
        var plataformas = new List<Plataforma>();
        using var conexao = await AbrirConexaoAsync();
        using var comando = CriarComando(conexao, "SELECT id, nm_plataforma, ds_fabricante, nr_ano_lancamento FROM tb_plataforma ORDER BY nm_plataforma;");
        
        using var leitor = await comando.ExecuteReaderAsync();
        while (await leitor.ReadAsync())
        {
            plataformas.Add(new Plataforma
            {
                Id = leitor.GetInt32("id"),
                Nome = leitor.GetString("nm_plataforma"),
                Fabricante = leitor.IsDBNull(leitor.GetOrdinal("ds_fabricante")) ? null : leitor.GetString("ds_fabricante"),
                AnoLancamento = leitor.IsDBNull(leitor.GetOrdinal("nr_ano_lancamento")) ? null : leitor.GetInt32("nr_ano_lancamento")
            });
        }
        return plataformas;
    }

    public async Task<Plataforma?> BuscarPorIdAsync(int id)
    {
        using var conexao = await AbrirConexaoAsync();
        using var comando = CriarComando(conexao, "SELECT id, nm_plataforma, ds_fabricante, nr_ano_lancamento FROM tb_plataforma WHERE id = @id;");
        AdicionarParametro(comando, "@id", id);

        using var leitor = await comando.ExecuteReaderAsync();
        if (await leitor.ReadAsync())
        {
            return new Plataforma
            {
                Id = leitor.GetInt32("id"),
                Nome = leitor.GetString("nm_plataforma"),
                Fabricante = leitor.IsDBNull(leitor.GetOrdinal("ds_fabricante")) ? null : leitor.GetString("ds_fabricante"),
                AnoLancamento = leitor.IsDBNull(leitor.GetOrdinal("nr_ano_lancamento")) ? null : leitor.GetInt32("nr_ano_lancamento")
            };
        }
        return null;
    }

    public async Task<bool> ExisteComNomeAsync(string nome, int? idIgnorar = null)
    {
        using var conexao = await AbrirConexaoAsync();
        var sql = "SELECT COUNT(1) FROM tb_plataforma WHERE nm_plataforma = @nome";
        if (idIgnorar.HasValue)
        {
            sql += " AND id <> @idIgnorar";
        }

        using var comando = CriarComando(conexao, sql);
        AdicionarParametro(comando, "@nome", nome);
        if (idIgnorar.HasValue)
        {
            AdicionarParametro(comando, "@idIgnorar", idIgnorar.Value);
        }

        var resultado = await comando.ExecuteScalarAsync();
        return Convert.ToInt64(resultado) > 0;
    }

    public async Task<bool> PossuiJogosAsync(int id)
    {
        using var conexao = await AbrirConexaoAsync();
        using var comando = CriarComando(conexao, "SELECT COUNT(1) FROM tb_jogo WHERE plataforma_id = @id;");
        AdicionarParametro(comando, "@id", id);

        var resultado = await comando.ExecuteScalarAsync();
        return Convert.ToInt64(resultado) > 0;
    }

    public async Task<int> InserirAsync(Plataforma plataforma)
    {
        using var conexao = await AbrirConexaoAsync();
        using var comando = CriarComando(conexao, "INSERT INTO tb_plataforma (nm_plataforma, ds_fabricante, nr_ano_lancamento) VALUES (@nome, @fabricante, @ano); SELECT LAST_INSERT_ID();");
        
        AdicionarParametro(comando, "@nome", plataforma.Nome);
        AdicionarParametro(comando, "@fabricante", plataforma.Fabricante);
        AdicionarParametro(comando, "@ano", plataforma.AnoLancamento);

        var idGerado = await comando.ExecuteScalarAsync();
        return Convert.ToInt32(idGerado);
    }

    public async Task AlterarAsync(Plataforma plataforma)
    {
        using var conexao = await AbrirConexaoAsync();
        using var comando = CriarComando(conexao, "UPDATE tb_plataforma SET nm_plataforma = @nome, ds_fabricante = @fabricante, nr_ano_lancamento = @ano WHERE id = @id;");
        
        AdicionarParametro(comando, "@id", plataforma.Id);
        AdicionarParametro(comando, "@nome", plataforma.Nome);
        AdicionarParametro(comando, "@fabricante", plataforma.Fabricante);
        AdicionarParametro(comando, "@ano", plataforma.AnoLancamento);

        await ExecutarGravacaoAsync(comando);
    }

    public async Task ExcluirAsync(int id)
    {
        using var conexao = await AbrirConexaoAsync();
        using var comando = CriarComando(conexao, "DELETE FROM tb_plataforma WHERE id = @id;");
        AdicionarParametro(comando, "@id", id);

        await ExecutarGravacaoAsync(comando);
    }
}