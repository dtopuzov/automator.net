using NUnit.Framework;

namespace Automator.Tests.Calculator.Tests
{
    public class FunctionalTests : BaseTest
    {
        [Test]
        public void Sum()
        {
            Calc.PressDigit(2);
            Calc.PressPlus();
            Calc.PressDigit(3);
            Calc.PressEqual();
            Assert.AreEqual("Display is 5", Calc.Result);
        }

        [Test]
        public void Multiply()
        {
            Calc.PressDigit(2);
            Calc.PressMultiply();
            Calc.PressDigit(3);
            Calc.PressEqual();
            Assert.AreEqual("Display is 6", Calc.Result);
        }
    }
}
