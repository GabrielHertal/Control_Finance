using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Control_Finance.Server.Models
{
    public class Categorias
    {
        [Key]
        public required int Id { get; set; }
        public required string Titulo { get; set; }
        [ForeignKey("AppUsers")]
        [Required]
        public required int FkIdUser { get; set; }
        public AppUsers? AppUsers { get; set; }
        [DefaultValue(true)]
        public required bool Ativo { get; set; } = true;
    }
}