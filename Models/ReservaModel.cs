using reservas.api.Enums;

namespace reservas.api.Models
{
    public class ReservaModel
    {
        public int Id { get; set; }
        public Status Status { get; set; }
        public string? Justificativa { get; set; }
        public LocaisReserva Reserva { get; set; }
        public DateTime? HoraResposta { get; set; }
        public DateTime HoraSolicitacao { get; set; }
        public DateTime DataInicioReserva { get; set; }
        public DateTime DataFimReserva { get; set; }
        //public DateTime? Edicao { get; set; }
        public string HoraInicio { get; set; }
        public int QuantidadeHorasReserva { get; set; }
        public int IdUser { get; set; }
        public string Descricao { get; set; }

        public UserModel? User { get; set; }
    }
}
