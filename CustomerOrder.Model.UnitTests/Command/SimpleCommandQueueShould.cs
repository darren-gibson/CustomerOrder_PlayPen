namespace CustomerOrder.Model.UnitTests.Command
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Model.Command;
    using Moq;
    using NUnit.Framework;

    public class SimpleCommandQueueShould
    {
        private ICommandQueue _queueUnderTest;

        [SetUp]
        public void SetUp()
        {
            _queueUnderTest = new SimpleCommandQueue();
        }

        [Test]
        public void ShouldGetCommandsInTheOrderThatTheyArePutOnTheQueue()
        {
            var commands = AddCommandsToQueue(3);
            foreach (var command in commands)
            {
                Assert.AreEqual(command, _queueUnderTest.TryDequeue());
            }
        }

        [Test]
        public void ShouldReturnNullWhenTheresNothingWaitingOnTheQueueAndTheTimeoutIsReached()
        {
            var result = _queueUnderTest.TryDequeue(1);
            Assert.IsNull(result);
        }

        [Test]
        public void ShouldWaitForTheAmountOfTimeSpecifiedInTheTimeout()
        {
            const long expectedMilliseconds = 50;
            const int maxVarianceInMilliseconds = 10;
            var sw = Stopwatch.StartNew();
            _queueUnderTest.TryDequeue(expectedMilliseconds);
            sw.Stop();
            Assert.GreaterOrEqual(sw.ElapsedMilliseconds, expectedMilliseconds - maxVarianceInMilliseconds);
            Assert.LessOrEqual(sw.ElapsedMilliseconds, expectedMilliseconds + maxVarianceInMilliseconds);
        }

        [Test]
        public void AReadWithTimeoutShouldReturnNull()
        {
            var result = _queueUnderTest.TryDequeue(1);
            Assert.IsNull(result);
        }

        [Test]
        public void BeThreadSafeToConsumeFrom()
        {
            var commands = new HashSet<ICommand>(AddCommandsToQueue(100));

            var commandsDequeued = new ConcurrentQueue<ICommand>();
            Parallel.For(0, 4, i => DequeueOnThreadTo(commandsDequeued));

            Assert.AreEqual(commands.Count, commandsDequeued.Count);
            ICommand actualCommand;
            while (commandsDequeued.TryDequeue(out actualCommand))
            {
                Assert.IsTrue(commands.Contains(actualCommand));
                commands.Remove(actualCommand);
            }
        }

        [Test]
        public void NotAllowANullCommandToBeAddedAsThisWillFakeATimeout()
        {
            Assert.Throws<ArgumentNullException>(() => _queueUnderTest.Enqueue(null));
        }

        private IEnumerable<ICommand> AddCommandsToQueue(int numberOfCommandsToAdd)
        {
            var commands = new List<ICommand>(numberOfCommandsToAdd);
            for (var i = 0; i < numberOfCommandsToAdd; ++i)
            {
                var command = CreateCommand();
                commands.Add(command);
                _queueUnderTest.Enqueue(command);
            }
            return commands;
        }

        private void DequeueOnThreadTo(ConcurrentQueue<ICommand> queue)
        {
            ICommand result;
            int countOfResults = 0;
            while ((result = _queueUnderTest.TryDequeue(50)) != null)
            {
                queue.Enqueue(result);
                ++countOfResults;
            }
            Console.Out.WriteLine("Thread {0}: {1} results.", Thread.CurrentThread.ManagedThreadId, countOfResults);
        }

        private ICommand CreateCommand()
        {
            return new Mock<ICommand>().Object;
        }
    }
}