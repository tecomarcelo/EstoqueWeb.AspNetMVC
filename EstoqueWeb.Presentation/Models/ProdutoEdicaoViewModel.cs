using System.ComponentModel.DataAnnotations;

namespace EstoqueWeb.Presentation.Models
{
    public class ProdutoEdicaoViewModel
    {
        //campo oculto
        public Guid Id { get; set; }
        
        [MaxLength(150, ErrorMessage = "Por favor, informe no máximo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe o nome do produto.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Por favor, informe o preço do produto.")]
        public decimal? Preco { get; set; }

        [Required(ErrorMessage = "Por favor, informe a quantidade do produto.")]
        public int? Quantidade { get; set; }

        [Required(ErrorMessage = "Por favor, informe o status do produto.")]
        public int? Ativo { get; set; }

        [MaxLength(500, ErrorMessage = "Por favor, informe no máximo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe a descrição do produto.")]
        public string? Descricao { get; set; }

        public string? DataAlteracao { get; set; }

        public string? DataInclusao { get; set; }
    }
}
