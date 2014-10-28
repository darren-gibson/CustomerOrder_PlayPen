namespace CustomerOrder.Model.UnitTests.Command
{
    internal class SlowCommand : CommandDouble
    {
        public SlowCommand() : base(100) { }
    }
}