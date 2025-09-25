using System;

namespace BancoSrbApi.Application.DTOs
{
    public class SaldoResponseDto
    {
        public int NumeroConta { get; set; }
        public string NomeTitular { get; set; }
        public DateTime DataConsulta { get; set; }
        public decimal Saldo { get; set; }
    }
}
