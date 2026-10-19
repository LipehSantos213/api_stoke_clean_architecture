using api_stoke.Domain.Entities;

namespace api_stoke.Application.DTOs.SkuProducts
{
    public class ResponseSkuProductDTO
    {
        public Guid Id { get; set; }
        public string Sku { get; set; }
        public string Status { get; set; }


        public static ResponseSkuProductDTO fromDomain(SkuProduct sku)
        {
            return new ResponseSkuProductDTO{
                Id=sku.Id,
                Sku = sku.Sku,
                Status = sku.Status == 0? "Vendido":"Em estoque"
            };
        }
    }
}
