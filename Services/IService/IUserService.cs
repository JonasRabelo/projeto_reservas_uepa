using reservas.api.Models;

namespace reservas.api.Services.IService
{
    public interface IUserService
    {
        Task<List<UserModel>> GetAllUsersAsync(CancellationToken ct);
        Task<UserModel> GetByIdAsync(int id, CancellationToken ct);
        Task<List<UserModel>> GetByNameOrMatriculaAsync(string seach, CancellationToken ct);
        Task<bool> SeachUserByEmailAsync(string email, CancellationToken ct);
        Task<UserModel> CreateUserAsync(UserCreateModel user, CancellationToken ct);
        Task<UserModel> UpdateUserAsync(UserModel user, UserUpdateModel userUpdate, CancellationToken ct);
        Task<bool> DeleteUserAsync(int id, CancellationToken ct);
    }
}
