using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;

namespace api_stoke.Application.Services
{
    public class PanelService
    {
        private readonly IPanelRepositories _repository;

        public PanelService(IPanelRepositories repository)
        {
            _repository = repository;
        }

        public async Task<Panel> GetPanel(Guid userId)
        {
            var panel = await _repository.Get(userId) ??
                await _repository.Create(new Panel(userId));
            return panel;
        }
    }
}
