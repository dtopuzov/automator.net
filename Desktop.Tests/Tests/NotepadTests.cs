using Desktop.Tests.Pages;
using Framework.Desktop;
using NUnit.Framework;

namespace Desktop.Tests
{
    public class NotepadTests : AppTest
    {
        private Notepad notepad;
        private SaveDialog saveDialog;

        [OneTimeSetUp]
        public void NotepadTestsOneTimeSetUp()
        {
            notepad = new Notepad(Context);
            saveDialog = new SaveDialog(Context);
        }

        [SetUp]
        public void NotepadTestsSetUp()
        {
            if (!Context.App.IsRunning)
            {
                Context.App.Driver.LaunchApp();
            }
        }

        [TearDown]
        public void NotepadTestsTearDown()
        {
            if (saveDialog.IsPresent)
            {
                saveDialog.DontSave();
            }
        }

        [Test]
        public void TitleOfNewWindowShouldBeUntitled()
        {
            Assert.AreEqual("Untitled - Notepad", Context.App.Title, "Wrong title of default Notepad new window.");
        }

        [Test]
        public void TypeTextShouldSetText()
        {
            notepad.TypeText("Smoke test!");
            // Notes:
            // Tests executed as part of GitHub Actions run on Windows server.
            // I'm using Windows 10 workstation.
            // Application menu on Windows server is white, on Windows 10 is blue.
            // If testing on same OS then you can use tolerance 0.1 or even lower.
            notepad.Match("notepad_with_text", timeout: 5, tolerance: 5);
        }

        [Test]
        public void CloseShouldAskForSave()
        {
            notepad.TypeText("Smoke test!");
            notepad.Exit();
            Assert.IsTrue(saveDialog.IsPresent, "Save dialog not present when try to close notepad.");
        }
    }
}