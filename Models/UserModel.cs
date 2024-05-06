using reservas.api.Enums;
using System.Security.Cryptography;
using System.Text;

namespace reservas.api.Models
{
    public class UserModel : UserModelBase
    {
        public string Matricula { get; set; }
        public List<ReservaModel> Reservas { get; set; }
    }
}
