# Machine Setup

## Desktop Testing

Download and install [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/).

Notes:

- For CI purpouses you can skip Visual Studio installation and only install [Download .NET Core SDK](https://dotnet.microsoft.com/download).

Some additional setup is required before test run, but it is all scripted:

- `Scripts\SetupMachine.bat` to enable Developer mode and disable UAC
- `Scripts\InstallAppium.bat` to install [Appium](http://appium.io/) and [Windows Application Driver](https://github.com/microsoft/WinAppDriver) (required to run test)
