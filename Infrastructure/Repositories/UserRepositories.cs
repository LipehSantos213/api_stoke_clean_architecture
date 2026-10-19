using api_stoke.Domain.Entities;
using api_stoke.Domain.Interfaces;
using api_stoke.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace api_stoke.Infrastructure.Repositories
{
    public class UserRepositories : IUserRepositories
    {
        private readonly AppDbContext _appContext;

        public UserRepositories(AppDbContext appContext)
        {
            _appContext = appContext;
        }

        public async Task<User> Create(User user)
        {
            _appContext.Users.Add(user);
            await _appContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetById(Guid id)
        {
            return await _appContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await _appContext.Users.FirstOrDefaultAsync(u=>u.Email == email);
        }

        public async Task<User> Update(User user)
        {
            await _appContext.Users.Where(u => u.Id == user.Id).ExecuteUpdateAsync(s => s
                .SetProperty(u=>u.Name, user.Name)
                .SetProperty(u=>u.Email, user.Email)
                );

            return (await GetById(user.Id))!;
        }
        
    }
}
