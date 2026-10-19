using api_stoke.Domain.Entities;

namespace api_stoke.Domain.Interfaces
{
    public interface ICartItemRepositories
    {
        Task<CartItem> Create(CartItem item);
        Task<CartItem> Update(CartItem item);

        Task<CartItem> Remove(Guid id);
        Task<CartItem?> Get(Guid id);
        Task<CartItem?> GetUnique(Guid cartId, Guid productId);
        Task<List<CartItem>?> GetAll(Guid cartId);
    }
}
