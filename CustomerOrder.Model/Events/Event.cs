namespace CustomerOrder.Model.Events
{
    using System;

    public abstract class Event : IEvent
    {
        protected Event()
        {
            EventId = Guid.NewGuid();
        }

        public Guid EventId { get; private set; }
    }
}
