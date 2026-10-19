using api_stoke.Application.DTOs.Cart;
using api_stoke.Application.Exceptions;
using api_stoke.Application.Services;
using api_stoke.Domain.Entities;

namespace api_stoke.Application.UseCases.Carts
{
    public class RemoveProductCartUseCase
    {
        private readonly ProductService _productService;
        private readonly CartService _service;
        private readonly SkuProductService _skuProductService;

        public RemoveProductCartUseCase(
            ProductService productService,
            CartService service,
            SkuProductService skuProductService)
        {
            _productService = productService;
            _service = service;
            _skuProductService = skuProductService;
        }

        public async Task Execute(Guid userId, AddProductCartDTO dto)
        {
            // Buscar produto pelo Id
            Product product = await _productService.GetProductById(dto.ProductId);
            
            // Verificar se o produto não pertence ao usuario
            if (product.UserId == userId)
                throw new UnauthorizedException("Não autorizado !");

            // Buscar o carrinho do usuario
            Cart cart = await _service.Get(userId);

            CartItem ci = await _service.GetUniqueCartItem(cart.Id, product.Id);

            List<SkuProduct> skus = await _skuProductService.ListSkuProductWithString(dto.ProductId, dto.Skus);

            // Remover produto do carrinho
            await _service.RemoveProduct(cart, skus, ci);
        }
    }
}
