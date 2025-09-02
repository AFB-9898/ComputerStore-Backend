using Domain.Entities;
using Domain.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infraestructura.Repositorios
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly BdContext _context;
        public ProductoRepository(BdContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Producto producto)
        {
            await _context.Productos.AddAsync(producto);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(Guid id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Producto>> GetAllAsync()
        {
            return await _context.Productos.Include(p => p.Categoria).ToListAsync();
        }

        public async Task<Producto?> GetByIdAsync(Guid id)
        {
            return await _context.Productos.Include(p => p.Categoria)
                                           .FirstOrDefaultAsync(p => p.UId == id);
        }

        public async Task UpdateAsync(Producto producto)
        {
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
        }
    }
}
