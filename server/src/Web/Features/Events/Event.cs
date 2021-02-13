using System;
using MediatR;

namespace Web.Features.Events
{
    public abstract record Event(DateTime CreatedAt) : INotification;
}
