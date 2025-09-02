using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Inventario
    {
        [Key]
        public Guid UId { get; set; } = Guid.NewGuid();
        public Guid ProductoId { get; set; }
        public Producto? Producto { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaActualizacion { get; set; } = DateTime.Now;
    }
}
