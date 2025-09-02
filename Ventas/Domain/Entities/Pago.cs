using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Pago
    {
        [Key]
        public Guid UId { get; set; } = Guid.NewGuid();
        public Guid OrdenId { get; set; }
        [JsonIgnore]
        public Orden? Orden { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; } = "Tarjeta";
        public string Estado { get; set; } = "Pendiente";
    }
}
