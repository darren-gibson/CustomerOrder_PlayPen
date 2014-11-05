namespace CustomerOrder.Model.Order
{
    internal interface ITrigger<TResult>
    {
        Trigger TriggerType { get; }
        TResult Execute();
    }
}
