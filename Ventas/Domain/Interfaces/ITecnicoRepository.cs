using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITecnicoRepository
    {
        Task<Tecnico> GetByIdAsync(Guid id);
        Task<IEnumerable<Tecnico>> GetAllAsync();
        Task AddAsync(Tecnico tecnico);
        Task UpdateAsync(Tecnico tecnico);
        Task DeleteAsync(Guid id);
    }
}
