using api_stoke.Application.DTOs.Movements;
using api_stoke.Application.DTOs.Products;
using api_stoke.Application.Exceptions;
using api_stoke.Application.Services;

namespace api_stoke.Application.UseCases.Movements
{
    public class ListMovementUseCase
    {
        private readonly MovementService _service;
        private readonly UserService _userService;
        private readonly ProductService _productService;

        public ListMovementUseCase(
            MovementService service,
            UserService userService,
            ProductService productService)
        {
            _service = service;
            _userService = userService;
            _productService = productService;
        }

        public async Task<(ResponseProductDTO, List<ResponseMovementDTO>)> Execute(Guid userId, Guid productId, string? movement)
        {
            // Verificar se a Query Param enviado pelo usuario é valida
            if(movement != null && movement != "ENTRADA" && movement != "ENTRADA")
                throw new UnprocessableException("Query invalida !");

            // Buscar o usuario
            var user = await _userService.FindUser(userId);

            // Buscando o produto no banco
            var product = await _productService.GetProductById(productId);
            var responseProduct = ResponseProductDTO.fromDomain(product, true);

            // Verificar se o produto pertence ao usuario
            if (!(user.Id.Equals(product.UserId)))
                throw new UnauthorizedException("Não autorizado !");

            switch (movement)
            {
                case "ENTRADA":
                    return (
                        responseProduct,
                        await _service.ListInputMovement(productId));
                case "SAIDA":
                    return (
                        responseProduct,
                        await _service.ListOutputMovement(productId));
                default:
                    return (
                        responseProduct,
                        await _service.ListAllMovement(productId));
            }
        }
    }
}
