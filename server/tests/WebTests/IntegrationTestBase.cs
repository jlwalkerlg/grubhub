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
        private readonly IntegrationTestFixture fixture;
        protected readonly IntegrationTestWebApplicationFactory factory;
        private readonly Config config;

        protected IntegrationTestBase(IntegrationTestFixture fixture)
        {
            this.fixture = fixture;
            factory = fixture.Factory;
            config = factory.Services.GetRequiredService<Config>();

            ResetDatabase();
        }

        private void ResetDatabase()
        {
            using var conn = new NpgsqlConnection(config.DbConnectionString);
            conn.Open();
            fixture.Checkpoint.Reset(conn).Wait();
        }

        protected void UseTestDbContext(Action<TestDbContext> action)
        {
            using var scope = factory.Services.CreateScope();
            using var db = scope.ServiceProvider.GetRequiredService<TestDbContext>();
            action(db);
        }

        protected T UseTestDbContext<T>(Func<TestDbContext, T> action)
        {
            using var scope = factory.Services.CreateScope();
            using var db = scope.ServiceProvider.GetRequiredService<TestDbContext>();
            return action(db);
        }

        protected void Insert(params object[] entities)
        {
            UseTestDbContext(db =>
            {
                foreach (var entity in entities)
                {
                    db.Add(entity);
                }
                db.SaveChanges();
            });
        }

        protected void Reload(params object[] entities)
        {
            UseTestDbContext(db =>
            {
                foreach (var entity in entities)
                {
                    db.Entry(entity).Reload();
                }
            });
        }
    }
}
