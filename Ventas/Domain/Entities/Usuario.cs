using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Usuario
    {
        [Key]
        public Guid UId { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Rol { get; set; } = "Cliente";
        public string PasswordHash { get; set; } = null!;

        [JsonIgnore]
        public ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();

        [JsonIgnore]
        public ICollection<Orden> Ordenes { get; set; } = new List<Orden>();

        [JsonIgnore]
        public ICollection<ServicioTecnico> ServiciosTecnicos { get; set; } = new List<ServicioTecnico>();
    }
}
