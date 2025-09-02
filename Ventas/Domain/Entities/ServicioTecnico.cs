using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class ServicioTecnico
    {
        [Key]
        public Guid UId { get; set; } = Guid.NewGuid();
        public Guid UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public Guid TecnicoId { get; set; }
        public Tecnico? Tecnico { get; set; }
        public string Descripcion { get; set; } = null!;
        public string Estado { get; set; } = "Solicitado";
    }
}
