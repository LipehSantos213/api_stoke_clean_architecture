using api_stoke.Application.Exceptions;

namespace api_stoke.Domain.Entities
{
    public class CartItem
    {
        public Guid Id { get; private set; }
        public Guid CartId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Amount { get; private set; }
        public int Price { get; private set; }
        public int PriceTotal { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public bool Active { get; private set; }

        public Cart Cart { get; private set; } = null!;
        public Product Product { get; private set; } = null!;
        public SkuCartItem SkuCartItem { get; private set; } = null!;

        private CartItem() { }

        public CartItem(Guid cartId, Guid productId = default! ,Guid id = default!,  DateTime createdAt = default!,
            int amount = default!, int price = default!, DateTime updatedAt = default!)
        {
            Validate(price, amount);
            PriceTotal = price * amount;
            CartId = cartId;
            ProductId = productId;
            Amount = amount;
            Price = price;
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            CreatedAt = createdAt == DateTime.MinValue ? DateTime.UtcNow : createdAt;
            UpdatedAt = updatedAt == DateTime.MinValue ? DateTime.UtcNow : updatedAt;
            Active = true;
        }

        private static void Validate(int price, int amount)
        {
            if (price <= 0)
                throw new UnprocessableException("Preço invalido !!!");
            if (amount <= 0)
                throw new UnprocessableException("Quantidade invalida !!!");
        }

        public void UpdateStatus(bool value)
        {
            if(value == Active)
            {
                string status = value ? "ativado" : "desativado";
                throw new ConflictException($"O item já está {status}");
            }

            Active = value;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddProduct(int amount)
        {
            Validate(Price, amount);

            Amount += amount;

            PriceTotal += Price * amount;
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemovProduct(int amount)
        {
            Validate(Price, amount);

            Amount -= amount;
            PriceTotal -= Price * amount;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePrice(int price)
        {
            Validate(price, Amount);

            Price = price;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
