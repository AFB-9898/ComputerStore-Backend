using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Tecnico
    {
        [Key]
        public Guid UId { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = null!;
        public string Especialidad { get; set; } = null!;

        [JsonIgnore]
        public ICollection<ServicioTecnico> Servicios { get; set; } = new List<ServicioTecnico>();
    }
}
