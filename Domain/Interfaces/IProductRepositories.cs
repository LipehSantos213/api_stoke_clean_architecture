using api_stoke.Domain.Entities;

namespace api_stoke.Domain.Interfaces
{
    public interface IProductRepositories
    {
        Task<Product> Create(Product product);
        Task<Product> Update(Product product);
        Task<Product?> Get(Guid id);

        Task<List<Product>?> GetName(Guid userId, string name);
        Task<List<Product>?> GetStatus(Guid userId, bool value);
        Task<List<Product>?> GetStokeMin(Guid userId, int stokeMin);
    }
}
