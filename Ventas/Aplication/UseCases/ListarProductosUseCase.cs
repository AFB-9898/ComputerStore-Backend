using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class ListarProductosUseCase
    {
        private readonly IProductoRepository _productoRepository;

        public ListarProductosUseCase(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public async Task<IEnumerable<Producto>> Ejecutar()
        {
            return await _productoRepository.GetAllAsync();
        }
    }
}
