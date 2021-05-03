using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Web.Data.EF;

namespace Web.Services.Events
{
    public class EFOutbox : IOutbox
    {
        private static readonly Dictionary<Type, IList<Type>> Types = new();

        private readonly ICapPublisher publisher;
        private readonly AppDbContext context;

        static EFOutbox()
        {
            foreach (var type in typeof(Startup).Assembly.GetTypes())
            {
                foreach (var iface in type.GetInterfaces())
                {
                    if (!iface.IsGenericType) continue;
                    if (iface.GetGenericTypeDefinition() != typeof(IEventListener<>)) continue;

                    var eventType = iface.GetGenericArguments().Single();

                    if (!Types.ContainsKey(eventType))
                    {
                        Types.Add(eventType, new List<Type>());
                    }

                    Types[eventType].Add(type);
                }
            }
        }

        public EFOutbox(ICapPublisher publisher, AppDbContext context)
        {
            this.publisher = publisher;
            this.context = context;
        }

        public List<Event> Events { get; } = new();

        public Task Add(Event @event)
        {
            Events.Add(@event);
            return Task.CompletedTask;
        }

        public async Task Commit()
        {
            await using var transaction = context.Database.BeginTransaction(publisher, autoCommit: false);

            foreach (var ev in Events)
            {
                foreach (var listener in Types[ev.GetType()])
                {
                    await publisher.PublishAsync(ev.GetType().Name + ":" + listener.Name, ev);
                }
            }

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
    }
}
