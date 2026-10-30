using api_stoke.Application.DTOs.Products;
using api_stoke.Application.DTOs.SkuProducts;
using api_stoke.Application.Services;
using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;

namespace api_stoke.Application.UseCases.Products
{
    public class QueryProductUseCase
    {
        private readonly ProductService _service;
        private readonly ISkuProductRepositories _skuProductRepository;

        public QueryProductUseCase(ProductService service, ISkuProductRepositories skuProductRepository)
        {
            _service = service;
            _skuProductRepository = skuProductRepository;
        }

        public async Task<ResponseProductDTO> Execute(Guid userId, Guid productId)
        {
            var product = await _service.GetProductById(productId);
            if (product.UserId != userId)
                throw new UnauthorizedAccessException("Não é possivel acessar !");

            Console.WriteLine($"[INFO] Produto {product.Id} encontrado !");

            List<SkuProduct> skusObj = await _skuProductRepository.ListSkuOfProduct(productId);
            Console.WriteLine($"[INFO] {skusObj.Count} Skus encontrados do Produto {productId}");

            List<ResponseSkuProductDTO>? skusRes = new();
            foreach (SkuProduct sk in skusObj)
            {
                Console.WriteLine($"[INFO] Sku {sk.Id} Adicionado na lista de retorno");
                skusRes.Add(ResponseSkuProductDTO.fromDomain(sk));
            }

            return ResponseProductDTO.fromDomain(product, skus:skusRes);
        }
    }
}
