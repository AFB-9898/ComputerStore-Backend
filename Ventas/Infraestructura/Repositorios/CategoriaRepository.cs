using Domain.Entities;
using Domain.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infraestructura.Repositorios
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly BdContext _context;
        public CategoriaRepository(BdContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categoria>> GetAllAsync()
        {
            // Incluye productos si quieres que la respuesta traiga la relación
            return await _context.Categorias
                                 .Include(c => c.Productos)
                                 .ToListAsync();
        }

        public async Task<Categoria?> GetByIdAsync(Guid id)
        {
            return await _context.Categorias
                                 .Include(c => c.Productos)
                                 .FirstOrDefaultAsync(c => c.UId == id);
        }

        public async Task AddAsync(Categoria categoria)
        {
            await _context.Categorias.AddAsync(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Categoria categoria)
        {
            _context.Categorias.Update(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entidad = await GetByIdAsync(id);
            if (entidad != null)
            {
                _context.Categorias.Remove(entidad);
                await _context.SaveChangesAsync();
            }
        }
    }
}
