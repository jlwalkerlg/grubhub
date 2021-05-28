using System;
using DotNetCore.CAP;

namespace Web.Services.Events
{
    public class SubscribeAttribute : CapSubscribeAttribute
    {
        public SubscribeAttribute(string eventName, Type listenerType) : base(eventName)
        {
            Group = listenerType.Name;
        }
    }
}
