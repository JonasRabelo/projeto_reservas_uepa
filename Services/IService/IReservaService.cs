using Microsoft.OpenApi.Models;
using reservas.api.Models;

namespace reservas.api.Services.IService
{
    public interface IReservaService
    {
        Task<ResultValidation> ValidateReservaAsync(ReservaModel reserva, CancellationToken ct);
        Task<List<ReservaModel>> GetAllReservaAsync(CancellationToken ct);
        Task<ReservaModel> GetReservaByIdAsync(int id, CancellationToken ct);
        Task<List<ReservaModel>> GetReservasByUserId(int userId, CancellationToken ct);
        Task<List<ReservaModel>> GetReservasByDateRange(DateTime startDate, DateTime endDate, CancellationToken ct);

        Task CreateReservaAsync(ReservaModel reserva, CancellationToken ct);
        Task<ReservaModel> UpdateReservaAsync(int id, ReservaModelRequest reservaRequest, CancellationToken ct);
        Task<bool> DeleteReservaAsync(int id, CancellationToken ct);
    }
}
