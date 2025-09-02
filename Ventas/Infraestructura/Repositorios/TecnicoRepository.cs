using Domain.Entities;
using Domain.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios
{
    public class TecnicoRepository : ITecnicoRepository
    {
        private readonly BdContext _context;

        public TecnicoRepository(BdContext context)
        {
            _context = context;
        }

        public async Task<Tecnico> GetByIdAsync(Guid id) =>
            await _context.Tecnicos.FindAsync(id);

        public async Task<IEnumerable<Tecnico>> GetAllAsync() =>
            await _context.Tecnicos.ToListAsync();

        public async Task AddAsync(Tecnico tecnico)
        {
            await _context.Tecnicos.AddAsync(tecnico);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tecnico tecnico)
        {
            _context.Tecnicos.Update(tecnico);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var tecnico = await GetByIdAsync(id);
            if (tecnico != null)
            {
                _context.Tecnicos.Remove(tecnico);
                await _context.SaveChangesAsync();
            }
        }
    }
}
