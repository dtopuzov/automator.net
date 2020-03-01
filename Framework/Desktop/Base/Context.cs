namespace Framework.Desktop
{
    /// <summary>
    /// Context is an object that is available during test run.
    /// It consist of:
    /// Settings - Settings object, that is generated from appsettings.json.
    /// App - App object that include methods for interaction with App (it also contains IWebDriver instance).
    /// </summary>
    public class Context
    {
        public Context(Settings settings = null, App app = null)
        {
            Settings = settings;
            App = app;
        }

        public Settings Settings { get; set; }

        public App App { get; set; }
    }
}
