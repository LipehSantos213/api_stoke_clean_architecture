using api_stoke.Application.DTOs;
using api_stoke.Application.DTOs.Users;
using api_stoke.Application.UseCases.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace api_stoke.Presentation.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersControllers : ControllerBase
    {
        private readonly CreateUserUseCase _createUserUseCase;
        private readonly LoginUserUseCase _loginUserUseCase;
        private readonly UpdateUserUseCase _updateUserUseCase;
        private readonly GetUserUseCase _getUserUseCase;
        private readonly UpdatePasswordUseCase _updatePasswodUseCase;
        public UsersControllers(
            CreateUserUseCase createUserUseCase,
            LoginUserUseCase loginUserUseCase,
            UpdateUserUseCase updateUserUseCase,
            GetUserUseCase getUserUseCase,
            UpdatePasswordUseCase updatePasswodUseCase)
        {
            _createUserUseCase = createUserUseCase;
            _loginUserUseCase = loginUserUseCase;
            _updateUserUseCase = updateUserUseCase;
            _getUserUseCase = getUserUseCase;
            _updatePasswodUseCase = updatePasswodUseCase;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO dto)
        {
            var user = await _createUserUseCase.Execute(dto);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO dto)
        {
            var res = await _loginUserUseCase.Execute(dto);
            return Ok(res);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Me([FromRoute] Guid id)
        {
            var res = await _getUserUseCase.Execute(id);
            return Ok(res);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserDTO dto)
        {
            var res = await _updateUserUseCase.Execute(id, dto);
            return Ok(res);
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> UpdatePassword([FromRoute] Guid id, [FromBody] UpdatePasswordDTO dto)
        {
            await _updatePasswodUseCase.Execute(id, dto);
            return Ok(
                ResponseWithMessageDTO.setMessage("Senha atualizada !")
            );
        }
    }
}
