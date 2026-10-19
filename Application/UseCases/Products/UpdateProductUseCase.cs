using api_stoke.Application.DTOs.Products;
using api_stoke.Application.Services;
using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;

namespace api_stoke.Application.UseCases.Products
{
    public class UpdateProductUseCase
    {
        private readonly IProductRepositories _repository;
        private readonly ProductService _service;
        private readonly UserService _userService;

        public UpdateProductUseCase(
            IProductRepositories repository,
            ProductService service,
            UserService userService)
        {
            _repository = repository;
            _service = service;
            _userService = userService;
        }

        public async Task<ResponseProductDTO> Execute(Guid userId, Guid productId, UpdateProductDTO dto)
        {
            User user = await _userService.FindUser(userId);
            Product product = await _service.GetProductById(productId);
            if (user.Id != product.UserId)
                throw new Exception("Não autorizado !");

            product.Update(dto.Name, dto.Description, dto.Price);

            var res = await _repository.Update(product);

            return ResponseProductDTO.fromDomain(res, true);
        }
    }
}
