using api_stoke.Domain.Entities;

namespace api_stoke.Domain.Interfaces
{
    public interface IUserRepositories
    {
        Task<User> Create(User user);
        Task<User?> GetById(Guid id);
        Task<User?> GetByEmail(string email);
        Task<User> Update(User user);
    }
}
