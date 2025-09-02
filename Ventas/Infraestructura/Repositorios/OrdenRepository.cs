using Domain.Entities;
using Domain.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios
{
    public class OrdenRepository : IOrdenRepository
    {
        private readonly BdContext _context;

        public OrdenRepository(BdContext context)
        {
            _context = context;
        }

        public async Task<Orden> GetByIdAsync(Guid id) =>
            await _context.Ordenes.FindAsync(id);

        public async Task<IEnumerable<Orden>> GetAllAsync() =>
            await _context.Ordenes.ToListAsync();

        public async Task AddAsync(Orden orden)
        {
            await _context.Ordenes.AddAsync(orden);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Orden orden)
        {
            _context.Ordenes.Update(orden);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var orden = await GetByIdAsync(id);
            if (orden != null)
            {
                _context.Ordenes.Remove(orden);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Orden>> GetByUsuarioAsync(Guid usuarioId)
        {
            return await _context.Ordenes
                                 .Where(o => o.UsuarioId == usuarioId)
                                 .ToListAsync();
        }
    }
}
