using Control_Finance.Server.Enums;

namespace Control_Finance.Server.DTO
{
    public class LancamentoDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string? Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateOnly Data_Lancamento { get; set; }
        public DateOnly? Data_Vencimento { get; set; }
        public int? Fk_Id_Categoria { get; set; }
        public decimal? Valor_Pago { get; set; }
        public DateOnly? Data_Pagamento { get; set; }
        public int Fk_Id_User { get; set; }
        public TipoLancamento Tipo_Lancamento { get; set; }
        public int Fk_Id_Conta { get; set; }    
        public bool Ativo { get; set; }
    }
}