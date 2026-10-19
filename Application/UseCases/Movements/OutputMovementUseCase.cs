using api_stoke.Application.DTOs.Movements;
using api_stoke.Application.Services;
using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;

namespace api_stoke.Application.UseCases.Movements
{
    public class OutputMovementUseCase
    {
        private readonly IMovementRepositories _repository;
        private readonly ProductService _productService;

        public OutputMovementUseCase(
            IMovementRepositories repository,
            ProductService productService)
        {
            _repository = repository;
            _productService = productService;
        }

        public async Task<(Movement, Product)> Execute(Guid userId, Guid productId, RegisterMovementDTO dto)
        {
            // Busca o produto no banco e verifica se ele não é do usuario
            var p = await _productService.GetProductById(productId);
            if (!(p.UserId.Equals(userId)))
                throw new Exception("Não autorizado !");

            // Cria uma instancia de Movement
            Movement movem = new Movement(
                productId,
                "SAIDA",
                dto.Amount,
                dto.Observation,
                "USUARIO");

            // Registra no banco o Movement
            var movement = await _repository.Create(movem);

            // Retira a quantidade de produto no banco
            var product = await _productService.OutputProductMovement(p, dto.Amount);

            return (movement, product);
        }
    }
}