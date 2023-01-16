using System.ComponentModel.DataAnnotations;
using EstoqueWeb.Infra.Data.Entities;

namespace EstoqueWeb.Presentation.Models
{
    public class ProdutoConsultaViewModel
    {
        [Required(ErrorMessage = "Por favor, informe um nome de Produto.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Por favor, informe um Status.")]
        public int? Ativo { get; set; }

        // Lista de produtos que sera utilizado para exibir na pag. de resultados da consulta
        public List<Produto>? Produtos { get; set; }
    }
}
