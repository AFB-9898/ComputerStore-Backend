using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> GetAllAsync();
        Task<Producto?> GetByIdAsync(Guid id);
        Task AddAsync(Producto producto);
        Task UpdateAsync(Producto producto);
        Task DeleteAsync(Guid id);
    }
}
