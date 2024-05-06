using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reservas.api.Data;
using reservas.api.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace reservas.api.Controllers
{
    [Route("api/admin/reservations")]
    [ApiController]
    [Authorize(Roles = "admin")] // Exigir autenticação e autorização para todas as ações nesta controller
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllReservasAsync(CancellationToken ct)
        {
            var reservas = await _context.Reservas.ToListAsync(ct);
            return Ok(reservas);
        }

        [HttpPut("{id}/accept")]
        public async Task<IActionResult> AcceptReservation(int id, [FromBody] ReservaModelResponse reservaResponse, CancellationToken ct)
        {
            if (reservaResponse == null)
                return BadRequest();

            var reserva = await _context.Reservas.FirstOrDefaultAsync(r => r.Id == id, ct);
            if (reserva == null)
                return NotFound();

            reserva.Status = reservaResponse.Status;
            reserva.Justificativa = reservaResponse.Justificativa;
            reserva.HoraResposta = DateTime.Now;

            await _context.SaveChangesAsync(ct);
            return Ok(reserva);
        }

        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectReservation(int id, [FromBody] ReservaModelResponse reservaResponse, CancellationToken ct)
        {
            if (reservaResponse == null)
                return BadRequest();

            var reserva = await _context.Reservas.FirstOrDefaultAsync(r => r.Id == id, ct);
            if (reserva == null)
                return NotFound();

            reserva.Status = reservaResponse.Status;
            reserva.Justificativa = reservaResponse.Justificativa;
            reserva.HoraResposta = DateTime.Now;

            await _context.SaveChangesAsync(ct);
            return Ok(reserva);
        }
    }
}
