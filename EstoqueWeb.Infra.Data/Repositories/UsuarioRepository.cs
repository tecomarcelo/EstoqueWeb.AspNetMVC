﻿using EstoqueWeb.Infra.Data.Entities;
using EstoqueWeb.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;

namespace EstoqueWeb.Infra.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        //atributo
        private readonly string _connectionString;

        //metodo construtor
        public UsuarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        

        public void Create(Usuario obj)
        {
            var query = @"
                    INSERT INTO USUARIO(ID, NOME, EMAIL, SENHA, DATAINCLUSAO)
                    VALUES(@Id, @Nome, @Email, @Senha, @DataInclusao)
                ";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, obj);
            }
        }

        public void Update(Usuario obj)
        {
            var query = @"
                    UPDATE USUARIO SET 
                        NOME = @Nome,
                        EMAIL = @Email,
                        SENHA = @Senha
                    WHERE ID = @Id
                ";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, obj);
            }
        }

        public void Delete(Usuario obj)
        {
            var query = @"
                    DELETE FROM USUARIO
                    WHERE ID = @Id
                ";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, obj);
            }
        }

        public List<Usuario> GetAll()
        {
            var query = @"SELECT * FROM USUARIO";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .Query<Usuario>(query)
                    .ToList();
            }
        }

        public Usuario GetById(Guid id)
        {
            var query = @"SELECT * FROM USUARIO 
                    WHERE ID = @Id";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .Query<Usuario>(query, new {id})
                    .FirstOrDefault();
            }
        }

        public Usuario GetByEmail(string email)
        {
            var query = @"SELECT * FROM USUARIO 
                    WHERE EMAIL = @Email";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .Query<Usuario>(query, new { email })
                    .FirstOrDefault();
            }
        }

        public Usuario GetByEmailESenha(string email, string senha)
        {
            var query = @"SELECT * FROM USUARIO 
                    WHERE EMAIL = @Email AND SENHA = @Senha";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .Query<Usuario>(query, new { email, senha })
                    .FirstOrDefault();
            }
        }
    }
}
