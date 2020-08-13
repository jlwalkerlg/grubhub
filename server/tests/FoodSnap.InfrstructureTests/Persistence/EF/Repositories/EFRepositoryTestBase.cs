using System;
using FoodSnap.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FoodSnap.InfrastructureTests.Persistence.EF.Repositories
{
    [Collection("EF")]
    public abstract class EFRepositoryTestBase : IDisposable
    {
        protected readonly AppDbContext context;

        public EFRepositoryTestBase(EFContextFixture fixture)
        {
            context = fixture.CreateContext();
            context.Database.BeginTransaction();
        }

        protected void FlushContext()
        {
            context.SaveChanges();

            foreach (var entry in context.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
