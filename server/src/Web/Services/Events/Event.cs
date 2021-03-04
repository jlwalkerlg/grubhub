using System;

namespace Web.Services.Events
{
    public abstract record Event(DateTime OccuredAt) : IRequest;
}
