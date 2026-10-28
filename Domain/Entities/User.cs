using api_stoke.Application.Exceptions;

namespace api_stoke.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public UserRole Role { get; private set; }
        public bool Active { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public List<Product>? Products { get; private set; } = new();
        public Panel Panel { get; } = null!;
        public Cart Cart { get; } = null!;


        public enum UserRole { 
            USUARIO,
            ADMIN
        }

        private User() { }

        public User(
            string name,
            string email,
            UserRole role,
            bool active = default!,
            string password = default!,
            Guid id = default!,
            DateTime createdAt = default!,
            DateTime updatedAt = default!,
            Panel panel = default!,
            Cart cart = default!)
        {
            Validate(name, email);

            Name = name;
            Email = email;
            Role = role;
            Active = active;
            Password = password;
            Id = id == Guid.Empty? Guid.NewGuid():id;
            CreatedAt = createdAt == DateTime.MinValue? DateTime.UtcNow : createdAt;
            UpdatedAt = updatedAt == DateTime.MinValue? DateTime.UtcNow : updatedAt;
            Panel = panel;
            Cart = cart;
        }

        private void Validate(string name, string email)
        {
            if (name.Length >= 120 || name.Length <= 5)
                throw new UnprocessableException("Nome invalido !");
            if (
                email.Length >= 200 ||
                email.Length <= 10 ||
                !email.Contains("@gmail.com") ||
                !email.EndsWith("@gmail.com"))
                throw new UnprocessableException("Email invalido !");
        }
    
        public void Update(string? name, string? email)
        {
            Validate(name??Name, email??Email);

            Name = name ?? Name;
            Email = email ?? Email;
        }

        public void UpdateStatus(bool value)
        {
            if(value == Active)
            {
                var status = value ? "ativado" : "desativado";
                throw new ConflictException($"O usuario já está {status}");
            }

            Active = value;
            UpdatedAt = DateTime.UtcNow;
        }
    
        public bool UpdatePassword(string newPassword)
        {
            if (Password == newPassword || newPassword.Length <8 || newPassword.Length >20)
                return false;
            Password = newPassword;
            UpdatedAt = DateTime.UtcNow;
            return true;
        }
    }
}
