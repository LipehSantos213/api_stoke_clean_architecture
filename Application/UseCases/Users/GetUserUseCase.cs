using api_stoke.Application.DTOs.Users;
using api_stoke.Application.Services;

namespace api_stoke.Application.UseCases.Users
{
    public class GetUserUseCase
    {
        private readonly UserService _service;

        public GetUserUseCase(UserService service)
        {
            _service = service;
        }

        public async Task<ResponseUserDTO> Execute(Guid userId)
        {
            var user = await _service.FindUser(userId);
            return ResponseUserDTO.fromDomain(user);
        }
    }
}
