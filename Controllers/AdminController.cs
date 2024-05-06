using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using reservas.api.Models;
using reservas.api.Services.IService;

namespace reservas.api.Controllers
{
    [Route("api/admin/reservations")]
    [ApiController]
    [Authorize(Roles = "admin")] // Exigir autenticação e autorização para todas as ações nesta controller
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllReservasAsync(CancellationToken ct)
        {
            var reservas = await _adminService.GetAllReservasAsync(ct);

            if (reservas == null) return NoContent(); 

            return Ok(reservas);
        }


        [HttpPut("{id}/accept")]
        public async Task<IActionResult> AcceptReservation([FromBody] ReservaModelResponse reservaResponse, int id, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reserva = await _adminService.AceptReservaAsync(reservaResponse, id, ct);

            if (reserva  == null) return BadRequest("Falha ao localizar a reserva no Banco de dados.");

            return Ok(reserva);
        }


        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectReservation(int id, [FromBody] ReservaModelResponse reservaResponse, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reserva = await _adminService.RejectReservaAsync(reservaResponse, id, ct);

            if (reserva == null) return BadRequest("Falha ao localizar a reserva no Banco de dados.");

            return Ok(reserva);
        }
    }
}
