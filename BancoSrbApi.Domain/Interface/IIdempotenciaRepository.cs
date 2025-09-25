using BancoSrbApi.BancoSrbApi.Domain.Entities;

namespace BancoSrbApi.BancoSrbApi.Domain.Interface
{
    public interface IIdempotenciaRepository
    {
        Idempotencia ObterPorChave(string chave);
        void Inserir(Idempotencia entidade);
    }
}
