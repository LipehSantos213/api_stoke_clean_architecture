using api_stoke.Application.DTOs.Cart;
using api_stoke.Application.Services;
using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;

namespace api_stoke.Application.UseCases.Carts
{
    public class AddProductCartUseCase
    {
        private readonly ProductService _productService;
        private readonly CartService _service;
        private readonly SkuProductService _skuProductService;
        private readonly ICartItemRepositories _cartItemRepository;

        public AddProductCartUseCase(
            ProductService productService,
            CartService service,
            SkuProductService skuProductService, ICartItemRepositories cartItemRepository)
        {
            _productService = productService;
            _service = service;
            _skuProductService = skuProductService;
            _cartItemRepository = cartItemRepository;
        }

        /// <summary>
        /// Caso de Uso para usuario adicionar um produto ao seu carrinho
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task Execute(Guid userId, AddProductCartDTO dto)
        {
            // Buscar produto pelo ID
            Product product = await _productService.GetProductById(dto.ProductId);

            // Verificar se o produto não pertence ao usuario atual
            if (product.UserId == userId)
                throw new Exception("Não autorizado !");

            // Buscar o carrinho do usuario
            Cart cart = await _service.Get(userId);

            CartItem ci = await _service.GetUniqueCartItem(cart.Id, product.Id);
            if (!ci.Active)
            {
                ci.UpdateStatus(true); // Ativa carrinho
                ci.UpdatePrice(product.Price);
                await _cartItemRepository.Update(ci);
            }

            List<SkuProduct> skus = await _skuProductService.ListSkuProductWithString(product.Id, dto.Skus);

            // Adicionar no carrinho do usuario o produto escolhido
            await _service.AddProduct(product, cart, skus, ci);
        }
    }
}
