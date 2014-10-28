namespace CustomerOrder.AcceptanceTests.Helpers
{
    using System;
    using Host;
    using Nancy.Hosting.Self;
    using TechTalk.SpecFlow;

    [Binding]
    public class Application
    {
        private static NancyHost _host;
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            var uri = new Uri(CustomerOrderHttpClient.BaseAddress);

            var bootstrapper = new Bootstrapper();
            _host = new NancyHost(uri, bootstrapper);
            _host.Start();
            Registry = bootstrapper.Registry;
        }

        public static Registry Registry { get; private set; }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            _host.Dispose();
        }
    }
}
