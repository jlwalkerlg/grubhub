using System;
using FoodSnap.Infrastructure.Persistence.EF;
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

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
