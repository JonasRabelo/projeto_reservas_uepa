using reservas.api.Enums;

namespace reservas.api.Models
{
    public class ReservaModelRequest
    {
        public LocaisReserva Reserva { get; set; }
        public DateTime DataInicioReserva { get; set; }
        public DateTime DataFimReserva { get; set; }
        public string HoraInicio { get; set; }
        public int QuantidadeHorasReserva { get; set; }
        public int IdUser { get; set; }
        
        public string Descricao { get; set; }
    }
}
