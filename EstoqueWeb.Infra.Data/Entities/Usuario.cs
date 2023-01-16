using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstoqueWeb.Infra.Data.Entities
{
    /// <summary>
    ///Classe para entidade Usuario 
    /// </summary>
    public class Usuario
    {
        #region Proprioedades

        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public DateTime DataInclusao { get; set; }

        #endregion

        #region Relacionamentos

        public List<Produto>? Produtos { get; set; }

        #endregion
    }
}
