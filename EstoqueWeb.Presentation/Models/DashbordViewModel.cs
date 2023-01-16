using EstoqueWeb.Infra.Data.Entities;

namespace EstoqueWeb.Presentation.Models
{
    public class DashbordViewModel
    {
        public string? Nome { get; set; }
        public int TotalGeral { get; set; }
        public int TotalAtivos { get; set; }
        public int TotalInativos { get; set; }

        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }

        // Lista de produtos que sera utilizado para exibir na pag. de resultados da consulta
        public List<Produto>? Produtos { get; set; }
    }
}
