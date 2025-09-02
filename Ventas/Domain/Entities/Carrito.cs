using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Carrito
    {
        [Key]
        public Guid UId { get; set; } = Guid.NewGuid();
        public Guid UsuarioId { get; set; }
        [JsonIgnore]
        public Usuario? Usuario { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [JsonIgnore]
        public ICollection<CarritoItem> Items { get; set; } = new List<CarritoItem>();
    }
}
