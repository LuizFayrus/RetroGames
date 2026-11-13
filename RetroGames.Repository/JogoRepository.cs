using MySqlConnector;
using RetroGames.Model;

namespace RetroGames.Repository;

public class JogoRepository : RepositoryBase, IJogoRepository
{
    public JogoRepository(RepositoryConfig config) : base(config) { }

    public async Task<IEnumerable<Jogo>> ListarAsync()
    {
        var jogos = new List<Jogo>();
        using var conexao = await AbrirConexaoAsync();
        using var comando = CriarComando(conexao, @"
            SELECT j.id, j.nm_jogo, j.ds_descricao, j.nr_ano_lancamento, j.plataforma_id,
                   p.nm_plataforma, p.ds_fabricante, p.nr_ano_lancamento as plataforma_ano
            FROM tb_jogo j
            INNER JOIN tb_plataforma p ON j.plataforma_id = p.id
            ORDER BY j.nm_jogo;");
        
        using var leitor = await comando.ExecuteReaderAsync();
        while (await leitor.ReadAsync())
        {
            jogos.Add(new Jogo
            {
                Id = leitor.GetInt32("id"),
                Nome = leitor.GetString("nm_jogo"),
                Descricao = leitor.IsDBNull(leitor.GetOrdinal("ds_descricao")) ? null : leitor.GetString("ds_descricao"),
                AnoLancamento = leitor.IsDBNull(leitor.GetOrdinal("nr_ano_lancamento")) ? null : leitor.GetInt32("nr_ano_lancamento"),
                PlataformaId = leitor.GetInt32("plataforma_id"),
                Plataforma = new Plataforma
                {
                    Id = leitor.GetInt32("plataforma_id"),
                    Nome = leitor.GetString("nm_plataforma"),
                    Fabricante = leitor.IsDBNull(leitor.GetOrdinal("ds_fabricante")) ? null : leitor.GetString("ds_fabricante"),
                    AnoLancamento = leitor.IsDBNull(leitor.GetOrdinal("plataforma_ano")) ? null : leitor.GetInt32("plataforma_ano")
                }
            });
        }
        return jogos;
    }

    public async Task<Jogo?> BuscarPorIdAsync(int id)
    {
        using var conexao = await AbrirConexaoAsync();
        using var comando = CriarComando(conexao, @"
            SELECT j.id, j.nm_jogo, j.ds_descricao, j.nr_ano_lancamento, j.plataforma_id,
                   p.nm_plataforma, p.ds_fabricante, p.nr_ano_lancamento as plataforma_ano
            FROM tb_jogo j
            INNER JOIN tb_plataforma p ON j.plataforma_id = p.id
            WHERE j.id = @id;");
        AdicionarParametro(comando, "@id", id);

        using var leitor = await comando.ExecuteReaderAsync();
        if (await leitor.ReadAsync())
        {
            return new Jogo
            {
                Id = leitor.GetInt32("id"),
                Nome = leitor.GetString("nm_jogo"),
                Descricao = leitor.IsDBNull(leitor.GetOrdinal("ds_descricao")) ? null : leitor.GetString("ds_descricao"),
                AnoLancamento = leitor.IsDBNull(leitor.GetOrdinal("nr_ano_lancamento")) ? null : leitor.GetInt32("nr_ano_lancamento"),
                PlataformaId = leitor.GetInt32("plataforma_id"),
                Plataforma = new Plataforma
                {
                    Id = leitor.GetInt32("plataforma_id"),
                    Nome = leitor.GetString("nm_plataforma"),
                    Fabricante = leitor.IsDBNull(leitor.GetOrdinal("ds_fabricante")) ? null : leitor.GetString("ds_fabricante"),
                    AnoLancamento = leitor.IsDBNull(leitor.GetOrdinal("plataforma_ano")) ? null : leitor.GetInt32("plataforma_ano")
                }
            };
        }
        return null;
    }

    public async Task<bool> ExisteComNomeNaPlataformaAsync(string nome, int plataformaId, int? idIgnorar = null)
    {
        using var conexao = await AbrirConexaoAsync();
        var sql = "SELECT COUNT(1) FROM tb_jogo WHERE nm_jogo = @nome AND plataforma_id = @plataformaId";
        if (idIgnorar.HasValue)
        {
            sql += " AND id <> @idIgnorar";
        }

        using var comando = CriarComando(conexao, sql);
        AdicionarParametro(comando, "@nome", nome);
        AdicionarParametro(comando, "@plataformaId", plataformaId);
        if (idIgnorar.HasValue)
        {
            AdicionarParametro(comando, "@idIgnorar", idIgnorar.Value);
        }

        var resultado = await comando.ExecuteScalarAsync();
        return Convert.ToInt64(resultado) > 0;
    }

    public async Task<int> InserirAsync(Jogo jogo)
    {
        using var conexao = await AbrirConexaoAsync();
        using var comando = CriarComando(conexao, @"
            INSERT INTO tb_jogo (nm_jogo, ds_descricao, nr_ano_lancamento, plataforma_id) 
            VALUES (@nome, @descricao, @ano, @plataformaId); 
            SELECT LAST_INSERT_ID();");
        
        AdicionarParametro(comando, "@nome", jogo.Nome);
        AdicionarParametro(comando, "@descricao", jogo.Descricao);
        AdicionarParametro(comando, "@ano", jogo.AnoLancamento);
        AdicionarParametro(comando, "@plataformaId", jogo.PlataformaId);

        var idGerado = await comando.ExecuteScalarAsync();
        return Convert.ToInt32(idGerado);
    }

    public async Task AlterarAsync(Jogo jogo)
    {
        using var conexao = await AbrirConexaoAsync();
        using var comando = CriarComando(conexao, @"
            UPDATE tb_jogo 
            SET nm_jogo = @nome, ds_descricao = @descricao, nr_ano_lancamento = @ano, plataforma_id = @plataformaId 
            WHERE id = @id;");
        
        AdicionarParametro(comando, "@id", jogo.Id);
        AdicionarParametro(comando, "@nome", jogo.Nome);
        AdicionarParametro(comando, "@descricao", jogo.Descricao);
        AdicionarParametro(comando, "@ano", jogo.AnoLancamento);
        AdicionarParametro(comando, "@plataformaId", jogo.PlataformaId);

        await ExecutarGravacaoAsync(comando);
    }

    public async Task ExcluirAsync(int id)
    {
        using var conexao = await AbrirConexaoAsync();
        using var comando = CriarComando(conexao, "DELETE FROM tb_jogo WHERE id = @id;");
        AdicionarParametro(comando, "@id", id);

        await ExecutarGravacaoAsync(comando);
    }
}