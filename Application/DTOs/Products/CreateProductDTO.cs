namespace api_stoke.Application.DTOs.Products
{
    public class CreateProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; } // Centavos
        public int Amount { get; set; }
    }
}
