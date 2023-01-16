using EstoqueWeb.Infra.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstoqueWeb.Infra.Data.Interfaces
{
    /// <summary>
    /// Interface de repositorio para a entidade Evento
    /// </summary>
    public interface IProdutoRepository : IBaseRepository<Produto>
    {
        /// <summary>
        /// Metodo para retornar todos os produtos por nome ativos ou não ativos.
        /// </summary>
        /// <param name="nome">filtro de nome</param> 
        /// <param name="ativo">filtro 1 para ativo ou 2 para inativo</param> 
        /// <returns>Lista de eventos</returns>
        List<Produto> GetByDatas(string? nome, int? ativo, Guid idUsuario);
    }
}
