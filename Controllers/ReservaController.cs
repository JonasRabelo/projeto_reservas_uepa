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
        private readonly IReservaService _reservaService;

        public ReservaController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }


        [HttpGet]
        public IActionResult GetAllReservas(CancellationToken ct)
        {
            try
            {
                return Ok(_reservaService.GetAllReservaAsync(ct));
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }    
        }


        [HttpGet("{id}")]
        public IActionResult GetReservaById(int id, CancellationToken ct)
        {
            try
            {
                var reserva = _reservaService.GetReservaByIdAsync(id, ct);

                if (reserva == null)
                    return NotFound();

                return Ok(reserva);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }         
        }


        [HttpGet("user/{userId}")]
        public IActionResult GetReservasByUserId(int userId, CancellationToken ct)
        {
            try
            {
                var reservas = _reservaService.GetReservasByUserId(userId, ct);

                if (reservas == null)
                    return NotFound();

                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }   



        [HttpGet("date")]
        public IActionResult GetReservasByDateRange(DateTime start, DateTime end, CancellationToken ct)
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
            try
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

                return CreatedAtAction(nameof(GetReservaById), new { id = reserva.Id, ct }, reserva);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }       
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReserva(int id, [FromBody] ReservaModelRequest reservaRequest, CancellationToken ct)
        {
            try
            {
                if (reservaRequest == null)
                    return BadRequest();

                var reserva = await _reservaService.UpdateReservaAsync(id, reservaRequest, ct);

                return Ok(reserva);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }          
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservaAsync(int id, CancellationToken ct)
        {
            try
            {
                if (await _reservaService.DeleteReservaAsync(id, ct)) return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }          
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
