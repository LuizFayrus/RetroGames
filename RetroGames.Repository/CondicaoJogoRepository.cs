using MySqlConnector;
using RetroGames.Model;

namespace RetroGames.Repository;

public class CondicaoJogoRepository : RepositoryBase, ICondicaoJogoRepository
{
    public CondicaoJogoRepository(RepositoryConfig config) : base(config) { }

    public async Task<IEnumerable<CondicaoJogo>> ListarAsync()
    {
        var lista = new List<CondicaoJogo>();
        using var conexao = await AbrirConexaoAsync();
        using var comando = CriarComando(conexao, "SELECT id, nm_condicao, ds_descricao FROM tb_condicao_jogo ORDER BY nm_condicao;");
        
        using var leitor = await comando.ExecuteReaderAsync();
        while (await leitor.ReadAsync())
        {
            lista.Add(new CondicaoJogo
            {
                Id = leitor.GetInt32("id"),
                Nome = leitor.GetString("nm_condicao"),
                Descricao = leitor.IsDBNull(leitor.GetOrdinal("ds_descricao")) ? null : leitor.GetString("ds_descricao")
            });
        }
        return lista;
    }

    public async Task<CondicaoJogo?> BuscarPorIdAsync(int id)
    {
        using var conexao = await AbrirConexaoAsync();
        using var comando = CriarComando(conexao, "SELECT id, nm_condicao, ds_descricao FROM tb_condicao_jogo WHERE id = @id;");
        AdicionarParametro(comando, "@id", id);

        using var leitor = await comando.ExecuteReaderAsync();
        if (await leitor.ReadAsync())
        {
            return new CondicaoJogo
            {
                Id = leitor.GetInt32("id"),
                Nome = leitor.GetString("nm_condicao"),
                Descricao = leitor.IsDBNull(leitor.GetOrdinal("ds_descricao")) ? null : leitor.GetString("ds_descricao")
            };
        }
        return null;
    }

    public async Task<bool> ExisteComNomeAsync(string nome, int? idIgnorar = null)
    {
        using var conexao = await AbrirConexaoAsync();
        var sql = "SELECT COUNT(1) FROM tb_condicao_jogo WHERE nm_condicao = @nome";
        if (idIgnorar.HasValue) sql += " AND id <> @idIgnorar";

        using var comando = CriarComando(conexao, sql);
        AdicionarParametro(comando, "@nome", nome);
        if (idIgnorar.HasValue) AdicionarParametro(comando, "@idIgnorar", idIgnorar.Value);

        var resultado = await comando.ExecuteScalarAsync();
        return Convert.ToInt64(resultado) > 0;
    }

    public async Task<int> InserirAsync(CondicaoJogo condicao)
    {
        using var conexao = await AbrirConexaoAsync();
        using var comando = CriarComando(conexao, "INSERT INTO tb_condicao_jogo (nm_condicao, ds_descricao) VALUES (@nome, @descricao); SELECT LAST_INSERT_ID();");
        
        AdicionarParametro(comando, "@nome", condicao.Nome);
        AdicionarParametro(comando, "@descricao", condicao.Descricao);

        var idGerado = await comando.ExecuteScalarAsync();
        return Convert.ToInt32(idGerado);
    }

    public async Task AlterarAsync(CondicaoJogo condicao)
    {
        using var conexao = await AbrirConexaoAsync();
        using var comando = CriarComando(conexao, "UPDATE tb_condicao_jogo SET nm_condicao = @nome, ds_descricao = @descricao WHERE id = @id;");
        
        AdicionarParametro(comando, "@id", condicao.Id);
        AdicionarParametro(comando, "@nome", condicao.Nome);
        AdicionarParametro(comando, "@descricao", condicao.Descricao);

        await ExecutarGravacaoAsync(comando);
    }

    public async Task ExcluirAsync(int id)
    {
        using var conexao = await AbrirConexaoAsync();
        using var comando = CriarComando(conexao, "DELETE FROM tb_condicao_jogo WHERE id = @id;");
        AdicionarParametro(comando, "@id", id);

        await ExecutarGravacaoAsync(comando);
    }
}