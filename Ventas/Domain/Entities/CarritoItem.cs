using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class CarritoItem
    {
        [Key]
        public Guid UId { get; set; } = Guid.NewGuid();
        public Guid CarritoId { get; set; }
        [JsonIgnore]
        public Carrito? Carrito { get; set; }
        public Guid ProductoId { get; set; }
        [JsonIgnore]
        public Producto? Producto { get; set; }
        public int Cantidad { get; set; }
        public Guid ProductId { get; set; }
    }
}
