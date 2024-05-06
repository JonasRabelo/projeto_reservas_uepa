using reservas.api.Models;

namespace reservas.api.Services.IService
{
    public interface ILoginService
    {
        Task<UserModel> GetUsersByEmailAsync(string email, CancellationToken ct);
        Task<string> LoginAsync(LoginModel login, UserModel user, CancellationToken ct);
    }
}
