using api_stoke.Domain.Entities;
using System.Runtime.CompilerServices;

namespace api_stoke.Domain.Interfaces
{
    public interface ICartRepositories
    {
        Task<Cart> Create(Cart cart);
        Task<Cart?> Get(Guid id);
        Task<Cart?> GetCartOfUser(Guid userId);
        Task Update(Cart cart);
        
    }
}
