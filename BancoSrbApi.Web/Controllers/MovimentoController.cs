using BancoSrbApi.Application.DTOs;
using BancoSrbApi.Application.Services;
using BancoSrbApi.BancoSrbApi.Application.Dtos;
using BancoSrbApi.BancoSrbApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BancoSrbApi.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentoController : ControllerBase
    {
        private readonly MovimentoService _service;
        public MovimentoController(MovimentoService service) => _service = service;

        [HttpPost]
        public IActionResult Post([FromBody] MovimentoRequestDto dto)
        {
            try
            {
                var resultado = _service.Movimentar(dto);
                return Ok(resultado);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message, type = ex.Tipo });
            }
        }
    }
}
