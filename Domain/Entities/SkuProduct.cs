using api_stoke.Application.Exceptions;
using System.Text;
using System.Text.RegularExpressions;

namespace api_stoke.Domain.Entities
{
    public class SkuProduct
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public string Sku { get; private set; }
        public StatusSku Status { get; private set; }
        public bool Active { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public enum StatusSku
        {
            VENDIDO,
            EM_ESTOQUE
        }

        public Product Product { get; } = null!;

        public List<SkuMovement>? SkuMovements { get; } = new()!;

        public List<SkuCartItem>? SkuCartItens { get; } = new()!;

        private SkuProduct() { }

        public SkuProduct(Guid productId, string sku, Guid id = default!)
        {
            ProductId = productId;
            Sku = sku;
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Status = StatusSku.EM_ESTOQUE;
            Active = true;
            CreatedAt = DateTime.UtcNow;
        }

        public static string ToGenerateSku(string lastSku, string nameProduct)
        {
            string prefix = nameProduct[..3].ToUpper();

            if (!lastSku.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                throw new ConflictException("SKU não aceito!");

            string number = Regex.Replace(lastSku, @"\D", "");

            if (!int.TryParse(number, out int currentNumber))
                throw new ConflictException("Formato de SKU inválido!");

            return $"{prefix}{currentNumber + 1:D4}";
        }

        public static string FirstSku(string nameProduct)
        {
            string firstCaract = nameProduct[..3].ToUpper();

            return $"{firstCaract}0001";
        }

        public void UpdateStatus(StatusSku status)
        {
            if (status.Equals(Status))
            {
                var s = status == StatusSku.EM_ESTOQUE ? "em estoque" : "vendido";
                throw new ConflictException($"O status já está {s}");
            }

            Status = status;
        }
    
        public void SetStatusSku(bool value)
        {
            if(Active == value)
            {
                string status = value ? "ativado" : "desativado";
                throw new ConflictException($"O sku já esta {status} !");
            }

            Active = value;
        }
    }
}
