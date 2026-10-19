using api_stoke.Application.Exceptions;

namespace api_stoke.Domain.Entities
{
    public class SkuCartItem
    {
        public Guid Id { get; private set; }
        public Guid CartItemId { get; private set; }
        public Guid SkuId { get; private set; }
        public bool Active { get; private set; }

        public CartItem CartItem { get; private set; } = null!;
        public SkuProduct SkuProduct { get; private set; } = null!;

        public SkuCartItem(Guid cartItemId, Guid skuId)
        {
            CartItemId = cartItemId;
            SkuId = skuId;
            Id = Guid.NewGuid();
            Active = true;
        }

        public void UpdateStatus(bool value)
        {
            if(value == Active)
            {
                string status = value ? "ativado" : "desativado";
                throw new ConflictException($"O Item já esta {status}");
            }
            Active = value;
        }
    }
}
