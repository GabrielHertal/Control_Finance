namespace Control_Finance.Server.DTO
{
    public class UsersDTO
    {
        public int? Id { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; } 
        public required DateOnly DataNascimento { get; set; }
        public required DateTime DataCriacao { get; set; }
        public decimal? RendaMensal { get; set; } = 0;
        public string? Password { get; set; }
        public bool Ativo { get; set; }
    }
}