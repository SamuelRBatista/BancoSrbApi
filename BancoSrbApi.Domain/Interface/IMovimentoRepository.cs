using BancoSrbApi.BancoSrbApi.Domain.Entities;
using System.Collections.Generic;

namespace BancoSrbApi.BancoSrbApi.Domain.Interface
{
    public interface IMovimentoRepository
    {
        void Inserir(Movimento movimento);
        IEnumerable<Movimento> ListarPorConta(string idConta);
    }
}
