using api_stoke.Domain.Entities;

namespace api_stoke.Domain.Interfaces
{
    public interface IRegistrationClientsRepositories
    {
        Task<RegistrationClients> Create(RegistrationClients rc);
        Task<RegistrationClients?> Get(Guid id);

    }
}
