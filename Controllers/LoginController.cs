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
        private readonly AppDbContext _context;
        private readonly IPasswordHashService _passwordHashService;
        private readonly ITokenService _tokenService;


        public LoginController(AppDbContext context, 
                               IPasswordHashService passwordHashService,
                               ITokenService tokenService)
        {
            _context = context;
            _passwordHashService = passwordHashService;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel login, CancellationToken ct)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == login.Email, ct);

            if (user == null)
                return NotFound();

            if (!_passwordHashService.VerifyPassword(login.Password, user.Password))
                return Unauthorized();

            var token = _tokenService.Generate(user);
            return Ok(new { Token = token });
        }
    }
}
