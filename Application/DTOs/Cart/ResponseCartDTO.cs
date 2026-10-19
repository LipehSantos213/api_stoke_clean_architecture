using api_stoke.Application.DTOs.CartItens;
using api_stoke.Application.DTOs.RegistrationClients;

namespace api_stoke.Application.DTOs.Cart
{
    public class ResponseCartDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int Amount { get; set; }
        public int PriceTotal { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<ResponseRegistrationClientDTO> RegistrationClients = new()!;
        public List<ResponseCartItemDTO> CartItens = new()!;
    }
}
