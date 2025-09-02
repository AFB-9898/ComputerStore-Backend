using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs
{
    public class ProductoDTO
    {
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int StockActual { get; set; }
        public Guid CategoriaId { get; set; }
        public string? ImagenUrl { get; set; }
    }

}
