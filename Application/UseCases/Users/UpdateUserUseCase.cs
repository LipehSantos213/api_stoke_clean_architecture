using api_stoke.Application.DTOs.Users;
using api_stoke.Application.Services;
using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;

namespace api_stoke.Application.UseCases.Users
{
    public class UpdateUserUseCase
    {
        private readonly IUserRepositories _repository;
        private readonly UserService _service;

        public UpdateUserUseCase(
            IUserRepositories repository,
            UserService service)
        {
            _repository = repository;
            _service = service;
        }

        public async Task<ResponseUserDTO> Execute(Guid userId, UpdateUserDTO dto)
        {
            var user = await _service.FindUser(userId);

            user.Update(dto.Name, dto.Email);

            var res = await _repository.Update(user);

            return ResponseUserDTO.fromDomain(res);
        }
    }
}
