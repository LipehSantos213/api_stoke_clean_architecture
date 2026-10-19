using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;

namespace api_stoke.Application.Services
{
    public class RegistrationClientsService
    {
        private readonly IRegistrationClientsRepositories _repository;

        public RegistrationClientsService(IRegistrationClientsRepositories repository)
        {
            _repository = repository;
        }

        public async Task<RegistrationClients> GetById(Guid id)
        {
            var rc = await _repository.Get(id);
            return rc ??
                throw new Exception("Não encontrado !");
        }
        
    }
}
