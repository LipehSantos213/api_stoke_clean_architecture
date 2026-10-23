using api_stoke.Application.DTOs.Movements;
using api_stoke.Application.DTOs.Products;
using api_stoke.Application.Exceptions;
using api_stoke.Application.Services;
using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;

namespace api_stoke.Application.UseCases.Movements
{
    public class InputMovementUseCase
    {
        //private readonly IMovementRepositories _repository;
        private readonly ProductService _productService;
        private readonly UserService _userService;
        private readonly SkuProductService _skuProductService;


        public InputMovementUseCase(
            //IMovementRepositories repository,
            ProductService productService,
            UserService userService,
            SkuProductService skuProductService)
        {
            //_repository = repository;
            _productService = productService;
            _userService = userService;
            _skuProductService = skuProductService;
        }

        public async Task<ResponseMovementDTO> Execute(Guid userId, Guid productId, RegisterMovementDTO dto)
        {
            // Busca o usuario pelo Id
            User user = await _userService.FindUser(userId);

            // Buscar o produto pelo Id
            Product product = await _productService.GetProductById(productId);
            
            // Verificar se o produto pertence ao usuario atual
            if (product.UserId != userId)
                throw new UnauthorizedException("Não autorizado !");

            Movement m = await _skuProductService.AddSkusOfProduct(productId, product.Name, dto.Amount);

            return ResponseMovementDTO.fromDomain(m);
        }
    }
}