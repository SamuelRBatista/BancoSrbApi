using BancoSrbApi.BancoSrbApi.Domain.Entities;
using BancoSrbApi.BancoSrbApi.Domain.Interface;
using Dapper;
using System;

namespace BancoSrbApi.BancoSrbApi.Infrastructure.Repository
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly BancoSrbApi.Infrastructure.Config.DatabaseConnection _db;
        public IdempotenciaRepository(BancoSrbApi.Infrastructure.Config.DatabaseConnection db) => _db = db;

        public Idempotencia ObterPorChave(string chave)
        {
            using var conn = _db.CreateConnection();
            return conn.QueryFirstOrDefault<Idempotencia>(
                "SELECT * FROM idempotencia WHERE chave_idempotencia = @Chave", new { Chave = chave });
        }

        public void Inserir(Idempotencia entidade)
        {
            using var conn = _db.CreateConnection();
            conn.Execute(@"INSERT INTO idempotencia(chave_idempotencia, requisicao, resultado)
                           VALUES(@ChaveIdempotencia, @Requisicao, @Resultado)", entidade);
        }
    }
}
