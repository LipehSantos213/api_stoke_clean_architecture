namespace api_stoke.Domain.Entities
{
    public class RegistrationClients
    {
        public Guid Id { get; private set; }
        public Guid PanelId { get; private set; }
        public Guid MovementId { get; private set; }
        public Guid CartId { get; private set; }

        public Panel Panel { get; } = null!;
        public Movement Movement { get; } = null!;
        public Cart Cart { get; } = null!;

        private RegistrationClients() { }

        public RegistrationClients(Guid panelId, Guid movementId, Guid cartId, Guid id = default!) 
        {
            PanelId = panelId;
            MovementId = movementId;
            CartId = cartId;
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
        }
    }
}
