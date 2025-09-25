using BancoSrbApi.Application.DTOs;
using BancoSrbApi.BancoSrbApi.Domain.Exceptions;
using BancoSrbApi.BancoSrbApi.Domain.Interface;
using System;
using System.Linq;

namespace BancoSrbApi.Application.Services
{
    public class ContaCorrenteService
    {
        private readonly IContaCorrenteRepository _contaRepo;
        private readonly IMovimentoRepository _movimentoRepo;

        public ContaCorrenteService(IContaCorrenteRepository contaRepo, IMovimentoRepository movimentoRepo)
        {
            _contaRepo = contaRepo;
            _movimentoRepo = movimentoRepo;
        }

        public SaldoResponseDto ConsultarSaldo(string idConta)
        {
            var conta = _contaRepo.ObterPorId(idConta.ToString());
            if (conta == null) throw new BusinessException("Conta inválida", "INVALID_ACCOUNT");
            if (!conta.Ativo) throw new BusinessException("Conta inativa", "INACTIVE_ACCOUNT");

            var movimentos = _movimentoRepo.ListarPorConta(idConta);
            var saldo = movimentos.Where(m => m.TipoMovimento == "C").Sum(m => m.Valor)
                      - movimentos.Where(m => m.TipoMovimento == "D").Sum(m => m.Valor);

            return new SaldoResponseDto
            {
                NumeroConta = conta.Numero,
                NomeTitular = conta.Nome,
                DataConsulta = DateTime.Now,
                Saldo = saldo
            };
        }
    }
}
