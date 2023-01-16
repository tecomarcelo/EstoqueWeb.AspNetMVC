using EstoqueWeb.Infra.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstoqueWeb.Infra.Data.Interfaces
{
    /// <summary>
    /// Interface de repositorio para a entidade Usuario
    /// </summary>
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        /// <summary>
        /// Metodo para retornar os dados de 1 Usuario baseado no email
        /// </summary>
        /// <param name="email">Email do usuario</param>
        /// <returns>Objeto usuario ou null se não for encontrado</returns>
        Usuario GetByEmail(string email);


        /// <summary>
        /// Metodo para retornar os dados de 1 Usuario baseado no email e senha
        /// </summary>
        /// <param name="email">Email do usuario</param>
        /// <param name="senha">Senha do usuario</param>
        /// <returns>Objeto usuario ou null se não for encontrado</returns>
        Usuario GetByEmailESenha(string email, string senha);
    }
}
