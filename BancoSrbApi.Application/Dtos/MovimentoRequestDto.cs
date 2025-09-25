namespace BancoSrbApi.BancoSrbApi.Application.Dtos
{
    public class MovimentoRequestDto
    {
        public string ChaveIdempotencia { get; set; }
        public string IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public string TipoMovimento { get; set; } // "C" ou "D"
    }
}
