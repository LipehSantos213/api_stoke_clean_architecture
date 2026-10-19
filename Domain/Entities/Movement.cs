using api_stoke.Application.Exceptions;

namespace api_stoke.Domain.Entities
{
    public class Movement
    {
        public Guid Id { get; private set; }
        //public Guid ProductId { get; private set; }
        public string TypeMovement { get; private set; }
        public int Amount { get; private set; }
        public string Observation { get; private set; }
        public string User { get; private set; }
        public DateTime CreatedAt { get; private set; }

        //public Guid ProductSkuId { get; private set; } // adicionado !

        //public Product Product { get; } = null!;
        public List<SkuMovement> SkuMovements { get; } = new()!;
        public RegistrationClients RegistrationClient { get; } = null!;

        // Contrutor privado para o EF Core
        private Movement() { }

        public Movement(
            //Guid productId,
            string typeMovement,
            int amount,
            string observation,
            string user,
            DateTime createdAt = default!,
            Guid id = default!
            //Product product = null!
            //Guid productSkuId = default!
            ) {
            Validate(typeMovement, amount, observation, user);

            //ProductId = productId;
            TypeMovement = typeMovement;
            Amount = amount;
            Observation = observation;
            User = user;
            CreatedAt = createdAt == DateTime.MinValue ? DateTime.UtcNow : createdAt;
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            //ProductSkuId = productSkuId;
            //Product = product;
        }

        private void Validate(string typeMovement, int amount, string observation, string user)
        {
            if(typeMovement != "ENTRADA" && typeMovement != "SAIDA")
            {
                throw new UnprocessableException("Tipo invalido !");
            }

            else if(amount <= 0)
            {
                throw new UnprocessableException("Quantidade invalida !");
            }

            else if(observation.Length >= 200 || observation.Length <= 10)
            {
                throw new UnprocessableException("Observação invalida !");
            }

            else if(user != "USUARIO" && user != "ADMINISTRADOR")
            {
                throw new UnprocessableException("Usuario invalido !");
            }
        }
    }
}
