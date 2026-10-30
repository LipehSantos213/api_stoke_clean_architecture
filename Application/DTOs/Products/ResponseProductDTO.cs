using api_stoke.Application.DTOs.SkuProducts;
using api_stoke.Application.DTOs.Users;
using api_stoke.Domain.Entities;

namespace api_stoke.Application.DTOs.Products
{
    public class ResponseProductDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }

        public ResponseUserDTO? User { get; set; } = new();
        public List<ResponseSkuProductDTO>? Skus { get; set; } = new()!;

        public static ResponseProductDTO fromDomain(Product product, User? user = null,
            List<ResponseSkuProductDTO>? skus = null)
        {
            return new ResponseProductDTO
            {
                Id = product.Id,
                UserId = product.UserId,
                Name = product.Name,
                Price = product.Price,
                Amount = product.Amount,
                Description = product.Description,
                Active = product.Active,
                CreatedAt = product.CreatedAt,
                User = user != null ? ResponseUserDTO.fromDomain(user!):null,
                Skus = skus != null? skus:null
            };
        }
    }
}
