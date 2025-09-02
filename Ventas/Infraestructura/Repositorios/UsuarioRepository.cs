using Domain.Entities;
using Domain.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly BdContext _context;

        public UsuarioRepository(BdContext context)
        {
            _context = context;
        }

        public async Task<Usuario> GetByIdAsync(Guid id) =>
            await _context.Usuarios.FindAsync(id);

        public async Task<IEnumerable<Usuario>> GetAllAsync() =>
            await _context.Usuarios.ToListAsync();

        public async Task AddAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var usuario = await GetByIdAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
