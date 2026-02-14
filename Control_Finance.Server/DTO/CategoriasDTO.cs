namespace Control_Finance.Server.DTO
{
    public class CategoriasDTO
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required int FkIdUser { get; set; }
        public bool Ativo { get; set; } = true;
    }
}