namespace api_stoke.Application.DTOs.Cart
{
    public class AddProductCartDTO
    {
        public Guid ProductId { get; set; }
        //public Guid CartItemId { get; set; } = default!; // pode ser nulo
        public List<string> Skus { get; set; }
    }
}
