using EstoqueWeb.Infra.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstoqueWeb.Reports.Interfaces
{
    public interface IProdutoReportService
    {
        //Metodo para fazer a geração de um relatório
        byte[] Create(string? nome, int? ativo, List<Produto> produtos);
    }
}
