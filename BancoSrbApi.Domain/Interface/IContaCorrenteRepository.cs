using BancoSrbApi.Models;

namespace BancoSrbApi.BancoSrbApi.Domain.Interface
{
    public interface IContaCorrenteRepository
    {
        ContaCorrente ObterPorId(string id);
        ContaCorrente ObterPorNumero(int numero);
    }
}
