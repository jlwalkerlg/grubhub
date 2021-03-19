using System;
using MediatR;

namespace Web.Services.Events
{
    public abstract record Event : INotification
    {
        protected Event(DateTime occuredAt)
        {
            OccuredAt = occuredAt;
        }

        public DateTime OccuredAt { get; }
    }
}
