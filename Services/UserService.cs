using Microsoft.EntityFrameworkCore;
using reservas.api.Data;
using reservas.api.Models;
using reservas.api.Services.IService;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace reservas.api.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHashService _passwordHashService;

        public UserService(AppDbContext context, IPasswordHashService passwordHashService)
        {
            _context = context;
            _passwordHashService = passwordHashService;
        }

        public async Task<UserModel> CreateUserAsync(UserCreateModel userCreate, CancellationToken ct)
        {
            var user = CreateUser(userCreate);

            user.Password = _passwordHashService.GeneratePasswordHash(userCreate.Password);
            await _context.Users.AddAsync(user, ct);
            await _context.SaveChangesAsync(ct);

            return user;
        }

        public async Task<List<UserModel>> GetAllUsersAsync(CancellationToken ct)
        {
            var users = await _context.Users.ToListAsync(ct);

            return users;
        }

        public async Task<UserModel> GetByIdAsync(int id, CancellationToken ct)
        {
            var user = await _context.Users.FindAsync(id, ct);

            return user;
        }

        public async Task<List<UserModel>> GetByNameOrMatriculaAsync(string seach, CancellationToken ct)
        {
            var users = await _context.Users
            .Where(u => u.Matricula.Contains(seach) || u.Name.Contains(seach))
                .ToListAsync(ct);

            return users;
        }

        public async Task<bool> SeachUserByEmailAsync(string email, CancellationToken ct)
        {
            return await _context.Users.AnyAsync(u => u.Email == email, ct);
        }

        public async Task<UserModel> UpdateUserAsync(UserModel user, UserUpdateModel userUpdate, CancellationToken ct)
        {
            user.Name = userUpdate.Name;
            user.Email = userUpdate.Email;
            user.Matricula = userUpdate.Matricula;
            user.Perfil = userUpdate.Perfil;
            user.DataNascimento = userUpdate.DataNascimento;
            user.DataAtualizacao = DateTime.Now;

            await _context.SaveChangesAsync(ct);

            return user;
        }

        public async Task<bool> DeleteUserAsync(int id, CancellationToken ct)
        {
            var user = await GetByIdAsync(id, ct);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync(ct);

            return true;
        }


        private UserModel CreateUser(UserCreateModel model)
        {
            return new UserModel
            {
                Name = model.Name,
                Email = model.Email,
                Matricula = model.Matricula,
                Perfil = model.Perfil,
                DataNascimento = model.DataNascimento,
                DataCadastro = DateTime.Now
            };
        }
    }
}
