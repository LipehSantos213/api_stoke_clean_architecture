using api_stoke.Application.DTOs.Movements;
using api_stoke.Application.Exceptions;
using api_stoke.Application.Services;
using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;

namespace api_stoke.Application.UseCases.Movements
{
    public class OutputMovementUseCase
    {
        private readonly ProductService _productService;
        private readonly UserService _userService;
        private readonly SkuProductService _skuProductService;

        public OutputMovementUseCase(
            ProductService productService,
            UserService userService,
            SkuProductService skuProductService)
        {
            _productService = productService;
            _userService = userService;
            _skuProductService = skuProductService;
        }

        public async Task<ResponseMovementDTO> Execute(Guid userId, Guid productId, RegisterMovementDTO dto)
        {
            User user = await _userService.FindUser(userId);
            Product p = await _productService.GetProductById(productId);
            if (p.UserId != userId)
                throw new UnauthorizedAccessException("Ação não autorizada !");

            if (dto.Observation == null)
                throw new UnprocessableException("Forneça a observação desse histórico de movimentação !");
            if (dto.Skus.Count == 0)
                throw new UnprocessableException("Forneça os skus do produto para serem removidos !");

            List<SkuProduct> skus = await _skuProductService.ListSkuProductWithString(productId, dto.Skus);

            Movement movem = await _skuProductService.RemoveSkusOfProduct(p, skus, dto.Observation);

            return ResponseMovementDTO.fromDomain(movem);
        }
    }
}