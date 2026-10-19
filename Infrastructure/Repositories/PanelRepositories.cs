using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;
using api_stoke.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace api_stoke.Infrastructure.Repositories
{
    public class PanelRepositories : IPanelRepositories
    {
        private readonly AppDbContext _appContext;

        public PanelRepositories(AppDbContext appContext)
        {
            _appContext = appContext;
        }

        public async Task<Panel> Create(Panel panel)
        {
            _appContext.Panels.Add(panel);
            await _appContext.SaveChangesAsync();
            return ( await  Get(panel.UserId) )!;
        }

        public async Task<Panel?> Get(Guid userId)
        {
            return await
                _appContext.Panels
                .FirstOrDefaultAsync(p=> p.UserId == userId);
        }

        public async Task Update(Panel panel)
        {
            await _appContext.Panels
                .Where(p => p.Id == panel.Id)
                .ExecuteUpdateAsync(p=> p
                    .SetProperty(p=> p.PriceTotal, panel.PriceTotal)
                    .SetProperty(p=> p.Amount, panel.Amount)
                    .SetProperty(p=> p.Sales, panel.Sales)
                    );
        }
    }
}
