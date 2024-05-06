using reservas.api.Models;

namespace reservas.api.Services.IService
{
    public interface IAdminService
    {
        Task<List<ReservaModel>> GetAllReservasAsync(CancellationToken ct);
        Task<ReservaModel> AceptReservaAsync(ReservaModelResponse reservaResponse, int id, CancellationToken ct);
        Task<ReservaModel> RejectReservaAsync(ReservaModelResponse reservaResponse, int id, CancellationToken ct);

    }
}
