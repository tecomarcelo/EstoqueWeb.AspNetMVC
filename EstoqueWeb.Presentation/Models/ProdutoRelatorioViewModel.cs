using System.ComponentModel.DataAnnotations;

namespace EstoqueWeb.Presentation.Models
{
    public class ProdutoRelatorioViewModel
    {
        [Required(ErrorMessage = "Por favor, informe um nome de Produto.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Por favor, informe um Status.")]
        public int? Ativo { get; set; }

        [Required(ErrorMessage = "Por favor, selecione o formato desejado.")]
        public int? Formato { get; set; }
    }
}
