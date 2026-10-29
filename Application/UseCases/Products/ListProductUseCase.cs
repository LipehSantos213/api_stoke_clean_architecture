using api_stoke.Application.DTOs.Products;
using api_stoke.Application.Exceptions;
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

        private List<ResponseProductDTO> AddProducts(List<ResponseProductDTO> products, List<Product> productsAdd)
        {
            Console.WriteLine("[INFO] Adicionando produtos buscados do banco");
            foreach(Product e in productsAdd)
            {
                ResponseProductDTO p = ResponseProductDTO.fromDomain(e);
                if(!products.Exists(i=> i.Id == p.Id)) 
                {
                    Console.WriteLine($"---- [INFO] Produto '{p.Id}' adicionado !");
                    products.Add(p);
                }
            }
            return products;
        }

        public async Task<List<ResponseProductDTO>> Execute(Guid userId, string? name, bool? active, int? valueStokeMin)
        {
            Console.WriteLine("[INFO] ListProductUseCase");
            List<ResponseProductDTO> products = new();

            List<Product>? allProducts = await _repository.GetProductsOfUser(userId, name, active, valueStokeMin);
            if (allProducts != null && allProducts.Count > 0)
            {
                products = AddProducts(products, allProducts);
            }
            else
            {
                throw new NotFoundException("Nenhum produto encontrado !");
            }
          
            return products;
        }
    }
}
