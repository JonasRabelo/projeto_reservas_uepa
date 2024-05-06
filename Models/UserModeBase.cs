using reservas.api.Enums;

namespace reservas.api.Models
{
    public abstract class UserModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public Perfil Perfil { get; set; }        
    }
}
