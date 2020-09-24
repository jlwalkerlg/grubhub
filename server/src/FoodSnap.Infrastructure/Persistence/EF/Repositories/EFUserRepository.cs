using System;
using System.Linq;
using System.Threading.Tasks;
using FoodSnap.Application.Users;
using FoodSnap.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace FoodSnap.Infrastructure.Persistence.EF.Repositories
{
    public class EFUserRepository : IUserRepository
    {
        private readonly AppDbContext context;

        public EFUserRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<User> GetByEmail(string email)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.Email.Address == email);
        }

        public async Task<User> GetById(Guid id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<bool> EmailExists(string email)
        {
            var count = await context.Users
                .Where(x => x.Email.Address == email)
                .CountAsync();

            return count > 0;
        }
    }
}
