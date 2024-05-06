using reservas.api.Enums;

namespace reservas.api.Models
{
    public class ReservaModelResponse
    {
        public required Status Status { get; set; }
        public required string Justificativa { get; set; }
    }
}
