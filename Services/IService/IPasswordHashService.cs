namespace reservas.api.Services.IService
{
    public interface IPasswordHashService
    {
        public string GeneratePasswordHash(string password);
        public bool VerifyPassword(string password, string passwordHash);
    }
}
