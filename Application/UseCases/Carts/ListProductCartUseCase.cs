using api_stoke.Application.DTOs.CartItens;
using api_stoke.Application.Services;

namespace api_stoke.Application.UseCases.Carts
{
    public class ListProductCartUseCase
    {
        private readonly CartService _service;
        private readonly UserService _userService;

        public ListProductCartUseCase(
            UserService userService,
            CartService service)
        {
            _userService = userService;
            _service = service;
        }

        public async Task<List<ResponseCartItemDTO>> Execute(Guid userId)
        {
            // Verificar se o usuario existe
            await _userService.FindUser(userId);

            // Buscar itens do carrinho
            List<ResponseCartItemDTO> itens = await _service.ListItensInCart(userId);

            return itens;
        }
    }
}
