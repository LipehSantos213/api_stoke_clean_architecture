using api_stoke.Application.Exceptions;

namespace api_stoke.Domain.Entities
{
    public class Movement
    {
        public Guid Id { get; private set; }
        public string TypeMovement { get; private set; }
        public int Amount { get; private set; }
        public string Observation { get; private set; }
        public string User { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public List<SkuMovement> SkuMovements { get; } = new()!;
        public RegistrationClients RegistrationClient { get; } = null!;

        private Movement() { }

        public Movement(
            string typeMovement,
            int amount,
            string observation,
            string user,
            DateTime createdAt = default!,
            Guid id = default!
            ) {
            Validate(typeMovement, amount, observation, user);

            TypeMovement = typeMovement;
            Amount = amount;
            Observation = observation;
            User = user;
            CreatedAt = createdAt == DateTime.MinValue ? DateTime.UtcNow : createdAt;
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
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
    
        public void SetAmount(int amount, bool toAdd = true)
        {
            if(amount <= 0)
            {
                throw new UnprocessableException("Quantidade invalida !");
            }

            if (toAdd)
                Amount += amount;
            else
                Amount -= amount;
        }
    }
}
