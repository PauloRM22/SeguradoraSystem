public class PropostaResponse
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
}