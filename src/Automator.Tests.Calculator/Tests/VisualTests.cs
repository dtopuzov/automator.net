using NUnit.Framework;

namespace Automator.Tests.Calculator.Tests
{
    public class VisualTests : BaseTest
    {
        [Test]
        public void ShouldLookOK()
        {
            Calc.SetSize(400, 500);
        }
    }
}
