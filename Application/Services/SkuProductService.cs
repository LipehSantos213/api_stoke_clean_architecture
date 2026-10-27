using api_stoke.Application.Exceptions;
using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Runtime.CompilerServices;
using System.Security;

namespace api_stoke.Application.Services
{
    public class SkuProductService
    {
        private readonly ISkuProductRepositories _repository;
        private readonly IMovementRepositories _movementRepository;
        private readonly ISkuMovementRepositories _skuMovementRepository;
        private readonly ISkuCartItemRepositories _skuCartItemRepository;
        private readonly ICartItemRepositories _cartItemRepository;
        private readonly IProductRepositories _productRepository;

        public SkuProductService(
            ISkuProductRepositories repository,
            IMovementRepositories movementRepository,
            ISkuMovementRepositories skuMovementRepository,
            ISkuCartItemRepositories skuCartItemRepository,
            ICartItemRepositories cartItemRepository,
            IProductRepositories productRepository)
        {
            _repository = repository;
            _movementRepository = movementRepository;
            _skuMovementRepository = skuMovementRepository;
            _skuCartItemRepository = skuCartItemRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
        }

        public async Task<SkuProduct> GetSku(Guid skuId)
        {
            SkuProduct sku = await _repository.GetById(skuId) 
                ?? throw new NotFoundException("Sku do producto não encontrado");
            return sku;
        }

        /// <summary>
        /// Faz uma busca e retorna os dados de cada Sku do produto
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="skus"></param>
        /// <returns>Retorna os Sku que pertecem ao produto e que não são vendidos</returns>
        public async Task<List<SkuProduct>> ListSkuProductWithString(Guid productId, List<string> skus)
        {
            List<SkuProduct> skuProducts = new();
            foreach(string sku in skus)
            {
                SkuProduct? currentSku = await _repository.GetBySku(sku);
                if(currentSku == null 
                    || currentSku != null && 
                        (currentSku.ProductId != productId 
                            || currentSku.Status == SkuProduct.StatusSku.EM_ESTOQUE)) 
                {
                    continue;
                }

                skuProducts.Add(currentSku!);
            }

            return skuProducts;
        }

        private async Task<string> VerifySkuStringInDatabase(string checkSku, string nameProduct)
        {
            string sku = checkSku;

            for (int i = 0; i < 10; i++)
            {
                SkuProduct? existsSku = await _repository.GetBySku(sku);

                if (existsSku == null)
                    return sku;

                sku = SkuProduct.ToGenerateSku(sku, nameProduct);
            }

            throw new ConflictException(
                "Não foi possível gerar um SKU disponível.");
        }

        public async Task<Movement> AddSkusOfProduct(Guid productId, string nameProduct, int amount, string observMovement = null!)
        {
            // Criar um instancia do movimentação do produto
            string observation = observMovement == null? $"{amount} unidade de {nameProduct} adicionadas !!!" : observMovement;
            Movement movement = new("ENTRADA", amount, observation, "USUARIO");

            // Criar movimentação no banco de dados
            await _movementRepository.Create(movement);

            // Laço para adicionar cada Sku no banco e relacionar
            // cada um à movimentação criada
            for (int i = 0; i < amount; i++)
            {
                // Buscar o ultimo Sku do produto adicionado no banco
                SkuProduct? lastSku = await _repository.GetLastSku(productId);

                // Verificação: se o produto não tiver sku, chama metodo para criar 
                // o primeiro Sku do produto, se ele tiver, cria uma nova Sku com base no
                // ultimo Sku registrado no banco
                string sku = lastSku == null ? 
                    // criar primeiro sku
                    SkuProduct.FirstSku(nameProduct): 
                    // Criar Sku com base no ultimo sku do produto
                    SkuProduct.ToGenerateSku(lastSku.Sku, nameProduct);

                // Verificação antes de registrar o SKU no banco
                string securitySku = await VerifySkuStringInDatabase(sku, nameProduct);

                SkuProduct skuObj;
                while (true)
                {
                    // Criar instancia do Sku do produto
                    skuObj = new SkuProduct(productId, securitySku);
                    try
                    {
                        // Criar Sku do produto no banco
                        await _repository.Create(skuObj);
                    }
                    catch (DbUpdateException ex)
                        when (ex.InnerException is PostgresException pgEx 
                        && pgEx.SqlState == "23505" && pgEx.ConstraintName == "IX_sku_products_sku")
                    {
                        securitySku = SkuProduct.ToGenerateSku(securitySku, nameProduct);

                        securitySku = await VerifySkuStringInDatabase(securitySku, nameProduct);
                    }
                    break;
                }

                // Criar instancia da relação do Sku com a movimentação do produto
                SkuMovement skuMovement = new(skuObj.Id, movement.Id);
                // Criando ela no banco
                await _skuMovementRepository.Create(skuMovement);
            }
            return movement;
        }

