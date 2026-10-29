using api_stoke.Domain.Entities;

namespace api_stoke.Domain.Interfaces
{
    public interface IProductRepositories
    {
        Task<Product> Create(Product product);
        Task<Product> Update(Product product);
        Task<Product?> Get(Guid id);

        Task<List<Product>?> GetProductsOfUser(Guid userId, string? name, bool? active, int? valueStokeMin);
    }
}
