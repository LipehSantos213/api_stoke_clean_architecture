using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;
using api_stoke.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace api_stoke.Infrastructure.Repositories
{
    public class MovementRepositories : IMovementRepositories
    {
        private readonly AppDbContext _appContext;

        public MovementRepositories(AppDbContext appContext)
        {
            _appContext = appContext;
        }

        public async Task<Movement> Create(Movement movement)
        {
            _appContext.Movements.Add(movement);
            await _appContext.SaveChangesAsync();
            return (await Get(movement.Id))!;
        }

        public async Task<Movement?> Get(Guid id)
        {
            return 
                await _appContext.Movements
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