        public async Task<Movement> RemoveSkusOfProduct(Product product, List<SkuProduct> skus, string observation)
        {
            Movement movement = new("SAIDA", skus.Count, observation,"USUARIO");
            await _movementRepository.Create(movement);

            int sub = 0;
            foreach(SkuProduct sp in skus)
            {
                try
                {
                    sp.SetStatusSku(false);
                }
                catch (ConflictException)
                {
                    sub++;
                    continue;
                }
                await _repository.Update(sp);
                SkuMovement skuMovem = new(sp.Id, movement.Id);
                await _skuMovementRepository.Create(skuMovem);
            }
            if(sub != 0)
            {
                movement.SetAmount(sub, toAdd:false);
                await _movementRepository.Update(movement);
            }

            product.SubtraAmount(skus.Count - sub);
            await _productRepository.Update(product);
            return movement;
        }

        public async Task<Movement> BuyProduct(Product product, User client, int amount, List<SkuProduct> skus)
        {
            // Criando movimentação de compra do produto
            string observation = $"{client.Name} comprou {amount} {product.Name} do {product.User.Name}";
            Movement movement = new Movement("SAIDA", amount, observation, "USUARIO");
            await _movementRepository.Create(movement);

            //List <(string, string)> messages = new();
            // Laço para atualizar o Sku do produto como VENDIDO e 
            // relaciona-lo à movimentação de compra de produto
            foreach(SkuProduct currentSkuProduct in skus)
            {
                // Validação: se o Sku do produto não está no banco ou se o Sku tiver no banco
                // e não for do produto ou estiver VENDIDO, o laço continua para o proximo Sku
                if (currentSkuProduct == null
                    || (currentSkuProduct != null && 
                        (currentSkuProduct.ProductId != product.Id 
                            || currentSkuProduct.Status == SkuProduct.StatusSku.VENDIDO)))
                {
                    //messages.Add((current, "Não aceito"));
                    continue;
                }

                // Atualiza o status do Sku para VENDIDO
                currentSkuProduct!.UpdateStatus(SkuProduct.StatusSku.VENDIDO);
                await _repository.Update(currentSkuProduct);

                // Cria a relação do Sku atual e Movement no banco
                SkuMovement skuMovement = new SkuMovement(currentSkuProduct.Id, movement.Id);
                await _skuMovementRepository.Create(skuMovement);
            }
            return movement;
        }
    
        public async Task<Dictionary<CartItem, List<SkuProduct>>> ListSkuProductsWithCartItens(List<SkuProduct> skus)
        {
            // Cria dicionario de retorno e a lista de Itens de cada produto
            Dictionary<CartItem, List<SkuProduct>> res = new();
            List<CartItem> CItens = new();

            foreach(SkuProduct sku in skus)
            {
                // Busca os Relacionamento que o produto tem com os Itens do carrinho
                List<SkuCartItem>? skuCartItens = await _skuCartItemRepository.GetWithSkuProductId(sku.Id);

                // Por acaso não tiver nada, passa para o proximo produto
                if(skuCartItens == null)
                    continue;

                // Laço para busca cada Item do carrinho do produto e adiciona-lo a uma lista
                foreach(SkuCartItem currentSkuCI in skuCartItens)
                    CItens.Add((await _cartItemRepository.Get(currentSkuCI.CartItemId))!);

                // Laço para percorrer cada Item do carrinho que foi
                // adicionado pelo laço acima
                foreach (CartItem ci in CItens)
                {
                    // =========================================
                    // ==== OBS: CADA 'ci' percente ao 'sku'====
                    // =========================================

                    // Se o dicionario estiver vazio
                    // é adicionado o primeiro valor nele
                    if (res.Count == 0)
                    {
                        res.Add(ci, [sku]);
                        continue; // Faz o laço ir para o proximo item
                    }

                    List<SkuProduct> v = new();
                    if(res.TryGetValue(ci, out v))
                        v.Add(sku);
                    else
                        res.Append(new(ci, [sku]));
                }
                CItens.Clear(); // Limpa a lista de Itens do carrinho
            }
            return res;
        }
    }
}
