using api_stoke.Application.Services;
using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;

namespace api_stoke.Application.UseCases.Panels
{
    public class ListPanelUseCase
    {
        private readonly IPanelRepositories _repository;
        private readonly PanelService _service;

        public ListPanelUseCase(
            IPanelRepositories repository,
            PanelService service)
        {
            _repository = repository;
            _service = service;
        }

        public async Task<Panel> Execute(Guid userId)
        {
            var panel = await _service.GetPanel(userId);
            return panel;
        }
    }
}
