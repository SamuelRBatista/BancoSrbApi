using System.Data;
using Microsoft.Data.Sqlite;

namespace BancoSrbApi.BancoSrbApi.Infrastructure.Config
{
    public class DatabaseConnection
    {
        private readonly string _connectionString;
        public DatabaseConnection(string connectionString) => _connectionString = connectionString;

        public IDbConnection CreateConnection()
        {
            var conn = new SqliteConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}
