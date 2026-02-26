using Control_Finance.Server.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Control_Finance.Server.Models
{
    public class Lancamentos
    {
        [Key]
        public required int Id { get; set; }
        [Required]
        [StringLength(50)]
        public required string Titulo { get; set; }
        [MaxLength(100)]
        public string? Descricao { get; set; }
        [Required]
        public required decimal Valor { get; set; }
        [Required]
        public required DateOnly Data_Lancamento { get; set; }
        public DateOnly? Data_Vencimento { get; set; }
        [ForeignKey("Categorias")]
        public int? Fk_Id_Categoria { get; set; }
        public Categorias? Categorias { get; set; }
        public decimal? Valor_Pago { get; set; } = null;
        public DateOnly? Data_Pagamento { get; set; } = null;
        [Required]
        [ForeignKey("AppUsers")]
        public required int Fk_Id_User { get; set; }
        public AppUsers? AppUsers { get; set; }
        [Required]
        [Comment("1 - Despesa, 2 - Receita, 3 - Investimento")]
        public TipoLancamento Tipo_Lancamento { get; set; }
        [ForeignKey("Contas")]
        public  int Fk_Id_Conta { get; set; }
        public Contas? Contas { get; set; } = null;
        [Required]
        [DefaultValue(true)]
        public required bool Ativo { get; set; } = true;
    }
}