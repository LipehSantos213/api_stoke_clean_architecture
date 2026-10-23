using api_stoke.Application.DTOs.Movements;
using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;

namespace api_stoke.Application.Services
{
    public class MovementService
    {
        private readonly IMovementRepositories _repository;

        public MovementService(IMovementRepositories repository)
        {
            _repository = repository;
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
