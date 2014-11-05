namespace CustomerOrder.Infrastructure.UnitTests
{
    using System;
    using NUnit.Framework;
    using Stateless;

    [TestFixture]
    public class StateMachineShould
    {
        [Test]
        public void ShouldRunACommandBeforeAStateIsEntered()
        {
            var onEntryExecuted = false;
            var stateMachine = new StateMachine<State, Trigger>(State.A);
            stateMachine.Configure(State.A).Permit(Trigger.One, State.B);
            stateMachine.Configure(State.B).OnEntry(() => onEntryExecuted = true);
            stateMachine.Fire(Trigger.One);
            Assert.IsTrue(onEntryExecuted);
            Assert.AreEqual(State.B, stateMachine.State);
        }

        [Test]
        public void NotChangeTheStateIfTheOnEntryOperationThrowsAnException()
        {
            var stateMachine = new StateMachine<State, Trigger>(State.A);
            stateMachine.Configure(State.A).Permit(Trigger.One, State.B);
            stateMachine.Configure(State.A).OnExit(() => { throw new Exception(); });
            Assert.Throws<Exception>(() => stateMachine.Fire(Trigger.One));
            Assert.AreEqual(State.A, stateMachine.State);
        }

        private enum Trigger
        {
            One,
        }

        private enum State
        {
            A,
            B,
        }
    }
}

