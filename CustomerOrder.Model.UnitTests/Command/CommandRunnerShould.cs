namespace CustomerOrder.Model.UnitTests.Command
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using Model.Command;
    using Moq;
    using NUnit.Framework;

    public class CommandRunnerShould
    {
        private CommandRunner _commandRunner;
        private Mock<ICommandRepository> _mockRepository;
        private SimpleCommandQueue _queue;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<ICommandRepository>();
            _queue = new SimpleCommandQueue();
            _commandRunner = new CommandRunner(_mockRepository.Object, _queue);
        }

        [TearDown]
        public void TearDown()
        {
            _commandRunner.Stop();
        }

        [Test]
        public void RunsTheCommand()
        {
            var quickCommand = new QuickCommand();
            var commandExecutedEvent = new ManualResetEvent(false);
            quickCommand.CommandStarted += (sender, args) => commandExecutedEvent.Set();

            _commandRunner.RunCommand(quickCommand);

            if (commandExecutedEvent.WaitOne(1000) == false)
                Assert.Fail("Command was not run");
        }

        [Test]
        public void AsynchronouslyRunsTheCommand()
        {
            var slowCommand = new SlowCommand();
            _commandRunner.RunCommand(slowCommand);
            Assert.IsFalse(slowCommand.IsFinished);
        }

        [Test]
        public void ThrowAnExceptionIfTheCommandDoesNotHaveAnIdentifierAndNotSaveTheCommand()
        {
            var command = new Mock<ICommand>().Object;
            Assert.Throws<ArgumentNullException>(() => _commandRunner.RunCommand(command));
            _mockRepository.Verify(r => r.Save(command), Times.Never);
        }

        [Test]
        public void RecordTheReceiptOfTheCommandInTheCommandRepository()
        {
            var expectedCommand = new QuickCommand();
            RunCommandAndWaitToComplete(expectedCommand);

            _mockRepository.Verify(r => r.Save(expectedCommand));
        }

        [Test]
        public void RecordTheResultOfTheCommandToTheCommandRepository()
        {
            var expectedResult = new object();
            var commandMock = CreateCommandMock(expectedResult);

            RunCommandAndWaitToComplete(commandMock.Object);

            _mockRepository.Verify(r => r.Save(commandMock.Object, expectedResult));
        }

        private void RunCommandAndWaitToComplete(ICommand command)
        {
            _commandRunner.RunCommand(command);
            var commandCompleteEvent = new ManualResetEvent(false);
            _commandRunner.CommandComplete += (sender, command1) => commandCompleteEvent.Set();

            /*for (var i = 0; i < 1; ++i)
            {
                Debug.WriteLine("queue: {0}, length: {1}", _queue.GetHashCode(), _queue.Length);
                Thread.Sleep(100);
            }*/
            if(!commandCompleteEvent.WaitOne(1000))
                Assert.Fail("command did not complete within timeout");
        }

        [Test]
        public void RecordAnyExceptionThrownByTheCommandInTheRepository()
        {
            var commandMock = CreateCommandMock("hello");
            commandMock.Setup(c => c.Execute()).Throws<InvalidOperationException>();

            RunCommandAndWaitToComplete(commandMock.Object);

            _mockRepository.Verify(r => r.Save(commandMock.Object, It.IsAny<InvalidOperationException>()));
        }

        [Test]
        public void NotProcessCommandsWhenStopped()
        {
            var quickCommand = new QuickCommand();
            StopCommandRunnerAndWaitForItToStop();

            var commandExecutedEvent = new ManualResetEvent(false);
            quickCommand.CommandStarted += (sender, args) => commandExecutedEvent.Set();

            _commandRunner.RunCommand(quickCommand);
            if (commandExecutedEvent.WaitOne(200))
                Assert.Fail("Command was run and should not have been");
        }

        private void StopCommandRunnerAndWaitForItToStop()
        {
            _commandRunner.Stop();

            var sw = Stopwatch.StartNew();
            while (_commandRunner.Status != CommandRunnerStatus.Stopped)
            {
                Assert.LessOrEqual(sw.ElapsedMilliseconds, 2000, "Runner has not stopped within timeout period");
                Thread.Sleep(50);
            }
        }

        [Test]
        public void WhenAlreadyStoppedThenTheStateShouldNotMoveToStoppingIfStoppedAgain()
        {
            StopCommandRunnerAndWaitForItToStop();
            _commandRunner.Stop();
            Assert.AreEqual(CommandRunnerStatus.Stopped, _commandRunner.Status);

        }

        [Test]
        public void NotStartAgainWhenAlreadyStarted()
        {
            Thread.Sleep(100); // to ensure the workers are all started
            var workerThreadsBefore = _commandRunner.ActiveWorkers;
            _commandRunner.Start();
            Thread.Sleep(100); // to ensure any additional workers have time to start (but hopefully there isn't any)
            Assert.AreEqual(workerThreadsBefore, _commandRunner.ActiveWorkers);
        }

        private static Mock<ICommand> CreateCommandMock(object expectedResult)
        {
            var commandMock = new Mock<ICommand>();
            var expectedGuid = Guid.NewGuid();
            commandMock.SetupGet(c => c.Id).Returns(expectedGuid);
            commandMock.Setup(c => c.Execute()).Returns(expectedResult);
            return commandMock;
        }
    }
}