using api_stoke.Application.Exceptions;
using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;

namespace api_stoke.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepositories _repository;

        public ProductService(IProductRepositories repository)
        {
            _repository = repository;
        }

        public async Task<Product> GetProductById(Guid id)
        {
            Product product = await _repository.Get(id) ?? 
                throw new NotFoundException("Produto não encontrado !");
            return product;
        }

        public async Task<Product> InputProductMovement(Guid productId, int amount)
        {
            var product = await GetProductById(productId);

            product.AddAmount(amount);

            return await _repository.Update(product);
        }

        public async Task<Product> OutputProductMovement(Product product,int amount)
        {
            product.SubtraAmount(amount);

            return await _repository.Update(product);
        }
    }
}
