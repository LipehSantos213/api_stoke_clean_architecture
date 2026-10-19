using api_stoke.Application.DTOs.Products;
using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;

namespace api_stoke.Application.UseCases.Products
{
    public class ListProductUseCase
    {
        public readonly IProductRepositories _repository;

        public ListProductUseCase(IProductRepositories repository)
        {
            _repository = repository;
        }

        private void AddProducts(List<ResponseProductDTO> products, List<Product> productsAdd)
        {
            foreach(var e in productsAdd)
            {
                var p = ResponseProductDTO.fromDomain(e, false);
                if(!products.Contains(p))
                    products.Add(p);
            }
        }

        public async Task<List<ResponseProductDTO>> Execute(Guid userId, string? name, bool? active, int? valueStokeMin)
        {
            List<ResponseProductDTO> products = new();

            if (!(name!.Equals(null)))
            {
                var prod = await _repository.GetName(userId, name.Trim());
                if(prod != null)
                    AddProducts(products, prod);
            }
            if (!(active!.Equals(null)))
            {
                var prod = await _repository.GetStatus(userId, (bool)active);

                if(prod != null)
                    AddProducts(products, prod);
            }
            if (!(valueStokeMin.Equals(null)))
            {
                var prod = await _repository.GetStokeMin(userId, (int)valueStokeMin);

                if(prod != null)
                    AddProducts(products, prod);
            }

            return products;
        }
    }
}
