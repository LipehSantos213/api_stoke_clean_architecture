using api_stoke.Application.DTOs.Movements;
using api_stoke.Application.DTOs.Products;
using api_stoke.Application.Services;
using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;

namespace api_stoke.Application.UseCases.Products
{
    public class CreateProductUseCase
    {
        private readonly IProductRepositories _repository;
        private readonly MovementService _movementService;
        private readonly ProductService _service;
        private readonly SkuProductService _skuProductService;

        public CreateProductUseCase(
            IProductRepositories repository,
            MovementService movementService,
            ProductService service,
            SkuProductService skuProductService)
        {
            _repository = repository;
            _movementService = movementService;
            _service = service;
            _skuProductService = skuProductService;
        }

        public async Task<ResponseProductDTO> Execute(Guid userId, CreateProductDTO dto)
        {
            //Product? skuExisting = await _repository.GetSku(dto.Sku);

            //if (!(skuExisting!.Equals(null)))
            //    throw new Exception("Sku existente !");

            Product product = new Product(dto.Name, dto.Description, dto.Price, 1, userId: userId);
            var res = await _repository.Create(product);

            //var movement = await _movementService.RegisterMovement(product.Id, "Produto Criado", true);
            //await _service.InputProductMovement(res.Id, movement.Amount);

            await _skuProductService.AddSkusOfProduct(res.Id, res.Name, dto.Amount);

            return ResponseProductDTO.fromDomain(res, true);
        }
    }

}
