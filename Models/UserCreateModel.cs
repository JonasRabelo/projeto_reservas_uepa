using reservas.api.Enums;

namespace reservas.api.Models
{
    public class UserCreateModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Matricula { get; set; }
        public Perfil Perfil { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}
