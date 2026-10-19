using api_stoke.Application.DTOs.Users;
using api_stoke.Application.Exceptions;
using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;

namespace api_stoke.Application.UseCases.Users
{
    public class CreateUserUseCase
    {
        private readonly IUserRepositories _repository;

        public CreateUserUseCase(IUserRepositories repository)
        {
            _repository = repository;
        }

        public async Task<ResponseUserDTO> Execute(CreateUserDTO dto)
        {
            var emailExisting = await _repository.GetByEmail(dto.Email);
            if (emailExisting != null)
                throw new ConflictException("Email existente !");

            var user = new User(
                dto.Name,
                dto.Email,
                User.UserRole.USUARIO,
                password: dto.Password,
                active:true);

            var res = await _repository.Create(user);

            return ResponseUserDTO.fromDomain(res);
        }
    }
}
