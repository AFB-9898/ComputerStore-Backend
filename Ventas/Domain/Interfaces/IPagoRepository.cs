using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPagoRepository
    {
        Task<Pago?> GetByIdAsync(Guid id);
        Task<IEnumerable<Pago>> GetByOrdenIdAsync(Guid ordenId);
        Task<IEnumerable<Pago>> GetAllAsync();
        Task AddAsync(Pago pago);
        Task UpdateAsync(Pago pago);
        Task DeleteAsync(Guid id);
    }
}