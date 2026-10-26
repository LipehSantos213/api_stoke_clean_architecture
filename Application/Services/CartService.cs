using api_stoke.Application.DTOs.CartItens;
using api_stoke.Application.Exceptions;
using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;

namespace api_stoke.Application.Services
{
    public class CartService
    {
        private readonly ICartRepositories _repository;
        private readonly ICartItemRepositories _cartItemRepository;
        private readonly ISkuCartItemRepositories _skuCartItemRepository;
        private readonly SkuProductService _skuProductService;

        public CartService(ICartRepositories repository, 
            ICartItemRepositories cartItemRepository, ISkuCartItemRepositories skuCartItemRepository,
            SkuProductService skuProductService)
        {
            _repository = repository;
            _cartItemRepository = cartItemRepository;
            _skuCartItemRepository = skuCartItemRepository;
            _skuProductService = skuProductService;
        }

        private async Task<Cart> Create(Guid userId)
        {
            var cart = new Cart(userId);

            var res = await _repository.Create(cart);

            return res;
        }

        public async Task<Cart> Get(Guid userId)
        {
            var cart = await _repository.GetCartOfUser(userId);
            return cart ?? await Create(userId);
        }

        public async Task<CartItem> GetUniqueCartItem(Guid cartId, Guid productId)
        {
            return await _cartItemRepository.GetUnique(cartId, productId)
                ?? await _cartItemRepository.Create(new(cartId, productId: productId));
        }


        private async Task<List<SkuProduct>> ListSkuProductOfCartItem(Guid cartItemId)
        {
            List<SkuCartItem> skuCI = (await _skuCartItemRepository.GetWithCartItemId(cartItemId))!;
            List<SkuProduct> skuProducts = new();
            skuCI.ForEach(async s => skuProducts.Add(await _skuProductService.GetSku(s.SkuId)));
            return skuProducts;
        }

        public async Task<List<ResponseCartItemDTO>> ListItensInCart(Guid userId)
        {
            var cart = await Get(userId);
            List<ResponseCartItemDTO> response = new();
            List<CartItem> ci = await _cartItemRepository.GetAll(cart.Id)
                ?? throw new NotFoundException("Nenhum item do carrinho encontrado !");
            

            foreach(CartItem cartItem in ci)
            {
                List<SkuProduct> skuProducts = await ListSkuProductOfCartItem(cartItem.Id);
                var res = ResponseCartItemDTO.fromDomain(cartItem, true, skuProducts);
                response.Add(res);
            }

            return response;
        }

        /// <summary>
        /// Adiciona os produtos no carrinho
        /// </summary>
        /// <param name="cartItem">Vazio: cria um novo item no carrinho para o produto</param>
        public async Task AddProduct(Product product, Cart cart, List<SkuProduct> skus, CartItem cartItem)
        {
            CartItem ci = cartItem;
            int subtration = 0; // Variavel de controle para quantidade de produtos não aceitos
            foreach(SkuProduct sku in skus)
            {
                // Verifica se o produto pertence ao Item desse carrinho
                if (ci.ProductId != sku.ProductId)
                {
                    subtration++;
                    continue; // pula para o proximo item
                }
                Guid id = sku.Id;
                // Criar instancia da relação do Sku do produto e o item do carrinho
                SkuCartItem skuCI = new(ci.Id, id);

                // Registra-o no banco
                await _skuCartItemRepository.Create(skuCI);
            }

            ci.AddProduct(skus.Count - subtration);

            CartItem newCI = await _cartItemRepository.Update(ci);
            cart.AddProduct(newCI.Price, newCI.Amount);
            await _repository.Update(cart);
        }

        /// <summary>
        /// Não remove o produto do CartItem, mas deixa-o desativado => Historico
        /// </summary>
        /// <exception cref="NotFoundException"></exception>
        public async Task RemoveProduct(Cart cart, List<SkuProduct> skus, CartItem cartItem)
        {
            int subtration = 0; // variavel de controle para os Skus não aceitos
            foreach(SkuProduct sku in skus)
            {
                // Buscando a relação entre o item do carrinho e o Sku do produto no banco
                SkuCartItem? skuCI = await _skuCartItemRepository.GetUnique(cartItem.Id, sku.Id);

                // Se não houver valor ou se tiver e for desativado passa para o proximo item
                if(skuCI == null || skuCI != null && (!skuCI.Active))
                {
                    subtration++; 
                    continue; // proximo item
                }

                // Atualizar o status para desativado
                skuCI!.UpdateStatus(false);
                // E atualizar no banco
                await _skuCartItemRepository.Update(skuCI);
            }

            int amount = skus.Count - subtration;

            // Atualiza os dados no item do carrinho
            cartItem.RemovProduct(amount);
            if (cartItem.Amount == 0)
                cartItem.UpdateStatus(false);

            // Atualiza no banco
            CartItem newCI = await _cartItemRepository.Update(cartItem);

            cart.RemoveProduct(newCI.Price, amount);
            await _repository.Update(cart);
        }
    
        public async Task RemoveProductsInCart(Dictionary<CartItem, List<SkuProduct>> sKpairs)
        {
            foreach(var i in sKpairs)
            {
                CartItem currentCartItem = i.Key;
                List<SkuProduct> currentSkuProducts = i.Value;
                Cart currentCart = (await _repository.Get(currentCartItem.CartId))!;

                await RemoveProduct(currentCart, currentSkuProducts, currentCartItem);
            }
        }
    }
}
