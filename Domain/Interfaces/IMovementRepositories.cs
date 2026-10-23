using api_stoke.Domain.Entities;

namespace api_stoke.Domain.Interfaces
{
    public interface IMovementRepositories
    {
        Task<Movement> Create(Movement movement);

        Task<Movement?> Get(Guid id);

        Task<Movement> Update(Movement movement);

        Task<List<Movement>?> GetInputMovement(Guid productId);
        Task<List<Movement>?> GetOutputMovement(Guid productId);
        Task<List<Movement>?> GetAllMovement(Guid productId);
    }
}
