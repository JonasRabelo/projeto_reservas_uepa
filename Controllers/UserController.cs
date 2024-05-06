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
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(CancellationToken ct)
        {
            try
            {
                var users = await _userService.GetAllUsersAsync(ct);

                if (users == null) return NoContent();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }


        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id, ct);
                if (user == null)
                    return NotFound();

                return Ok(user);
            }catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [Authorize(Roles = "admin")]
        [HttpGet("search")]
        public async Task<IActionResult> GetByNameOrMatricula(string query, CancellationToken ct)
        {
            try
            {
                var users = await _userService.GetByNameOrMatriculaAsync(query, ct);

                if (!users.Any())
                    return NotFound();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }  
        }



        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] UserCreateModel userCreate, CancellationToken ct)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Verificar se já existe um usuário com o mesmo e-mail
                if (await _userService.SeachUserByEmailAsync(userCreate.Email, ct))
                    return Conflict("E-mail já está em uso.");

                var user = _userService.CreateUserAsync(userCreate, ct);

                if (user == null) return BadRequest("Falha ao criar o usuário, tente novamente.");

                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [Authorize(Roles = "admin,usuario")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UserUpdateModel userUpdate, CancellationToken ct)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _userService.GetByIdAsync(id, ct);
                if (user == null)
                    return NotFound("Erro ao buscar o usuário na base de dados.");

                // Verificar se o e-mail está sendo alterado para um que já está em uso
                if (user.Email != userUpdate.Email)
                    if (await _userService.SeachUserByEmailAsync(userUpdate.Email, ct))
                        return Conflict("E-mail já está em uso.");

                user = await _userService.UpdateUserAsync(user, userUpdate, ct);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken ct)
        {
            try
            {
                if (await _userService.DeleteUserAsync(id, ct))
                    return NotFound("Erro ao buscar o usuário na base de dados.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
