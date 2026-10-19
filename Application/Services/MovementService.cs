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
    }
}
