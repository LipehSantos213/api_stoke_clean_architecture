using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;
using api_stoke.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace api_stoke.Infrastructure.Repositories
{
    public class SkuMovementRepositories : ISkuMovementRepositories
    {
        public readonly AppDbContext _appContext;

        public SkuMovementRepositories(AppDbContext appContext)
        {
            _appContext = appContext;
        }

        public async Task<SkuMovement?> Get(Guid id)
        {
            return await
                _appContext.SkuMovements.FirstOrDefaultAsync(sm => sm.Id == id);
        }

        public async Task<SkuMovement> Create(SkuMovement skuMovement)
        {
            _appContext.SkuMovements.Add(skuMovement);
            await _appContext.SaveChangesAsync();
            return (await Get(skuMovement.Id))!;
        }
    }
}
