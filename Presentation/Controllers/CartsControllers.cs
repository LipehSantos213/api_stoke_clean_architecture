using api_stoke.Application.DTOs.Cart;
using api_stoke.Application.UseCases.Carts;
using Microsoft.AspNetCore.Mvc;

namespace api_stoke.Presentation.Controllers
{
    [ApiController]
    [Route("api/users/{userId:guid}/carts")]
    public class CartsControllers : ControllerBase
    {
        private readonly AddProductCartUseCase _addProductCartUseCase;
        private readonly ListProductCartUseCase _listProductCartUseCase;
        private readonly RemoveProductCartUseCase _removeProductCartUseCase;

        public CartsControllers(
            AddProductCartUseCase addProductCartUseCase,
            ListProductCartUseCase listProductCartUseCase, 
            RemoveProductCartUseCase removeProductCartUseCase)
        {
            _addProductCartUseCase = addProductCartUseCase;
            _listProductCartUseCase = listProductCartUseCase;
            _removeProductCartUseCase = removeProductCartUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromRoute] Guid userId)
        {
            var productsInCart = await _listProductCartUseCase.Execute(userId);
            return Ok(productsInCart);
        }

        [HttpPost("products")]
        public async Task<IActionResult> Create([FromRoute] Guid userId, [FromBody] AddProductCartDTO dto)
        {
            await _addProductCartUseCase.Execute(userId, dto);
            return Ok("Produto adicionado !");
        }

        [HttpDelete("products")]
        public async Task<IActionResult> Delete(
            [FromRoute] Guid userId, 
            [FromBody] AddProductCartDTO dto)
        {
            await _removeProductCartUseCase.Execute(userId, dto);
            return Ok("Product removido do carrinho !");
        }
    }
}
