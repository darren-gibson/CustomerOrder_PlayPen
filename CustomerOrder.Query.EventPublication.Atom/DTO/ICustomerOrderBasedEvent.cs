namespace CustomerOrder.Query.EventPublication.Atom.DTO
{
    public interface ICustomerOrderBasedEvent
    {
        string EventId { get; set; }
        string Order { get; set; }
    }
}
