using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reservas.api.Data;
using reservas.api.Enums;
using reservas.api.Models;
using reservas.api.Services.IService;

namespace reservas.api.Controllers
{
    [Route("api/reservas")]
    [ApiController]
    [Authorize(Roles = "admin,usuario")]
    public class ReservaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IReservaService _reservaService;


        public ReservaController(AppDbContext context, IReservaService reservaService)
        {
            _context = context;
            _reservaService = reservaService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllReservas(CancellationToken ct)
        {
            return Ok(_reservaService.GetAllReservaAsync(ct));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservaById(int id, CancellationToken ct)
        {
            var reserva = _reservaService.GetReservaByIdAsync(id, ct);

            if (reserva == null)
                return NotFound();

            return Ok(reserva);
        }


        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetReservasByUserId(int userId, CancellationToken ct)
        {
            var reservas = _reservaService.GetReservasByUserId(userId, ct);
            
            if (reservas == null)
                return NotFound();

            return Ok(reservas);
        }



        [HttpGet("date")]
        public async Task<IActionResult> GetReservasByDateRange(DateTime start, DateTime end, CancellationToken ct)
        {
            try
            {
                var reservas = _reservaService.GetReservasByDateRange(start, end, ct);

                if (reservas == null)
                    return NotFound();

                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateReserva([FromBody] ReservaModelRequest reservaRequest, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ReservaModel reserva = CreateReservaModel(reservaRequest);

            var validationResult = await _reservaService.ValidateReservaAsync(reserva, ct);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ErrorMessage);
            }

            await _reservaService.CreateReservaAsync(reserva, ct);

            return CreatedAtAction(nameof(GetReservaById), new { id = reserva.Id, ct}, reserva);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReserva(int id, [FromBody] ReservaModelRequest reservaRequest, CancellationToken ct)
        {
            if (reservaRequest == null)
                return BadRequest();

            var reserva = await _reservaService.UpdateReservaAsync(id, reservaRequest, ct);

            return Ok(reserva);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservaAsync(int id, CancellationToken ct)
        {
            if (await _reservaService.DeleteReservaAsync(id, ct)) return NotFound();

            return NoContent();
        }


        private ReservaModel CreateReservaModel(ReservaModelRequest reservaRequest)
        {
            return new ReservaModel
            {
                Id = 0,
                Status = Status.AguardandoResposta,
                Reserva = reservaRequest.Reserva,
                HoraSolicitacao = DateTime.Now,
                DataInicioReserva = reservaRequest.DataInicioReserva,
                DataFimReserva = reservaRequest.DataFimReserva,
                HoraInicio = reservaRequest.HoraInicio,
                QuantidadeHorasReserva = reservaRequest.QuantidadeHorasReserva,
                IdUser = reservaRequest.IdUser,
                Descricao = reservaRequest.Descricao

    };
        }
    }
}
