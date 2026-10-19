using api_stoke.Domain.Entities;

namespace api_stoke.Domain.Interfaces
{
    public interface ISkuCartItemRepositories
    {
        Task<SkuCartItem> Create(SkuCartItem skuCI);
        Task<SkuCartItem?> GetUnique(Guid cartItemId, Guid skuId);
        Task<List<SkuCartItem>?> GetWithCartItemId(Guid cartItemId);
        Task<List<SkuCartItem>?> GetWithSkuProductId(Guid skuProductId);
        Task Update(SkuCartItem skuCI);
    }
}
