namespace api_stoke.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Price { get; private set; } // Centavos
        public int Amount { get; private set; } // Quantidade
        public bool Active { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public List<CartItem> CartItens { get; private set; } = new()!;
        public List<SkuProduct> Skus { get; private set; } = new()!;
        public User User { get; private set; } = null!;

        private Product() { }

        public Product(
            string name,
            string description,
            int price,
            int amount,
            Guid id = default!,
            Guid userId=default!,
            DateTime createdAt = default!,
            DateTime updatedAt = default!,
            User user = default!
            )
        {
            Validate(name, description, price);

            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            UserId = userId;
            Name = name;
            Description = description;
            //Sku = sku;
            Price = price;
            Amount = amount;
            CreatedAt = createdAt == DateTime.MinValue ? DateTime.UtcNow : createdAt;
            UpdatedAt = updatedAt == DateTime.MinValue ? DateTime.UtcNow : updatedAt;
            User = user;
        }

        private void Validate(string name, string description,int price)
        {
            if(name.Length >= 80 || name.Length <= 5)
            {
                throw new Exception("Nome invalido !");
            }

            else if(description.Length >= 200 || description.Length <= 5)
            {
                throw new Exception("Descrição invalida !");
            }

            else if(price<= 0)
            {
                throw new Exception("Preço invalido !");
            }

            //else if(sku.Equals(null))
            //{
            //    throw new Exception("SKU invalido !");
            //}

        }
    
        public void UpdateStatus(bool value)
        {
            if(this.Active == value)
            {
                var status = value ? "ativado" : "desaivado";
                throw new Exception($"O produto já está {status}");
            }

            Active = value;
            UpdatedAt = DateTime.UtcNow;
        }
    
        public void Update(string? name, string? description, int? price)
        {
            Validate(name??Name, description??Description, price??Price);

            Name = name??Name;
            Description = description??Description;
            Price = price??Price;
            UpdatedAt = DateTime.UtcNow;
        }
    
        public void AddAmount(int amount)
        {
            if(amount <= 0)
                throw new Exception("Quantidade invalida !");

            Amount += amount;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SubtraAmount(int amount)
        {
            if (amount >= 1 || (Amount - amount) == 0 || (Amount - amount) <= -1)
                throw new Exception("Quantidade invalida !");

            Amount -= amount;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
