using api_stoke.Application.DTOs.Products;
using api_stoke.Application.DTOs.SkuProducts;
using api_stoke.Domain.Entities;

namespace api_stoke.Application.DTOs.CartItens
{
    public class ResponseCartItemDTO
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Amount { get; set; }
        public int Price { get; set; } // Preço do Produto 
        public int PriceTotal { get; set; } // Preço Total (Price * Amount)
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Active { get; set; }

        public List<ResponseSkuProductDTO>? Skus = new()!;

        public ResponseProductDTO? Product=new();


        public static ResponseCartItemDTO fromDomain(CartItem cartItem, bool product, List<SkuProduct> skus = default!)
        {
            List<ResponseSkuProductDTO> s = new();
            if(skus != null)
            {
                skus.ForEach(i => s.Add(ResponseSkuProductDTO.fromDomain(i)));
            }
            else
            {
                s = null!;
            }
            
            return new ResponseCartItemDTO
            {
                Id = cartItem.Id,
                CartId = cartItem.CartId,
                ProductId = cartItem.ProductId,
                Amount = cartItem.Amount,
                Price = cartItem.Price,
                PriceTotal = cartItem.PriceTotal,
                CreatedAt = cartItem.CreatedAt,
                UpdatedAt = cartItem.UpdatedAt,
                Product = product ?
                    ResponseProductDTO.fromDomain(cartItem.Product, false) :
                    null,
                Skus = s
            };
        }
        
    }
}
