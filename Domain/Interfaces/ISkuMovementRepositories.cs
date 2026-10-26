using api_stoke.Domain.Entities;

namespace api_stoke.Domain.Interfaces
{
    public interface ISkuMovementRepositories
    {
        Task<SkuMovement> Create(SkuMovement skuMovement);
        Task<SkuMovement?> Get(Guid id);
    }
}
