using api_stoke.Application.DTOs.Users;
using api_stoke.Application.Services;
using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;

namespace api_stoke.Application.UseCases.Users
{
    public class LoginUserUseCase
    {
        private readonly UserService _service;

        public LoginUserUseCase(
            UserService service)
        {
            _service = service;
        }

        public async Task<ResponseUserDTO> Execute(LoginUserDTO dto)
        {
            var user = await _service.FindByEmail(dto.Email);

            if (user.Password != dto.Password)
                throw new Exception("Não autorizado !");

            return ResponseUserDTO.fromDomain(user);
        }
    }
}
