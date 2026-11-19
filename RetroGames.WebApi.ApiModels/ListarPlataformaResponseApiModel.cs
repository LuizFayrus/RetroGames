namespace RetroGames.WebApi.ApiModels;

public class ListarPlataformaResponseApiModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Fabricante { get; set; }
    public int? AnoLancamento { get; set; }
}