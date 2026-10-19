using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;
using api_stoke.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace api_stoke.Infrastructure.Repositories
{
    public class CartRepositories : ICartRepositories
    {
        private readonly AppDbContext _appContext;

        public CartRepositories(AppDbContext appContext)
        {
            _appContext = appContext;
        }

        public async Task<Cart> Create(Cart cart)
        {
            _appContext.Carts.Add(cart);
            await _appContext.SaveChangesAsync();

            return ( await GetCartOfUser(cart.UserId) )!;
        }

        public async Task<Cart?> GetCartOfUser(Guid userId)
        {
            return await 
                _appContext.Carts
                .FirstOrDefaultAsync(c=> c.UserId == userId); 
        }

        public async Task Update(Cart cart)
        {
            await _appContext.Carts.Where(c => c.Id == cart.Id).ExecuteUpdateAsync(
                c=> c.SetProperty(c=> c.Amount, cart.Amount)
                    .SetProperty(c=> c.PriceTotal, cart.PriceTotal)
                    .SetProperty(c=> c.UpdatedAt, cart.UpdatedAt)
                );
        }
    }
}
