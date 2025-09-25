using BancoSrbApi.Application.Services;
using BancoSrbApi.BancoSrbApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BancoSrbApi.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly ContaCorrenteService _service;
        public ContaCorrenteController(ContaCorrenteService service) => _service = service;

        [HttpGet("{id}")]
        public IActionResult GetSaldo(string id)
        {
            try
            {
                var saldo = _service.ConsultarSaldo(id);
                return Ok(saldo);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message, type = ex.Tipo });
            }
        }
    }
}
