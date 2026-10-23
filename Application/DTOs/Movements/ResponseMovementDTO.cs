using api_stoke.Application.DTOs.Products;
using api_stoke.Application.DTOs.SkuProducts;
using api_stoke.Domain.Entities;

namespace api_stoke.Application.DTOs.Movements
{
    public class ResponseMovementDTO
    {
        public Guid Id { get; set; }
        public string TypeMovement { get; set; }
        public int Amount { get; set; }
        public string Observation { get; set; }
        public string User { get; set; }
        public DateTime CreatedAt { get; set; }

        public ResponseProductDTO? Product { get; set; } = null!;
        public List<ResponseSkuProductDTO>? Skus { get; set; } = null!;

        public Guid? UserSallerId { get; set; }
        public Guid? UserClientId { get; set; }
            
        public static ResponseMovementDTO fromDomain(
                Movement movement,
                ResponseProductDTO product = null!,
                List<ResponseSkuProductDTO> skus = null!,
                Guid? userSallerId = null,
                Guid? userClientId = null)
        {
            return new ResponseMovementDTO
            {
                Id = movement.Id,
                Amount = movement.Amount,
                Observation = movement.Observation,
                TypeMovement = movement.TypeMovement,
                User = movement.User,
                CreatedAt = movement.CreatedAt,
                Product = product,
                Skus = skus,
                UserSallerId = userSallerId,
                UserClientId = userClientId
            };
        }
    }
}
