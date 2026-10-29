using api_stoke.Application.DTOs.Products;
using api_stoke.Application.UseCases.Products;
using Microsoft.AspNetCore.Mvc;

namespace api_stoke.Presentation.Controllers
{
    [ApiController]
    [Route("api/users/{userId:guid}/products")]
    public class ProductsControllers : ControllerBase
    {
        private readonly CreateProductUseCase _createProductUseCase;
        private readonly ListProductUseCase _listProductUseCase;
        private readonly QueryProductUseCase _queryProductUseCase;
        private readonly UpdateProductUseCase _updateProductUseCase;
        private readonly BuyProductUseCase _buyProductUseCase;

        public ProductsControllers(
            CreateProductUseCase createProductUseCase,
            ListProductUseCase listProductUseCase,
            QueryProductUseCase queryProductUseCase,
            UpdateProductUseCase updateProductUseCase,
            BuyProductUseCase buyProductUseCase) 
        {
            _createProductUseCase = createProductUseCase;
            _listProductUseCase = listProductUseCase;
            _queryProductUseCase = queryProductUseCase;
            _updateProductUseCase = updateProductUseCase;
            _buyProductUseCase = buyProductUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromRoute] Guid userId, [FromBody] CreateProductDTO dto)
        {
            var product = await _createProductUseCase.Execute(userId, dto);
            return Ok(product);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid userId, [FromRoute] Guid id)
        {
            var product = await _queryProductUseCase.Execute(userId, id);
            return Ok(product);
        }

        [HttpGet]
        public async Task<IActionResult> List(
            [FromRoute] Guid userId,
            [FromQuery] string? name,
            [FromQuery] bool? active,
            [FromQuery] int? valueStokeMin)
        {
            List<ResponseProductDTO> products = await _listProductUseCase.Execute(userId, name, active, valueStokeMin);
            return Ok(products);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromBody] UpdateProductDTO dto, [FromRoute] Guid userId,[FromRoute] Guid id)
        {
            var product = await _updateProductUseCase.Execute(userId, id, dto);
            return Ok(product);
        }

        [HttpPost("{id:guid}/buy")]
        public async Task<IActionResult> BuyProduct([FromBody] BuyProductDTO dto, [FromRoute] Guid userId, [FromRoute] Guid id)
        {
            var res = await _buyProductUseCase.Execute(userId, id, dto);
            return Ok(res);
        }
    }
}
