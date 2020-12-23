using System.Linq;
using System.Threading.Tasks;
using Application.Users;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EF.Repositories
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
            return await context.Users
                .SingleOrDefaultAsync(x => x.Email.Address == email);
        }

        public async Task<User> GetById(UserId id)
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
