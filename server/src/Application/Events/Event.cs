using System;

namespace Application.Events
{
    public abstract record Event(DateTime CreatedAt);
}
