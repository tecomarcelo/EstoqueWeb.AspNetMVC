using Dapper;
using EstoqueWeb.Infra.Data.Entities;
using EstoqueWeb.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstoqueWeb.Infra.Data.Repositories
{
    /// <summary>
    /// Classe para implementar as operações de banco de dados do Produto
    /// </summary>
    public class ProdutoRepository : IProdutoRepository
    {

        //atributo
        private readonly string _connectionString;

        //metodo construtor para passar a connectionstring para a classe de repositorio.
        public ProdutoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(Produto obj)
        {
            var query = @"
                INSERT INTO PRODUTOS(ID, NOME, PRECO, QUANTIDADE, DESCRICAO, ATIVO, DATAINCLUSAO, DATAALTERACAO, IDUSUARIO)
                VALUES(@Id, @Nome, @Preco, @Quantidade, @Descricao, @Ativo, @DataInclusao, @DataAlteracao, @IdUsuario)
            ";

            //conectando com o banco de dados
            using (var connection = new SqlConnection(_connectionString))
            {
                //executando a gravação do evento na base de dados
                connection.Execute(query, obj);
            }
        }

        public void Update(Produto obj)
        {
            var query = @"
                UPDATE PRODUTOS 
                SET 
                    NOME = @Nome,
                    PRECO = @Preco,
                    QUANTIDADE = @Quantidade,
                    DESCRICAO = @Descricao,
                    ATIVO = @Ativo,
                    DATAALTERACAO = @DataAlteracao
                WHERE
                    ID = @Id
            ";

            //conectando com o banco de dados
            using (var connection = new SqlConnection(_connectionString))
            {
                //executando a gravação do evento na base de dados
                connection.Execute(query, obj);
            }
        }

        public void Delete(Produto obj)
        {
            var query = @"
                DELETE FROM PRODUTOS 
                WHERE
                    ID = @Id
            ";

            //conectando com o banco de dados
            using (var connection = new SqlConnection(_connectionString))
            {
                //executando a gravação do evento na base de dados
                connection.Execute(query, obj);
            }
        }

        public List<Produto> GetAll()
        {
            var query = @"
                SELECT * FROM PRODUTOS 
                ORDER BY NOME ASC
            ";

            //conectando com o banco de dados
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .Query<Produto>(query)
                    .ToList();
            }
        }

        public List<Produto> GetByDatas(string? nome, int? ativo, Guid idUsuario)
        {
            var query = @"
                SELECT * FROM PRODUTOS 
                WHERE NOME LIKE @PARAM AND ATIVO = @Ativo AND IDUSUARIO = @IdUsuario
                ORDER BY NOME ASC
            ";

            //conectando com o banco de dados
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .Query<Produto>(query, new { @PARAM = $"%{nome}%", ativo, idUsuario })
                    .ToList();
            }
        }

        public Produto GetById(Guid id)
        {
            var query = @"
                SELECT * FROM PRODUTOS 
                WHERE ID = @id
            ";

            //conectando com o banco de dados
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .Query<Produto>(query, new { id })
                    .FirstOrDefault();
            }
        }
    }
}
