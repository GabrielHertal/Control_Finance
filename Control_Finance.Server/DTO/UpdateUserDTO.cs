namespace Control_Finance.Server.DTO
{
    public class UpdateUserDTO
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required DateOnly DataNascimento { get; set; }
        public decimal? RendaMensal { get; set; } = 0;
        public string? Password { get; set; }
    }
}
