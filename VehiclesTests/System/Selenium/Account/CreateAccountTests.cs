using OpenQA.Selenium.Chrome;

namespace VehiclesTests.System.Selenium.Account
{
    [Parallelizable]
    public class CreateAccountTests : IDisposable
    {
        private ChromeDriver? _driver = null;

        [OneTimeSetUp]
        public void Setup()
        {
            _driver = new();
            _driver.Navigate().GoToUrl("http://www.google.com");
        }

        [Test]
        public void GoToGoogle()
        {
            Assert.That(_driver?.Title, Is.EqualTo("Google"));
        }

        public void Dispose()
        {
            _driver?.Dispose();            
            GC.SuppressFinalize(this);
        }
    }
}
