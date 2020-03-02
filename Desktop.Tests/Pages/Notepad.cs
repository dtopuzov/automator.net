using Framework.Desktop;

namespace Desktop.Tests.Pages
{
    public class Notepad : AppPage
    {
        public Notepad(Context context) : base(context)
        {
        }

        public void TypeText(string text)
        {
             Context.App.Driver.FindElementByAccessibilityId("15").SendKeys(text);
        }

        public void Exit()
        {
            Context.App.CloseApp();
        }
    }
}
