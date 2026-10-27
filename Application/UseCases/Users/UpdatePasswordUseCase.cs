using api_stoke.Application.DTOs.Users;
using api_stoke.Application.Exceptions;
using api_stoke.Application.Services;
using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;

namespace api_stoke.Application.UseCases.Users
{
    public class UpdatePasswordUseCase
    {
        private readonly UserService _userService;
        private readonly IUserRepositories _repository;

        public UpdatePasswordUseCase(
            UserService userService,
            IUserRepositories repository)
        {
            _userService = userService;
            _repository = repository;
        }

        public async Task Execute(Guid userId, UpdatePasswordDTO dto)
        {
            User user = await _userService.FindUser(userId);
            if (user.Password != dto.CurrentPassword)
                throw new UnauthorizedException("Não autorizado !");
            var updated = user.UpdatePassword(dto.NewPassword.Trim());
            if (!updated)
                throw new UnprocessableException("Nova senha invalida !");
            await _repository.Update(user);
        }
    }
}
