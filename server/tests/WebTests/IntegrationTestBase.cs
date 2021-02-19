using System;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Web;
using WebTests.TestData;
using Xunit;

namespace WebTests
{
    [Trait("Category", "IntegrationTest")]
    [Collection("IntegrationTest")]
    public class IntegrationTestBase
    {
        protected readonly IntegrationTestFixture fixture;
        protected readonly IntegrationTestWebApplicationFactory factory;
        protected readonly Config config;

        public IntegrationTestBase(IntegrationTestFixture fixture)
        {
            this.fixture = fixture;
            factory = fixture.Factory;
            config = factory.Services.GetRequiredService<Config>();

            ResetDatabase();
        }

        private void ResetDatabase()
        {
            using (var conn = new NpgsqlConnection(config.DbConnectionString))
            {
                conn.Open();
                fixture.Checkpoint.Reset(conn).Wait();
            }
        }

        public void UseTestDbContext(Action<TestDbContext> action)
        {
            using (var scope = factory.Services.CreateScope())
            using (var db = scope.ServiceProvider.GetRequiredService<TestDbContext>())
            {
                action(db);
            };
        }

        public T UseTestDbContext<T>(Func<TestDbContext, T> action)
        {
            using (var scope = factory.Services.CreateScope())
            using (var db = scope.ServiceProvider.GetRequiredService<TestDbContext>())
            {
                return action(db);
            };
        }

        public void Insert(params object[] entities)
        {
            UseTestDbContext(db =>
            {
                db.AddRange(entities);
                db.SaveChanges();
            });
        }
    }
}
