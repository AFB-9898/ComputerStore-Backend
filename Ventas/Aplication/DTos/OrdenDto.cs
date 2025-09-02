using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class OrdenDto
    {
        public Guid UId { get; set; }
        public Guid UsuarioId { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = "Pendiente";
    }
}