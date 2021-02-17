using System;

namespace Web.Features.Events
{
    public abstract record Event(DateTime CreatedAt) : IRequest;
}
