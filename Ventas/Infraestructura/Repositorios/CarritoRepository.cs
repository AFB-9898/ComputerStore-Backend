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
    public class CarritoRepository : ICarritoRepository
    {
        private readonly BdContext _context;

        public CarritoRepository(BdContext context)
        {
            _context = context;
        }

        public async Task<Carrito?> GetByIdAsync(Guid id)
        {
            return await _context.Carritos
                                 .Include(c => c.Items)
                                 .FirstOrDefaultAsync(c => c.UId == id);
        }

        public async Task<Carrito?> GetByItemIdAsync(Guid itemId)
        {
            return await _context.Carritos
                                 .Include(c => c.Items)
                                 .FirstOrDefaultAsync(c => c.Items.Any(i => i.UId == itemId));
        }

        public async Task<IEnumerable<Carrito>> GetAllAsync()
        {
            return await _context.Carritos
                                 .Include(c => c.Items)
                                 .ToListAsync();
        }

        public async Task AddAsync(Carrito carrito)
        {
            // Asegurar UId y FK en los items nuevos
            if (carrito.Items != null)
            {
                foreach (var it in carrito.Items)
                {
                    if (it.UId == Guid.Empty) it.UId = Guid.NewGuid();
                    it.CarritoId = carrito.UId; // FK
                }
            }

            await _context.Carritos.AddAsync(carrito);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Carrito carrito)
        {
            // Traer el carrito existente con items (tracked)
            var existing = await _context.Carritos
                                         .Include(c => c.Items)
                                         .FirstOrDefaultAsync(c => c.UId == carrito.UId);

            if (existing == null)
                throw new KeyNotFoundException("Carrito no encontrado");

            // Sincronizar campos simples del carrito (si hay otros campos a mantener, actualízalos)
            existing.FechaCreacion = carrito.FechaCreacion;
            existing.UsuarioId = carrito.UsuarioId;

            // Sincronizar items:
            carrito.Items ??= new List<CarritoItem>();
            existing.Items ??= new List<CarritoItem>();

            // 1) Actualizar y añadir
            foreach (var item in carrito.Items)
            {
                var exItem = existing.Items.FirstOrDefault(i => i.UId == item.UId);

                if (exItem == null)
                {
                    // nuevo item
                    if (item.UId == Guid.Empty) item.UId = Guid.NewGuid();
                    item.CarritoId = existing.UId; // asegurar FK
                    existing.Items.Add(item);
                }
                else
                {
                    // actualizar campos relevantes
                    exItem.Cantidad = item.Cantidad;
                    exItem.ProductoId = item.ProductoId;
                    // si tienes snapshot de precio o nombre, actualízalos aquí también
                }
            }

            // 2) Eliminar items que ya no están
            var itemsToRemove = existing.Items
                                        .Where(i => !carrito.Items.Any(ci => ci.UId == i.UId))
                                        .ToList();

            foreach (var rem in itemsToRemove)
            {
                existing.Items.Remove(rem);
                // opcional: _context.CarritoItems.Remove(rem); EF se encarga al guardar si está configurado correctamente
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var carrito = await GetByIdAsync(id);
            if (carrito != null)
            {
                _context.Carritos.Remove(carrito);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Carrito>> GetByUsuarioAsync(Guid usuarioId)
        {
            return await _context.Carritos
                                 .Where(c => c.UsuarioId == usuarioId)
                                 .Include(c => c.Items)
                                 .ToListAsync();
        }
    }
}
