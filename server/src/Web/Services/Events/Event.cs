using System;

namespace Web.Services.Events
{
    public abstract record Event : MediatR.IRequest
    {
        protected Event(DateTimeOffset occuredAt)
        {
            OccuredAt = occuredAt;
        }

        public DateTimeOffset OccuredAt { get; }
    }
}
