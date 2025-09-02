using Domain.Entities;
using Domain.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios
{
    public class PagoRepository : IPagoRepository
    {
        private readonly BdContext _context;
        public PagoRepository(BdContext context) { _context = context; }

        public async Task<Pago?> GetByIdAsync(Guid id) =>
            await _context.Pagos.Include(p => p.Orden).FirstOrDefaultAsync(p => p.UId == id);

        public async Task<IEnumerable<Pago>> GetByOrdenIdAsync(Guid ordenId) =>
            await _context.Pagos.Where(p => p.OrdenId == ordenId).ToListAsync();

        public async Task<IEnumerable<Pago>> GetAllAsync() =>
            await _context.Pagos.ToListAsync();

        public async Task AddAsync(Pago pago)
        {
            await _context.Pagos.AddAsync(pago);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Pago pago)
        {
            _context.Pagos.Update(pago);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var pago = await GetByIdAsync(id);
            if (pago != null)
            {
                _context.Pagos.Remove(pago);
                await _context.SaveChangesAsync();
            }
        }
    }
}
