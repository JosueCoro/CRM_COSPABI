using CRM_COSPABI.DTOs;
using CRM_COSPABI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRM_COSPABI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login/admin")]
        public async Task<ActionResult<LoginResponseDto>> LoginAdmin([FromBody] LoginDto loginDto)
        {
            var resultado = await _authService.LoginAdminAsync(loginDto);
            if (resultado == null)
            {
                return Unauthorized("Credenciales inválidas o usuario inactivo.");
            }
            return Ok(resultado);
        }

        [HttpPost("login/cliente")]
        public async Task<ActionResult<LoginResponseDto>> LoginCliente([FromBody] LoginDto loginDto)
        {
            var resultado = await _authService.LoginClienteAsync(loginDto);
            if (resultado == null)
            {
                return Unauthorized("Credenciales inválidas o cuenta inactiva.");
            }
            return Ok(resultado);
        }
    }
}
