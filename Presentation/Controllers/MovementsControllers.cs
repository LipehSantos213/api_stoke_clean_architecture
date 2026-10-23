using api_stoke.Application.DTOs.Movements;
using api_stoke.Application.UseCases.Movements;
using Microsoft.AspNetCore.Mvc;

namespace api_stoke.Presentation.Controllers
{
    [ApiController]
    [Route("api/users/{userId:guid}/products/{id:guid}/movements")]
    public class MovementsControllers : ControllerBase
    {
        private readonly InputMovementUseCase _inputMovementUseCase;
        private readonly ListMovementUseCase _listMovementUseCase;
        private readonly OutputMovementUseCase _outputMovementUseCase;

        public MovementsControllers(
            InputMovementUseCase inputMovementUseCase,
            ListMovementUseCase listMovementUseCase,
            OutputMovementUseCase outputMovementUseCase)
        {
            _inputMovementUseCase = inputMovementUseCase;
            _listMovementUseCase = listMovementUseCase;
            _outputMovementUseCase = outputMovementUseCase;
        }

        [HttpPost("input")]
        public async Task<IActionResult> Input([FromRoute] Guid userId, [FromRoute] Guid id, [FromBody] RegisterMovementDTO dto)
        {
            var res = await _inputMovementUseCase.Execute(userId, id, dto);
            return Ok(res);
        }

        [HttpPost("output")]
        public async Task<IActionResult> Output([FromRoute] Guid userId, [FromRoute] Guid id, [FromBody] RegisterMovementDTO dto)
        {
            ResponseMovementDTO res = await _outputMovementUseCase.Execute(userId, id, dto);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> List([FromRoute] Guid userId, [FromRoute] Guid id, [FromQuery] string? movement)
        {
            var res = await _listMovementUseCase.Execute(userId,id, movement);
            return Ok(new { res.Item1, res.Item2 });
        }
    }
}
