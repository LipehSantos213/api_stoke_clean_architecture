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

        public async Task<Movement> Update(Movement movement)
        {
            await _appContext.Movements.Where(m => m.Id == movement.Id).ExecuteUpdateAsync(m=> 
                m.SetProperty(i=> i.Amount, movement.Amount));
            return (await Get(movement.Id))!;
        }

        public async Task<List<Movement>?> GetInputMovement(Guid productId)
        {
            return await
                _appContext.Movements.SelectMany(m=> m.SkuMovements)
                .Where(sm=>sm.SkuProduct.ProductId == productId).Select(sm=>sm.Movement).
                Where(m=>m.TypeMovement == "ENTRADA").ToListAsync();
        }

        public async Task<List<Movement>?> GetOutputMovement(Guid productId)
        {
            return await
                _appContext.Movements.SelectMany(m => m.SkuMovements)
                .Where(sm => sm.SkuProduct.ProductId == productId).Select(sm => sm.Movement).
                Where(m => m.TypeMovement == "SAIDA").ToListAsync();
        }

        public async Task<List<Movement>?> GetAllMovement(Guid productId)
        {
            return await
                _appContext.Movements.SelectMany(m => m.SkuMovements)
                .Where(sm => sm.SkuProduct.ProductId == productId).Select(sm => sm.Movement)
                .ToListAsync();
        }
    }
}
