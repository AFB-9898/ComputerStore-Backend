using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class AgregarCarritoItemUseCase
    {
        private readonly ICarritoRepository _carritoRepository;

        public AgregarCarritoItemUseCase(ICarritoRepository carritoRepository)
        {
            _carritoRepository = carritoRepository;
        }

        public async Task Ejecutar(Guid carritoId, Guid productoId, int cantidad)
        {
            var carrito = await _carritoRepository.GetByIdAsync(carritoId);
            if (carrito != null)
            {
                var item = new CarritoItem
                {
                    CarritoId = carritoId,
                    ProductoId = productoId,
                    Cantidad = cantidad
                };
                carrito.Items.Add(item);
                await _carritoRepository.UpdateAsync(carrito);
            }
        }
    }
}
