
using Microsoft.Data.Sqlite;

namespace Cadastro_de_cliente.Data
{
    internal class ConexaoBanco
    {
        private const string _connectionString = "Data Source=clientes.db";

        public static SqliteConnection ObterConexao()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}
