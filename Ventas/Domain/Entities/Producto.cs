using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Producto
    {
        [Key]
        public Guid UId { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int StockActual { get; set; }
        public string? ImagenUrl { get; set; }

        // Relación con Categoria
        public Guid CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        [JsonIgnore]
        public ICollection<CarritoItem> CarritoItems { get; set; } = new List<CarritoItem>();

        [JsonIgnore]
        public ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();
    }
}
