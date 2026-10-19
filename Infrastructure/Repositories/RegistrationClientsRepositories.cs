using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;
using api_stoke.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace api_stoke.Infrastructure.Repositories
{
    public class RegistrationClientsRepositories : IRegistrationClientsRepositories
    {
        private readonly AppDbContext _appContext;

        public RegistrationClientsRepositories(AppDbContext appContext)
        {
            _appContext = appContext;
        }

        public async Task<RegistrationClients> Create(RegistrationClients rc)
        {
            _appContext.RegistrationClients.Add(rc);
            await _appContext.SaveChangesAsync();
            return ( await Get(rc.Id) )!;
        }

        public async Task<RegistrationClients?> Get(Guid id)
        {
            return await _appContext.RegistrationClients.FirstOrDefaultAsync(rc=>rc.Id == id);
        }
    }
}
