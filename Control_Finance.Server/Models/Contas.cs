using Control_Finance.Server.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Control_Finance.Server.Models
{
    public class Contas
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public required string Titulo { get; set; }
        public TipoConta Tipo_Conta { get; set; }
        [Required]
        [ForeignKey("AppUsers")]
        public required int Fk_Id_User { get; set; }
        public AppUsers? AppUsers { get; set; }
        [Required]
        [DefaultValue(true)]
        public required bool Ativo { get; set; } = true;
    }
}