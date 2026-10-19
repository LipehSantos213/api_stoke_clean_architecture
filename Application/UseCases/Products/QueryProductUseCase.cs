using api_stoke.Application.DTOs.Products;
using api_stoke.Application.Services;
using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;

namespace api_stoke.Application.UseCases.Products
{
    public class QueryProductUseCase
    {
        private readonly ProductService _service;

        public QueryProductUseCase(ProductService service)
        {
            _service = service;
        }

        public async Task<ResponseProductDTO> Execute(Guid userId, Guid productId)
        {
            var product = await _service.GetProductById(productId);
            if (product.UserId != userId)
                throw new UnauthorizedAccessException("Não é possivel acessar !");
            return ResponseProductDTO.fromDomain(product, true);
        }
    }
}
