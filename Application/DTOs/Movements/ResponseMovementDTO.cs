using api_stoke.Application.DTOs.Products;
using api_stoke.Domain.Entities;

namespace api_stoke.Application.DTOs.Movements
{
    public class ResponseMovementDTO
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string TypeMovement { get; set; }
        public int Amount { get; set; }
        public string Observation { get; set; }
        public string User { get; set; }
        public DateTime CreatedAt { get; set; }

        public ResponseProductDTO? Product { get; set; }

        public static ResponseMovementDTO fromDomain(Movement movement, bool product)
        {
            return new ResponseMovementDTO
            {
                Id = movement.Id,
                Amount = movement.Amount,
                Observation = movement.Observation,
                ProductId = movement.ProductId,
                TypeMovement = movement.TypeMovement,
                User = movement.User,
                CreatedAt = movement.CreatedAt,
                Product = product ? ResponseProductDTO.fromDomain(movement.Product, false) : null
            };
        }
    }
}
