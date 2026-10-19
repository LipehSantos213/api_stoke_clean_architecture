using api_stoke.Application.DTOs.Products;
using api_stoke.Application.Exceptions;
using api_stoke.Application.Services;
using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;

namespace api_stoke.Application.UseCases.Products
{
    public class BuyProductUseCase
    {
        private readonly ProductService _service;
        private readonly PanelService _panelService;
        private readonly UserService _userService;
        //private readonly MovementService _movementService;
        private readonly IPanelRepositories _panelRepository;
        private readonly IRegistrationClientsRepositories _rcRepository;
        private readonly CartService _cartService;

        private readonly SkuProductService _skuProductService;

        public BuyProductUseCase(
            ProductService service,
            PanelService panelService,
            UserService userService,
            //MovementService movementService,
            IPanelRepositories panelRepository,
            IRegistrationClientsRepositories rcRepository,
            CartService cartService,
            SkuProductService skuProductService)
        {
            _service = service;
            _panelService = panelService;
            _userService = userService;
            //_movementService = movementService;
            _panelRepository = panelRepository;
            _rcRepository = rcRepository;
            _cartService = cartService;

            _skuProductService = skuProductService;
        }

        public async Task<ResponseProductDTO> Execute(Guid userId, Guid productId, BuyProductDTO dto)
        {
            if (dto.Amount != dto.Skus.Count)
                throw new UnprocessableException(
                    "Quantidade de Skus não coincidem com a quantidade de produtos !");

            // Busca o client no banco
            User client = await _userService.FindUser(userId);

            // Busca o produto a ser vendido e verifica se não pertence ao Client
            Product p = await _service.GetProductById(productId);
            if (p.UserId.Equals(userId))
                throw new UnauthorizedException("Não autorizado !");
            
            // Busca o Carrinho do cliente
            Cart cartClient = await _cartService.Get(userId);

            //  Busca o item do produto no carrinho do cliente
            CartItem cartItem = await _cartService.GetUniqueCartItem(cartClient.Id, p.Id);

            // Retira a quantidade do produto vendido no banco
            Product product = await _service.OutputProductMovement(p, dto.Amount); // Chama o Update para atualizar no banco !!!

            // Busca os registros no banco de cada Skus do produto
            List<SkuProduct> skus = await _skuProductService.ListSkuProductWithString(product.Id, dto.Skus);

            // Registrando os Skus do produto como vendido e vincular elas à uma Movimentação
            Movement movement = await _skuProductService.BuyProduct(product, client, dto.Amount, skus);

            // Atualizar dados no Carrinho do client
            await _cartService.RemoveProduct(cartClient, skus, cartItem);

            // Lista os CartItens e os produtos que cada um tem com os produtos que foram comprados agora
            var skuProductWihCartItens = await _skuProductService.ListSkuProductsWithCartItens(skus);
            
            // Romove os produtos dos CartItens que tem relação aos produtos que foram comprados
            await _cartService.RemoveProductsInCart(skuProductWihCartItens);

            // Pega o Panel do vendedor
            Panel panelSeller = await _panelService.GetPanel(product.UserId);

            // Criar instancia no Registro de Vendas
            RegistrationClients rc = new RegistrationClients(panelSeller.Id, movement.Id, cartClient.Id);

            // Registrar no banco
            await _rcRepository.Create(rc);

            // Atualizar dados do Panel 
            panelSeller.UpdateData(product.Price, dto.Amount);

            // Atualizar o Panel do vendedor com os novos dados
            await _panelRepository.Update(panelSeller);

            return ResponseProductDTO.fromDomain(product, true);
        }
    }
}
