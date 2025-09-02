using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Categoria
    {
        [Key]
        public Guid UId { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public bool Estado { get; set; } = true;

        [JsonIgnore]
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
