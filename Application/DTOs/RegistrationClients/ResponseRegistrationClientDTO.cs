using api_stoke.Application.DTOs.Movements;
using api_stoke.Application.DTOs.Users;
using api_stoke.Domain.Entities;

namespace api_stoke.Application.DTOs.RegistrationClients
{
    public class ResponseRegistrationClientDTO
    {
        public Guid Id { get; set; }
        public ResponseMovementDTO Movement = new();
        public ResponseUserDTO UserSaller = new();
        public ResponseUserDTO UserClient = new();

        public static ResponseRegistrationClientDTO fromDomain(Guid idRegistrationClient, Movement movement, User client, User saller)
        {
            return new ResponseRegistrationClientDTO
            {
                Id = idRegistrationClient,

                Movement = ResponseMovementDTO.fromDomain(movement, false),
                UserSaller = ResponseUserDTO.fromDomain(saller),
                UserClient = ResponseUserDTO.fromDomain(client)
            };
        }
    }
}
