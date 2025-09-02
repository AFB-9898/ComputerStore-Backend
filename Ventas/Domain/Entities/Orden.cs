using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Orden
    {
        [Key]
        public Guid UId { get; set; } = Guid.NewGuid();
        public Guid UsuarioId { get; set; }
        [JsonIgnore]
        public Usuario? Usuario { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public string Estado { get; set; } = "Pendiente";

        [JsonIgnore]
        public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    }
}
