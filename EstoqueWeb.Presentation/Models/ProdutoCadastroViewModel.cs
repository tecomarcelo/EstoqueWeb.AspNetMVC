using System.ComponentModel.DataAnnotations;

namespace EstoqueWeb.Presentation.Models
{
    /// <summary>
    /// Pagina de modelo de dados para a pagina de cadastro de produto
    /// </summary>
    public class ProdutoCadastroViewModel
    {
        [MinLength(6, ErrorMessage = "Por favor, informe no mínimo {1} caracteres.")]
        [MaxLength(150, ErrorMessage = "Por favor, informe no máximo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe o nome do produto.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Por favor, informe o preço do produto.")]
        public string? Preco { get; set; }

        [Required(ErrorMessage = "Por favor, informe a quantidade do produto.")]
        public string? Quantidade { get; set; }

        [Required(ErrorMessage = "Por favor, informe o status do produto.")]
        public int? Ativo { get; set; }

        [MaxLength(500, ErrorMessage = "Por favor, informe no máximo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe a descrição do produto.")]
        public string? Descricao { get; set; }
    }
}
