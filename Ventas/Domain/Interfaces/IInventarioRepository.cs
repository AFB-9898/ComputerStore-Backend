using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IInventarioRepository
    {
        Task<Inventario?> GetByIdAsync(Guid id);
        Task<IEnumerable<Inventario>> GetAllAsync();
        Task AddAsync(Inventario inventario);
        Task UpdateAsync(Inventario inventario);
        Task DeleteAsync(Guid id);

        // Nuevos métodos
        Task<Inventario?> GetByProductIdAsync(Guid productId);
        Task AdjustStockAsync(Guid productId, int cantidadDelta);
    }
}
