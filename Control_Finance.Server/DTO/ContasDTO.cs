namespace Control_Finance.Server.DTO
{
    public class ContasDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public int Tipo_Conta { get; set; } 
        public int Fk_Id_User { get; set; }
        public bool Ativo { get; set; }
    }
}