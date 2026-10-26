using api_stoke.Domain.Entities;

namespace api_stoke.Domain.Interfaces
{
    public interface ISkuProductRepositories
    {
        Task<SkuProduct> Create(SkuProduct sku);

        Task<SkuProduct> Update(SkuProduct sku);

        Task<SkuProduct?> GetBySku(string sku);

        Task<SkuProduct?> GetById(Guid id);

        Task<SkuProduct?> GetLastSku(Guid productId);
    }
}
