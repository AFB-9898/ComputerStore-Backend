using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IOrdenRepository
    {
        Task<Orden?> GetByIdAsync(Guid id);
        Task<IEnumerable<Orden>> GetByUsuarioAsync(Guid usuarioId);
        Task AddAsync(Orden orden);
        Task UpdateAsync(Orden orden);
        Task DeleteAsync(Guid id);
    }
}
