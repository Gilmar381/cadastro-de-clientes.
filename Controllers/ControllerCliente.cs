using Cadastro_de_cliente.Models;
using Cadastro_de_cliente.Data;
using Microsoft.Data.Sqlite;

namespace Cadastro_de_cliente.Controllers
{
    public class ControllerCliente
    {
        public ControllerCliente()
        {
            InicializarBanco(); 
        }

        private void InicializarBanco()
        {
            using (SqliteConnection conexao = ConexaoBanco.ObterConexao())
            {
                conexao.Open();
                SqliteCommand comando = conexao.CreateCommand();
                comando.CommandText = @"
            CREATE TABLE IF NOT EXISTS Clientes (
                Id       INTEGER PRIMARY KEY AUTOINCREMENT,
                Nome     TEXT NOT NULL,
                Email    TEXT NOT NULL,
                Telefone TEXT,
                Endereco TEXT
            )";
                comando.ExecuteNonQuery();
            }
        }





        public void Adicionar(Cliente cliente)
        {
            using (SqliteConnection conexao = ConexaoBanco.ObterConexao())
            {
                conexao.Open();

                // verifica duplicidade de email
                SqliteCommand cmdVerifica = conexao.CreateCommand();
                cmdVerifica.CommandText = "SELECT COUNT(*) FROM Clientes WHERE Email = @Email";
                cmdVerifica.Parameters.AddWithValue("@Email", cliente.Email);
                long count = (long)cmdVerifica.ExecuteScalar();
                if (count > 0)
                    throw new Exception("Email já cadastrado!");

                // insere o novo cliente
                SqliteCommand cmdInsere = conexao.CreateCommand();
                cmdInsere.CommandText = @"
            INSERT INTO Clientes (Nome, Email, Telefone, Endereco)
            VALUES (@Nome, @Email, @Telefone, @Endereco)";
                cmdInsere.Parameters.AddWithValue("@Nome", cliente.Nome);
                cmdInsere.Parameters.AddWithValue("@Email", cliente.Email);
                cmdInsere.Parameters.AddWithValue("@Telefone", cliente.Telefone);
                cmdInsere.Parameters.AddWithValue("@Endereco", cliente.Endereco);
                cmdInsere.ExecuteNonQuery();
            }
        }

        public List<Cliente> Listar()
        {
            List<Cliente> clientes = new List<Cliente>();

            using (SqliteConnection conexao = ConexaoBanco.ObterConexao())
            {
                conexao.Open();
                SqliteCommand comando = conexao.CreateCommand();
                comando.CommandText = "SELECT Id, Nome, Email, Telefone, Endereco FROM Clientes";

                SqliteDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    clientes.Add(new Cliente
                    {
                        Id = reader.GetInt32(0),
                        Nome = reader.GetString(1),
                        Email = reader.GetString(2),
                        Telefone = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        Endereco = reader.IsDBNull(4) ? "" : reader.GetString(4)
                    });
                }
            }

            return clientes;
        }

        public Cliente BuscarPorId(int id)
        {
            using (SqliteConnection conexao = ConexaoBanco.ObterConexao())
            {
                conexao.Open();
                SqliteCommand comando = conexao.CreateCommand();
                comando.CommandText = "SELECT Id, Nome, Email, Telefone, Endereco FROM Clientes WHERE Id = @Id";
                comando.Parameters.AddWithValue("@Id", id);

                SqliteDataReader reader = comando.ExecuteReader();
                if (reader.Read())
                {
                    return new Cliente
                    {
                        Id = reader.GetInt32(0),
                        Nome = reader.GetString(1),
                        Email = reader.GetString(2),
                        Telefone = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        Endereco = reader.IsDBNull(4) ? "" : reader.GetString(4)
                    };
                }

                return null;
            }
        }

        public bool Atualizar(Cliente clienteAtualizado)
        {
            using (SqliteConnection conexao = ConexaoBanco.ObterConexao())
            {
                conexao.Open();

                // verifica se o cliente existe
                SqliteCommand cmdVerifica = conexao.CreateCommand();
                cmdVerifica.CommandText = "SELECT COUNT(*) FROM Clientes WHERE Id = @Id";
                cmdVerifica.Parameters.AddWithValue("@Id", clienteAtualizado.Id);
                long count = (long)cmdVerifica.ExecuteScalar();
                if (count == 0)
                    return false;

                // verifica duplicidade de email (de outro cliente)
                SqliteCommand cmdEmail = conexao.CreateCommand();
                cmdEmail.CommandText = "SELECT COUNT(*) FROM Clientes WHERE Email = @Email AND Id != @Id";
                cmdEmail.Parameters.AddWithValue("@Email", clienteAtualizado.Email);
                cmdEmail.Parameters.AddWithValue("@Id", clienteAtualizado.Id);
                long emailDuplicado = (long)cmdEmail.ExecuteScalar();
                if (emailDuplicado > 0)
                    throw new Exception("Email já cadastrado!");

                // atualiza o cliente
                SqliteCommand cmdAtualiza = conexao.CreateCommand();
                cmdAtualiza.CommandText = @"
            UPDATE Clientes 
            SET Nome = @Nome, Email = @Email, Telefone = @Telefone, Endereco = @Endereco
            WHERE Id = @Id";
                cmdAtualiza.Parameters.AddWithValue("@Nome", clienteAtualizado.Nome);
                cmdAtualiza.Parameters.AddWithValue("@Email", clienteAtualizado.Email);
                cmdAtualiza.Parameters.AddWithValue("@Telefone", clienteAtualizado.Telefone);
                cmdAtualiza.Parameters.AddWithValue("@Endereco", clienteAtualizado.Endereco);
                cmdAtualiza.Parameters.AddWithValue("@Id", clienteAtualizado.Id);
                cmdAtualiza.ExecuteNonQuery();

                return true;
            }
        }

        public bool Remover(int id)
        {
            using (SqliteConnection conexao = ConexaoBanco.ObterConexao())
            {
                conexao.Open();

                // verifica se o cliente existe
                SqliteCommand cmdVerifica = conexao.CreateCommand();
                cmdVerifica.CommandText = "SELECT COUNT(*) FROM Clientes WHERE Id = @Id";
                cmdVerifica.Parameters.AddWithValue("@Id", id);
                long count = (long)cmdVerifica.ExecuteScalar();
                if (count == 0)
                    return false;

                // remove o cliente
                SqliteCommand cmdRemove = conexao.CreateCommand();
                cmdRemove.CommandText = "DELETE FROM Clientes WHERE Id = @Id";
                cmdRemove.Parameters.AddWithValue("@Id", id);
                cmdRemove.ExecuteNonQuery();

                return true;
            }
        }
    }
}