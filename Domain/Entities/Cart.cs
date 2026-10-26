using api_stoke.Application.Exceptions;

namespace api_stoke.Domain.Entities
{
    public class Cart
    {
        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }
        public int Amount { get; private set; }
        public int PriceTotal { get; private set; }

        public List<RegistrationClients>? RegistrationClients { get; private set; } = new();
        public List<CartItem>? CartItens { get; private set; } = new();

        public DateTime UpdatedAt { get; private set; }

        public User User { get; } = null!;

        private Cart() { }

        public Cart(Guid userId,
            int amount = default!,
            int priceTotal=default!,
            Guid id = default!,
            DateTime updatedAt = default!,
            User user = default!)
        {
            UserId = userId;
            Amount = amount;
            PriceTotal = priceTotal;
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            UpdatedAt = updatedAt == DateTime.MinValue ? DateTime.UtcNow : updatedAt;
            User = user;
        }

        private static void Validate(int price, int amount)
        {
            if(price <= 0)
            {
                throw new UnprocessableException("Preço invalido !!!");
            }else if(amount <= 0)
            {
                throw new UnprocessableException("Quantidade invalida !!!");
            }
        }

        // Amount = Quantidade que o Cliente que comprar(futuramente ou não)
        // Price = Preço da unidade do produto que o cliente quer comprar(futuramente ou não)
        public void AddProduct(int price, int amount = 1)
        {
            Validate(price, amount);

            Amount += amount;

            var priceTotal = price * amount;

            PriceTotal += priceTotal;
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveProduct(int price, int amount)
        {
            Validate(price, amount);

            var priceTotal = price * amount;

            PriceTotal -= priceTotal;
            Amount -= amount;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
