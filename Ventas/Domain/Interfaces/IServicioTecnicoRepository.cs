using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IServicioTecnicoRepository
    {
        Task<ServicioTecnico> GetByIdAsync(Guid id);
        Task<IEnumerable<ServicioTecnico>> GetAllAsync();
        Task AddAsync(ServicioTecnico servicio);
        Task UpdateAsync(ServicioTecnico servicio);
        Task DeleteAsync(Guid id);
    }
}
