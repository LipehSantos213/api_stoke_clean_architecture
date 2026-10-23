namespace api_stoke.Application.DTOs.Cart
{
    public class AddProductCartDTO
    {
        public Guid ProductId { get; set; }
        public List<string> Skus { get; set; }
    }
}
