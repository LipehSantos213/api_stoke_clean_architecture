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

        public async Task<Movement> RegisterMovement(Guid productId, string observation, bool entry, int amount = 1)
        {
            var typeMovement = entry ? "ENTRADA" : "SAIDA";
            Movement movement = new Movement(
                productId,
                typeMovement,
                amount,
                observation,
                "USUARIO");

            var res = await _repository.Create(movement);
            return res;
        }
    }
}
