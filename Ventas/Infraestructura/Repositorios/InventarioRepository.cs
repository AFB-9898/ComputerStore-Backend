using Domain.Entities;
using Domain.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infraestructura.Repositorios
{
    public class InventarioRepository : IInventarioRepository
    {
        private readonly BdContext _context;

        public InventarioRepository(BdContext context)
        {
            _context = context;
        }

        public async Task<Inventario?> GetByIdAsync(Guid id) =>
            await _context.Inventarios
                          .Include(i => i.Producto)
                              .ThenInclude(p => p.Categoria)
                          .FirstOrDefaultAsync(i => i.UId == id);

        public async Task<IEnumerable<Inventario>> GetAllAsync() =>
            await _context.Inventarios
                          .Include(i => i.Producto)
                              .ThenInclude(p => p.Categoria)
                          .ToListAsync();

        public async Task AddAsync(Inventario inventario)
        {
            await _context.Inventarios.AddAsync(inventario);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Inventario inventario)
        {
            _context.Inventarios.Update(inventario);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var inventario = await GetByIdAsync(id);
            if (inventario != null)
            {
                _context.Inventarios.Remove(inventario);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Inventario?> GetByProductIdAsync(Guid productId)
        {
            return await _context.Inventarios
                                 .Include(i => i.Producto)
                                     .ThenInclude(p => p.Categoria)
                                 .FirstOrDefaultAsync(i => i.ProductoId == productId);
        }

        public async Task AdjustStockAsync(Guid productId, int cantidadDelta)
        {
            var inventario = await _context.Inventarios
                                           .FirstOrDefaultAsync(i => i.ProductoId == productId);

            if (inventario == null)
            {
                if (cantidadDelta <= 0)
                    throw new InvalidOperationException("No existe inventario para el producto y la cantidad de ajuste es no positiva.");

                inventario = new Inventario
                {
                    UId = Guid.NewGuid(),
                    ProductoId = productId,
                    Cantidad = cantidadDelta,
                    FechaActualizacion = DateTime.UtcNow
                };

                await _context.Inventarios.AddAsync(inventario);
            }
            else
            {
                inventario.Cantidad += cantidadDelta;
                inventario.FechaActualizacion = DateTime.UtcNow;

                if (inventario.Cantidad < 0)
                    throw new InvalidOperationException("El ajuste produce cantidad de inventario negativa.");

                _context.Inventarios.Update(inventario);
            }

            await _context.SaveChangesAsync();
        }
    }
}
