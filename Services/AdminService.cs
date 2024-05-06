using Microsoft.EntityFrameworkCore;
using reservas.api.Data;
using reservas.api.Models;
using reservas.api.Services.IService;

namespace reservas.api.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;
        private readonly IReservaService _reservaService;

        public AdminService(AppDbContext context, IReservaService reservaService)
        {
            _context = context;            
            _reservaService = reservaService;
        }


        public async Task<List<ReservaModel>> GetAllReservasAsync(CancellationToken ct)
        {
            return await _context.Reservas.ToListAsync(ct);
        }
        public async Task<ReservaModel> AceptReservaAsync(ReservaModelResponse reservaResponse,int id, CancellationToken ct)
        {
            var reserva = await _reservaService.GetReservaByIdAsync(id, ct);
            if (reserva == null) return new ReservaModel { };

            reserva.Status = reservaResponse.Status;
            reserva.Justificativa = reservaResponse.Justificativa;
            reserva.HoraResposta = DateTime.Now;

            await _context.SaveChangesAsync(ct);

            return reserva;
        }

        public async Task<ReservaModel> RejectReservaAsync(ReservaModelResponse reservaResponse, int id, CancellationToken ct)
        {
            var reserva = await _reservaService.GetReservaByIdAsync(id, ct);

            if (reserva == null) return new ReservaModel { };

            reserva.Status = reservaResponse.Status;
            reserva.Justificativa = reservaResponse.Justificativa;
            reserva.HoraResposta = DateTime.Now;

            await _context.SaveChangesAsync(ct);

            return reserva;
        }
    }
}
