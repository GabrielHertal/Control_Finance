namespace Control_Finance.Server.DTO
{
    public class RegisterUserDTO
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public DateOnly DataNascimento { get; set; }
        public decimal? RendaMensal { get; set; }
    }
}
