using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reservas.api.Data;
using reservas.api.Models;
using reservas.api.Services.IService;

namespace reservas.api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHashService _passwordHashService;

        public UserController(AppDbContext context, IPasswordHashService passwordHashService)
        {
            _context = context;
            _passwordHashService = passwordHashService;
        }


        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(CancellationToken ct)
        {
            var users = await _context.Users.ToListAsync(ct);
            return Ok(users);
        }


        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var user = await _context.Users.FindAsync(id, ct);
            if (user == null)
                return NotFound();

            return Ok(user);
        }


        [Authorize(Roles = "admin")]
        [HttpGet("search")]
        public async Task<IActionResult> GetByNameOrMatricula(string query, CancellationToken ct)
        {
            var users = await _context.Users
            .Where(u => u.Matricula.Contains(query) || u.Name.Contains(query))
                .ToListAsync(ct);

            if (!users.Any())
                return NotFound();

            return Ok(users);
        }



        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] UserCreateModel userCreate, CancellationToken ct)
        {
            // Validar modelo
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verificar se já existe um usuário com o mesmo e-mail
            if (await _context.Users.AnyAsync(u => u.Email == userCreate.Email, ct))
                return Conflict("E-mail já está em uso.");

            var user = CreateUser(userCreate);

            user.Password = _passwordHashService.GeneratePasswordHash(userCreate.Password);
            await _context.Users.AddAsync(user, ct);
            await _context.SaveChangesAsync(ct);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }


        [Authorize(Roles = "admin,usuario")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UserUpdateModel userUpdate, CancellationToken ct)
        {
            // Validar modelo
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            // Verificar se o e-mail está sendo alterado para um que já está em uso
            if (user.Email != userUpdate.Email && await _context.Users.AnyAsync(u => u.Email == userUpdate.Email, ct))
                return Conflict("E-mail já está em uso.");

            user.Name = userUpdate.Name;
            user.Email = userUpdate.Email;
            user.Matricula = userUpdate.Matricula;
            user.Perfil = userUpdate.Perfil;
            user.DataNascimento = userUpdate.DataNascimento;
            user.DataAtualizacao = DateTime.Now;

            await _context.SaveChangesAsync(ct);
            return Ok(user);
        }


        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken ct)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(ct);

            return Ok(user);
        }

        private UserModel CreateUser(UserCreateModel userCreate)
        {
            return new UserModel
            {
                Name = userCreate.Name,
                Email = userCreate.Email,
                Matricula = userCreate.Matricula,
                Perfil = userCreate.Perfil,
                DataNascimento = userCreate.DataNascimento,
                DataCadastro = DateTime.Now
            };
        }
    }
}
