namespace CustomerOrder.Model.Events
{
    using System;

    public interface IEvent
    {
        Guid EventId { get; }
    }
}
