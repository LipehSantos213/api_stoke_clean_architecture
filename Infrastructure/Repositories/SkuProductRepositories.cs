using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;
using api_stoke.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace api_stoke.Infrastructure.Repositories
{
    public class SkuProductRepositories : ISkuProductRepositories
    {
        private readonly AppDbContext _appContext;

        public SkuProductRepositories(AppDbContext appContext)
        {
            _appContext = appContext;
        }

        public async Task<SkuProduct?> GetById(Guid id)
        {
            return await
                _appContext.SkuProducts.FirstOrDefaultAsync(sk => sk.Id == id);
        }

        public async Task<SkuProduct> Create(SkuProduct sku)
        {
            _appContext.SkuProducts.Add(sku);
            await _appContext.SaveChangesAsync();
            return (await GetById(sku.Id))!;
        }

        public async Task<SkuProduct> Update(SkuProduct sku)
        {
            await _appContext.SkuProducts.ExecuteUpdateAsync(sk => sk
                .SetProperty(i=> i.Status, sku.Status)
                .SetProperty(i=>i.Active, sku.Active));
            return (await GetById(sku.Id))!;
        }

        public async Task<SkuProduct?> GetBySku(string sku)
        {
            return await
                _appContext.SkuProducts.FirstOrDefaultAsync(sk=> EF.Functions.Like(sk.Sku, $"%{sku}%"));
        }

        public async Task<SkuProduct?> GetLastSku(Guid productId)
        {
            return await
                _appContext.SkuProducts.Where(sk=> sk.ProductId == productId)
                .OrderByDescending(sk=>sk.CreatedAt).FirstOrDefaultAsync();
        }

        public async Task<List<SkuProduct>> ListSkuOfProduct(Guid productId)
        {
            Console.WriteLine($"[INFO] Buscando Skus do Produto {productId}");
            return await _appContext.SkuProducts.Where(sk => sk.ProductId == productId).ToListAsync();
        }
    }
}
