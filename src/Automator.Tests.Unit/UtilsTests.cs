using Automator.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Automator.Tests.Unit
{
    [TestClass]
    public class UtilsTests
    {
        [TestMethod]
        public void FindFreePort()
        {
            var port = Network.GetAvailablePort(8000, 9000);
            Assert.AreEqual(8000, port);
        }
    }
}
