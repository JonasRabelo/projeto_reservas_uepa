using Microsoft.EntityFrameworkCore;
using reservas.api.Data;
using reservas.api.Models;
using reservas.api.Services.IService;
using System.ComponentModel.DataAnnotations;

namespace reservas.api.Services
{
    public class ReservaService : IReservaService
    {
        private readonly AppDbContext _context;

        public ReservaService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<ResultValidation> ValidateReservaAsync(ReservaModel reserva, CancellationToken ct)
        {
            // Verifica se existe alguma reserva conflitante na base de dados
            List<ReservaModel> conflictingReservas = await _context.Reservas.Where(r => r.Id != reserva.Id && // Exclui a própria reserva da verificação
                    r.Reserva == reserva.Reserva && 
                    r.DataInicioReserva < reserva.DataFimReserva && r.DataFimReserva > reserva.DataInicioReserva).ToListAsync(ct);

            if (conflictingReservas.Any())
            {
                return new ResultValidation
                {
                    IsValid = false,
                    ErrorMessage = "A reserva entra em conflito com outras reservas existentes."
                };
            }

            return new ResultValidation { IsValid = true };
        }


        public async Task<List<ReservaModel>> GetAllReservaAsync(CancellationToken ct)
        {
            return await _context.Reservas.ToListAsync(ct);
        }

        public async Task<ReservaModel> GetReservaByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Reservas.FirstOrDefaultAsync(r => r.Id == id, ct);
        }

        public async Task<List<ReservaModel>> GetReservasByUserId(int userId, CancellationToken ct)
        {
            List<ReservaModel> reservas = await _context.Reservas.Where(r => r.IdUser == userId).ToListAsync(ct);

            return reservas;
        }

        public async Task<List<ReservaModel>> GetReservasByDateRange(DateTime startDate, DateTime endDate, CancellationToken ct)
        {
            List<ReservaModel> reservas = await _context.Reservas
                    .Where(r => r.DataInicioReserva >= startDate && r.DataFimReserva <= endDate)
                    .ToListAsync(ct);

            return reservas;
        }

        public async Task CreateReservaAsync(ReservaModel reserva, CancellationToken ct)
        {
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<ReservaModel> UpdateReservaAsync(int id, ReservaModelRequest reservaRequest, CancellationToken ct)
        {
            var reserva = await _context.Reservas.FirstOrDefaultAsync(r => r.Id == id, ct)!;
            
            if (reserva == null) return new ReservaModel { };

            reserva.Reserva = reservaRequest.Reserva;
            reserva.QuantidadeHorasReserva = reservaRequest.QuantidadeHorasReserva;
            reserva.DataFimReserva = reservaRequest.DataFimReserva;
            reserva.DataInicioReserva = reservaRequest.DataInicioReserva;
            reserva.Descricao = reservaRequest.Descricao;
            reserva.HoraInicio = reservaRequest.HoraInicio;

            await _context.SaveChangesAsync(ct);

            return reserva;
        }

        public async Task<bool> DeleteReservaAsync(int id, CancellationToken ct)
        {
            var reserva = await _context.Reservas.FirstOrDefaultAsync(r => r.Id == id, ct);
            if (reserva == null) return false;


            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync(ct);

            return true;
        }
    }
}
