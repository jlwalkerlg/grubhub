using System;

namespace Web.Services.Events
{
    public abstract record Event : MediatR.IRequest
    {
        protected Event(DateTime occuredAt)
        {
            OccuredAt = occuredAt;
        }

        public DateTime OccuredAt { get; }
    }
}
