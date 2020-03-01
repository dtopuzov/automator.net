using Framework.Desktop;
using OpenQA.Selenium.Support.UI;
using System;

namespace Desktop.Tests.Pages
{
    public class SaveDialog : AppPage
    {
        public SaveDialog(Context context) : base(context)
        {
        }

        public bool IsPresent => Context.App.Driver.FindElementsByAccessibilityId("MainInstruction").Count > 0;

        public void Save()
        {
            Context.App.Driver.FindElementByAccessibilityId("CommandButton_6").Click();
        }

        public void DontSave()
        {
            Context.App.Driver.FindElementByAccessibilityId("CommandButton_7").Click();
        }

        public void Cancel()
        {
            Context.App.Driver.FindElementByAccessibilityId("CommandButton_2").Click();
        }
    }
}
