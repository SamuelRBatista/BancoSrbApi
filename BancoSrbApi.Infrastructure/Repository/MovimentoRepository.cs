using BancoSrbApi.BancoSrbApi.Domain.Entities;
using BancoSrbApi.BancoSrbApi.Domain.Interface;
using BancoSrbApi.BancoSrbApi.Infrastructure.Config;
using Dapper;
using System.Collections.Generic;

namespace BancoSrbApi.BancoSrbApi.Infrastructure.Repository
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly DatabaseConnection _db;

        public MovimentoRepository(DatabaseConnection db) => _db = db;

        public void Inserir(Movimento movimento)
        {
            using var conn = _db.CreateConnection();
            conn.Execute(@"
                INSERT INTO movimento(idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
                VALUES(@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)",
                new
                {
                    IdMovimento = movimento.IdMovimento,
                    IdContaCorrente = movimento.IdContaCorrente,
                    DataMovimento = movimento.DataMovimento,
                    TipoMovimento = movimento.TipoMovimento,
                    Valor = movimento.Valor
                });
        }

        public IEnumerable<Movimento> ListarPorConta(string idConta)
        {
            using var conn = _db.CreateConnection();
            return conn.Query<Movimento>(
                "SELECT * FROM movimento WHERE idcontacorrente = @Id",
                new { Id = idConta });
        }
    }
}
