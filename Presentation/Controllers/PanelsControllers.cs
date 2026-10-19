using api_stoke.Application.UseCases.Panels;
using Microsoft.AspNetCore.Mvc;

namespace api_stoke.Presentation.Controllers
{
    [ApiController]
    [Route("api/users/{id:guid}/panels")]
    public class PanelsControllers : ControllerBase
    {
        private readonly ListPanelUseCase _listPanelUseCase;
        public PanelsControllers(ListPanelUseCase listPanelUseCase)
        {
            _listPanelUseCase = listPanelUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromRoute] Guid id)
        {
            var res = await _listPanelUseCase.Execute(id);
            return Ok(res);
        }
    }
}
