namespace RetroGames.Model;

public class Jogo
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public int? AnoLancamento { get; set; }
    public int PlataformaId { get; set; }
    public Plataforma Plataforma { get; set; } = null!;
}