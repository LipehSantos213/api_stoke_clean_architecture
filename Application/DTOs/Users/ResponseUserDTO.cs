

using api_stoke.Domain.Entities;

namespace api_stoke.Application.DTOs.Users
{
    public class ResponseUserDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public UserRole Role { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }

        public enum UserRole
        {
            USUARIO,
            ADMIN
        }

        

        public static ResponseUserDTO fromDomain(User user)
        {
            return new ResponseUserDTO{
                Id= user.Id,
                Name= user.Name,
                Role= UserRole.USUARIO,
                Active= user.Active,
                CreatedAt= user.CreatedAt
            };
        }
    }
}
