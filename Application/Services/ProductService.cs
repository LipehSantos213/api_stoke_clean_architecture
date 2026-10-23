using api_stoke.Application.DTOs.Movements;
using api_stoke.Application.DTOs.Products;
using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;
using System.Reflection.Metadata.Ecma335;

namespace api_stoke.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepositories _repository;

        public ProductService(IProductRepositories repository)
        {
            _repository = repository;
        }

        public async Task<Product> GetProductById(Guid id)
        {
            Product product = await _repository.Get(id) ?? 
                throw new Exception("Produto não encontrado !");
            return product;
        }

        public async Task<Product> InputProductMovement(Guid productId, int amount)
        {
            var product = await GetProductById(productId);

            product.AddAmount(amount);

            return await _repository.Update(product);
        }

        public async Task<Product> OutputProductMovement(Product product,int amount)
        {
            product.SubtraAmount(amount);

            return await _repository.Update(product);
        }

        private List<ResponseMovementDTO> ResponseMovements(List<Movement> movements)
        {
            List<ResponseMovementDTO> response = new();
            movements.ForEach(m => response.Add(ResponseMovementDTO.fromDomain(m)));
            return response;
        }

        public async Task<List<ResponseMovementDTO>> ListInputMovement(Guid productId)
        {
            var movements = (await _repository.GetInputMovement(productId)) ??
                throw new Exception("Movimentações do produto não encotrada !");

            var response = ResponseMovements(movements!);

            return response;
        }

        public async Task<List<ResponseMovementDTO>> ListOutputMovement(Guid productId)
        {
            var movements = await _repository.GetOutputMovement(productId) ??
                throw new Exception("Movimentações do produto não encontrada !");

            var response = ResponseMovements(movements);
            
            return response;
        }

        public async Task<List<ResponseMovementDTO>> ListAllMovement(Guid productId)
        {
            var movements = await _repository.GetAllMovement(productId) ??
                throw new Exception("Movimentações do produto não encontrada !");

            var response = ResponseMovements(movements);

            return response;
        }
    }
}
