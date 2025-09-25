using BancoSrbApi.Application.DTOs;
using BancoSrbApi.BancoSrbApi.Application.Dtos;
using BancoSrbApi.BancoSrbApi.Domain.Entities;
using BancoSrbApi.BancoSrbApi.Domain.Exceptions;
using BancoSrbApi.BancoSrbApi.Domain.Interface;
using System;
using System.Linq;
using System.Text.Json;

namespace BancoSrbApi.Application.Services
{
    public class MovimentoService
    {
        private readonly IContaCorrenteRepository _contaRepo;
        private readonly IMovimentoRepository _movimentoRepo;
        private readonly IIdempotenciaRepository _idempotenciaRepo;

        public MovimentoService(
            IContaCorrenteRepository contaRepo,
            IMovimentoRepository movimentoRepo,
            IIdempotenciaRepository idempotenciaRepo)
        {
            _contaRepo = contaRepo;
            _movimentoRepo = movimentoRepo;
            _idempotenciaRepo = idempotenciaRepo;
        }

        public MovimentoResponseDto Movimentar(MovimentoRequestDto dto)
        {
            
            var existente = _idempotenciaRepo.ObterPorChave(dto.ChaveIdempotencia);
            if (existente != null)
                return JsonSerializer.Deserialize<MovimentoResponseDto>(existente.Resultado);

         
            var conta = _contaRepo.ObterPorId(dto.IdContaCorrente);
            if (conta == null) throw new BusinessException("Conta inválida", "INVALID_ACCOUNT");
            if (!conta.Ativo) throw new BusinessException("Conta inativa", "INACTIVE_ACCOUNT");
            if (dto.Valor <= 0) throw new BusinessException("Valor inválido", "INVALID_VALUE");
            if (dto.TipoMovimento != "C" && dto.TipoMovimento != "D") throw new BusinessException("Tipo inválido", "INVALID_TYPE");

           
            var movimentos = _movimentoRepo.ListarPorConta(dto.IdContaCorrente);
            var saldo = movimentos.Where(m => m.TipoMovimento == "C").Sum(m => m.Valor)
                      - movimentos.Where(m => m.TipoMovimento == "D").Sum(m => m.Valor);

          
            if (dto.TipoMovimento == "D" && saldo < dto.Valor)
                throw new BusinessException("Saldo insuficiente", "INSUFFICIENT_FUNDS");

            var movimento = new Movimento
            {
                IdMovimento = Guid.NewGuid().ToString(),
                IdContaCorrente = dto.IdContaCorrente,
                Valor = dto.Valor,
                TipoMovimento = dto.TipoMovimento,
                DataMovimento = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            _movimentoRepo.Inserir(movimento);

            
            var resultado = JsonSerializer.Serialize(new MovimentoResponseDto { IdMovimento = movimento.IdMovimento });
            _idempotenciaRepo.Inserir(new Idempotencia
            {
                ChaveIdempotencia = dto.ChaveIdempotencia,
                Requisicao = JsonSerializer.Serialize(dto),
                Resultado = resultado
            });

            return new MovimentoResponseDto { IdMovimento = movimento.IdMovimento };
        }

    }
}
