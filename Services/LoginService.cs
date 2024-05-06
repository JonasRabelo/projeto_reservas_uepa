using Microsoft.EntityFrameworkCore;
using reservas.api.Data;
using reservas.api.Models;
using reservas.api.Services.IService;

namespace reservas.api.Services
{
    public class LoginService : ILoginService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHashService _passwordHashService;
        private readonly ITokenService _tokenService;

        public LoginService(AppDbContext context, 
                            IPasswordHashService passwordHashService, 
                            ITokenService tokenService)
        {
            _context = context;
            _passwordHashService = passwordHashService;
            _tokenService = tokenService;
        }


        public async Task<UserModel> GetUsersByEmailAsync(string email, CancellationToken ct)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            return user!;
        }

        public async Task<string> LoginAsync(LoginModel login, UserModel user, CancellationToken ct)
        {
            if (!_passwordHashService.VerifyPassword(login.Password, user.Password))
                return "";

            var token = _tokenService.Generate(user);

            return token;
        }
    }
}
