using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;
using api_stoke.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace api_stoke.Infrastructure.Repositories
{
    public class CartItemRepositories : ICartItemRepositories
    {
        private readonly AppDbContext _appContext;

        public CartItemRepositories(AppDbContext appContext)
        {
            _appContext = appContext;
        }

        public async Task<CartItem> Create(CartItem item)
        {
            _appContext.CartItens.Add(item);
            await _appContext.SaveChangesAsync();
            return ( await Get(item.Id) )!;
        }

        public async Task<CartItem> Update(CartItem item)
        {
            await _appContext.CartItens.Where(ci => ci.Id == item.Id).ExecuteUpdateAsync(ci=>
                ci.SetProperty(i=> i.Amount, item.Amount)
                .SetProperty(i=>i.Price, item.Price)
                .SetProperty(i=>i.PriceTotal, item.PriceTotal)
                .SetProperty(i=>i.UpdatedAt, item.UpdatedAt)
                .SetProperty(i=>i.Active, item.Active));
            return (await Get(item.Id))!;
        }

        public async Task<CartItem?> Get(Guid id)
        {
            return await
                _appContext.CartItens
                .FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<CartItem?> GetUnique(Guid cartId, Guid productId)
        {
            return await
                _appContext.CartItens
                .FirstOrDefaultAsync(item=>
                    item.CartId == cartId && 
                    item.ProductId == productId
                );
        }

        public async Task<List<CartItem>?> GetAll(Guid cartId)
        {
            var cartItens = await _appContext.CartItens
                .Where(item => item.CartId == cartId)
                .ToListAsync();
            return cartItens;
        }

        public async Task<CartItem> Remove(Guid id)
        {
            var item = (await Get(id))!;
            //_appContext.CartItens.Remove(item);
            //await _appContext.SaveChangesAsync();
            await _appContext.CartItens
                .Where(i => i.Id == id)
                .ExecuteDeleteAsync();

            return item;
        }
    }
}