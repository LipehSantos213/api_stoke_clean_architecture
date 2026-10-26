using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;
using api_stoke.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace api_stoke.Infrastructure.Repositories
{
    public class SkuCartItemRepositories : ISkuCartItemRepositories
    {
        private readonly AppDbContext _appContext;

        public SkuCartItemRepositories(AppDbContext appContext)
        {
            _appContext = appContext;
        }

        private async Task<SkuCartItem?> Get(Guid id)
        {
            return await
                _appContext.SkuCartItens.FirstOrDefaultAsync(sc => sc.Id == id);
        }

        public async Task<SkuCartItem> Create(SkuCartItem skuCI)
        {
            await _appContext.SkuCartItens.AddAsync(skuCI);
            return (await Get(skuCI.Id))!;
        }

        public async Task<SkuCartItem?> GetUnique(Guid cartItemId, Guid skuId)
        {
            return await 
                _appContext.SkuCartItens
                    .FirstOrDefaultAsync(sc => sc.CartItemId == cartItemId 
                                && sc.SkuId == skuId);
        }

        public async Task<List<SkuCartItem>?> GetWithCartItemId(Guid cartItemId)
        {
            return await 
                _appContext.CartItens
                    .Where(c => c.Id == cartItemId)
                    .SelectMany(c => c.SkuCartItens).ToListAsync();
        }

        public async Task<List<SkuCartItem>?> GetWithSkuProductId(Guid skuProductId)
        {
            return await
                _appContext.SkuProducts
                    .Where(c => c.Id == skuProductId)
                    .SelectMany(c => c.SkuCartItens).ToListAsync();
        }

        public async Task Update(SkuCartItem skuCI)
        {
            await _appContext.SkuCartItens.ExecuteUpdateAsync(sc => sc.
                SetProperty(i => i.Active, skuCI.Active));
        }
    }
}