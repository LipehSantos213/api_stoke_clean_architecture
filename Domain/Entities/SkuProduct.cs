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

        public enum StatusSku
        {
            VENDIDO,
            EM_ESTOQUE
        }

        public Product Product { get; } = null!;

        public List<SkuMovement> SkuMovements { get; } = new()!;

        private SkuProduct() { }

        public SkuProduct(Guid productId, string sku, Guid id = default!)
        {
            ProductId = productId;
            Sku = sku;
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Status = StatusSku.EM_ESTOQUE;
            Active = true;
        }

        public static string ToGenerateSku(string lastSku, string nameProduct)
        {
            string firstCaract = nameProduct[..3].ToUpper();
            string lastSkuFirstCaract = lastSku[..3];
            if (!firstCaract.Equals(lastSkuFirstCaract))
                throw new ConflictException("Sku não aceito !");

            string number = Regex.Replace(lastSku, @"\D", "");

            // Ex: MSO0019
            string sku = $"{firstCaract}";

            StringBuilder numberReverse = new StringBuilder(new string(number.Reverse().ToArray()));
            int i = 0;
            while (i < number.Length)
            {
                int current = numberReverse[i];
                int sum = 1;
                sum += current;

                if(sum <= 9)
                {
                    numberReverse[i] = (char)sum;
                    break;
                }
                else // maior que 10
                {
                    numberReverse[i] = '0';
                    i++;
                    continue;
                }

            }

            string n = new string(numberReverse.ToString().Reverse().ToArray());
            sku += n;

            return sku;
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
