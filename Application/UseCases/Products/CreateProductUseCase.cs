using api_stoke.Application.DTOs.Movements;
using api_stoke.Application.DTOs.Products;
using api_stoke.Application.Exceptions;
using api_stoke.Application.Services;
using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace api_stoke.Application.UseCases.Products
{
    public class CreateProductUseCase
    {
        private readonly IProductRepositories _repository;
        private readonly UserService _userService;
        private readonly MovementService _movementService;
        private readonly ProductService _service;
        private readonly SkuProductService _skuProductService;

        public CreateProductUseCase(
            IProductRepositories repository,
            MovementService movementService,
            ProductService service,
            SkuProductService skuProductService,
            UserService userService)
        {
            _repository = repository;
            _movementService = movementService;
            _service = service;
            _skuProductService = skuProductService;
            _userService = userService;
        }

        public async Task<ResponseProductDTO> Execute(Guid userId, CreateProductDTO dto)
        {
            if (dto.Amount < 1)
                throw new 
                    UnprocessableException("Forneça a quantidade de produtos !");

            User user = await _userService.FindUser(userId);

            Product product = new Product(dto.Name, dto.Description, dto.Price, dto.Amount, userId: userId);
            Product res = await _repository.Create(product);

            await _skuProductService.AddSkusOfProduct(res.Id, res.Name, dto.Amount);

            return ResponseProductDTO.fromDomain(res, user);
        }
    }
}
