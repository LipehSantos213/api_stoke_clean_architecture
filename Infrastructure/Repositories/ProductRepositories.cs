using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;
using api_stoke.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace api_stoke.Infrastructure.Repositories
{
    public class ProductRepositories : IProductRepositories
    {
        private readonly AppDbContext _appContext;

        public ProductRepositories(AppDbContext appContext)
        {
            _appContext = appContext;
        }

        public async Task<Product> Create(Product product)
        {
            _appContext.Products.Add(product);
            await _appContext.SaveChangesAsync();
            return (await Get(product.Id))!;
        }

        public async Task<Product?> Get(Guid id)
        {
            return 
                await _appContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> Update(Product product)
        {
            await _appContext.Products.Where(p => p.Id == product.Id).ExecuteUpdateAsync(p=> p
            .SetProperty(p=>p.Name, product.Name)
            .SetProperty(p=>p.Description, product.Description)
            .SetProperty(p=>p.Price, product.Price)
            .SetProperty(p=>p.Amount, product.Amount)
            .SetProperty(p=>p.Active, product.Active)
            .SetProperty(p=>p.UpdatedAt, product.UpdatedAt)
            );
            return ( await Get(product.Id) )!;
        }

        public async Task<List<Product>?> GetName(Guid userId, string name)
        {
            var products = await 
                _appContext.Products
                .Where(p => p.UserId == userId && EF.Functions.Like(p.Name, $"%{name}%")).ToListAsync();
            return products;
        }

        public async Task<List<Product>?> GetStatus(Guid userId, bool value)
        {
            var products = await 
                _appContext.Products
                .Where(p=> p.UserId == userId && p.Active == value).ToListAsync();
            return products;
        }

        public async Task<List<Product>?> GetStokeMin(Guid userId, int stokeMin)
        {
            var products = await 
                _appContext.Products
                .Where(p=> p.UserId == userId && p.Amount <= stokeMin).ToListAsync();
            return products;
        }
    }
}
