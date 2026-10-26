namespace api_stoke.Domain.Entities
{
    public class Panel
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }

        public int PriceTotal { get; private set; } // Preço total vendido
        public int Amount { get; private set; } // Numero de vendas

        public int Sales { get; private set; } // 

        public User User { get; private set; } = null!; // Usuario desse Panel

        public List<RegistrationClients>? RegistrationClients { get; } = new();


        private Panel() { }

        public Panel(Guid userId,
            int priceTotal = default!,
            int amount = default!,
            int sales = default!,
            Guid id = default!,
            User user = default!) 
        {
            UserId = userId;
            PriceTotal = priceTotal;
            Amount = amount;
            Sales = sales;
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            User = user;
        }

        public void UpdateData(int priceProduct, int amount) 
        {
            var priceTotal = priceProduct * amount;

            PriceTotal += priceTotal;
            Amount += amount;
            Sales += 1;
        }
    }
}
