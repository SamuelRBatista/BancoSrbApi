using BancoSrbApi.BancoSrbApi.Domain.Interface;
using BancoSrbApi.Models;
using Dapper;
using System;

namespace BancoSrbApi.BancoSrbApi.Infrastructure.Repository
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly BancoSrbApi.Infrastructure.Config.DatabaseConnection _db;
        public ContaCorrenteRepository(BancoSrbApi.Infrastructure.Config.DatabaseConnection db) => _db = db;

        public ContaCorrente ObterPorId(string id)
        {
            using var conn = _db.CreateConnection();
            return conn.QueryFirstOrDefault<ContaCorrente>(
                "SELECT * FROM contacorrente WHERE idcontacorrente = @Id", new { Id = id });
        }

        public ContaCorrente ObterPorNumero(int numero)
        {
            using var conn = _db.CreateConnection();
            return conn.QueryFirstOrDefault<ContaCorrente>(
                "SELECT * FROM contacorrente WHERE numero = @Numero", new { Numero = numero });
        }
    }
}
