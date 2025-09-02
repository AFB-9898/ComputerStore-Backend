using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class CarritoItemDto
    {
        public Guid UId { get; set; }
        public Guid CarritoId { get; set; }
        public Guid ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}