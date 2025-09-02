using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class AgregarProductoUseCase
    {
        private readonly IProductoRepository _productoRepository;

        public AgregarProductoUseCase(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public async Task Ejecutar(string nombre, decimal precio, int stock, Guid categoriaId)
        {
            var producto = new Producto
            {
                Nombre = nombre,
                Precio = precio,
                StockActual = stock,
                CategoriaId = categoriaId
            };
            await _productoRepository.AddAsync(producto);
        }
    }
}
