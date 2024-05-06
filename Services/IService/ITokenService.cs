using reservas.api.Models;

namespace reservas.api.Services.IService
{
    public interface ITokenService
    {
        public string Generate(UserModel user);
    }
}
