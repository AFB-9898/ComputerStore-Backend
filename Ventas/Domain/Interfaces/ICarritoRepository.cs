using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICarritoRepository
    {
        Task<Carrito?> GetByItemIdAsync(Guid itemId);
        Task<IEnumerable<Carrito>> GetAllAsync();
        Task AddAsync(Carrito carrito);
        Task UpdateAsync(Carrito carrito);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Carrito>> GetByUsuarioAsync(Guid usuarioId);
        Task<Carrito> GetByIdAsync(Guid carritoId);
    }
}
