using Domain.Entities;
using Domain.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infraestructura.Repositorios
{
    public class ServicioTecnicoRepository : IServicioTecnicoRepository
    {
        private readonly BdContext _context;

        public ServicioTecnicoRepository(BdContext context)
        {
            _context = context;
        }

        // Trae un servicio técnico por Id incluyendo Usuario y Técnico
        public async Task<ServicioTecnico?> GetByIdAsync(Guid id)
        {
            return await _context.ServiciosTecnicos
                                 .Include(s => s.Usuario)   // Incluye datos del usuario
                                 .Include(s => s.Tecnico)   // Incluye datos del técnico
                                 .FirstOrDefaultAsync(s => s.UId == id);
        }

        // Trae todos los servicios técnicos incluyendo Usuario y Técnico
        public async Task<IEnumerable<ServicioTecnico>> GetAllAsync()
        {
            return await _context.ServiciosTecnicos
                                 .Include(s => s.Usuario)
                                 .Include(s => s.Tecnico)
                                 .ToListAsync();
        }

        public async Task AddAsync(ServicioTecnico servicio)
        {
            await _context.ServiciosTecnicos.AddAsync(servicio);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ServicioTecnico servicio)
        {
            _context.ServiciosTecnicos.Update(servicio);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var servicio = await GetByIdAsync(id);
            if (servicio != null)
            {
                _context.ServiciosTecnicos.Remove(servicio);
                await _context.SaveChangesAsync();
            }
        }

        // Opcionales: Métodos para filtrar por Usuario o Técnico directamente en DB
        public async Task<IEnumerable<ServicioTecnico>> GetByUsuarioAsync(Guid usuarioId)
        {
            return await _context.ServiciosTecnicos
                                 .Include(s => s.Usuario)
                                 .Include(s => s.Tecnico)
                                 .Where(s => s.UsuarioId == usuarioId)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<ServicioTecnico>> GetByTecnicoAsync(Guid tecnicoId)
        {
            return await _context.ServiciosTecnicos
                                 .Include(s => s.Usuario)
                                 .Include(s => s.Tecnico)
                                 .Where(s => s.TecnicoId == tecnicoId)
                                 .ToListAsync();
        }
    }
}
