using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;

namespace api_stoke.Application.Services
{
    public class UserService
    {
        private readonly IUserRepositories _repository;

        public UserService(IUserRepositories repository)
        {
            _repository = repository;
        }

        public async Task<User> FindByEmail(string email)
        {
            var user = await _repository.GetByEmail(email)??
                throw new Exception("Usuario não encontrado !");
            return user;
        }
    
        public async Task<User> FindUser(Guid userId)
        {
            var user = await _repository.GetById(userId) ??
                throw new Exception("Usuario não encontrado !");
            return user!;
        }
    }
}
 