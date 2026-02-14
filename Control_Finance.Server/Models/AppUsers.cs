using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Control_Finance.Server.Models
{
    public class AppUsers : IdentityUser<int>
    {
        [Required]
        [StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres.")]
        public required string Nome { get; set; }
        [Required]
        [Description("Data de Nascimento")]
        public required DateOnly DataNascimento { get; set; }
        [Description("Renda Mensal aproximada")]
        [DefaultValue(0)]
        public decimal? RendaMensal { get; set; }
        [Required]
        [Description("Data de Criação do Usuário")]
        public required DateTime DataCriacao = DateTime.Now;
        [Required]
        [DefaultValue(true)]
        public bool Ativo { get; set; }

    }
}
