namespace Control_Finance.Server.DTO
{
    public class SecurityDTO
    {
        public required int Id { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public string? TokenJWT { get; set; }
        public DateOnly DataNascimento { get; set; }
        public decimal? RendaMensal { get; set; }
        public DateOnly DataCriacao { get; set; }
    }
}