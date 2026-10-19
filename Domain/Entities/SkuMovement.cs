namespace api_stoke.Domain.Entities
{
    public class SkuMovement
    {
        public Guid Id { get; private set; }
        public Guid SkuProductId { get; private set; }
        public Guid MovementId { get; private set; }

        public SkuProduct SkuProduct { get; } = null!;
        public Movement Movement { get; } = null!;

        private SkuMovement() { }

        public SkuMovement(Guid skuProductId, Guid movementId, Guid id = default!)
        {
            SkuProductId = skuProductId;
            MovementId = movementId;

            Id = id == Guid.Empty ? Guid.NewGuid() : id;
        }
    }
}
