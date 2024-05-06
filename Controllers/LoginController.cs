using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reservas.api.Data;
using reservas.api.Models;
using reservas.api.Services;
using reservas.api.Services.IService;

namespace reservas.api.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;


        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel login, CancellationToken ct)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _loginService.GetUsersByEmailAsync(login.Email, ct);

                if (user == null) return NotFound("E-mail não encontrado.");

                var token = _loginService.LoginAsync(login, user, ct);

                if (token.Equals("")) return Unauthorized();

                return Ok( new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }
    }
}
