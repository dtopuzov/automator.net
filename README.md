[![CI Status](https://github.com/dtopuzov/automator.net/workflows/CI/badge.svg)](https://github.com/dtopuzov/automator.net)
![](https://img.shields.io/badge/license-%20Apache--2.0-brightgreen.svg)
[![PRs Welcome](https://img.shields.io/badge/PRs-Welcome-brightgreen.svg?style=flat )](https://github.com/dtopuzov/automator.net/pulls)
[![GitHub stars](https://img.shields.io/github/stars/dtopuzov/automator.net.svg?style=flat&color=brightgreen)](https://github.com/dtopuzov/automator.net/stargazers)

# Automator.NET

## About

.NET based e2e testing framework for web, desktop and mobile.

## Setup

Download and install [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/).

Notes:
 - For CI purpouses you can skip Visual Studio installation and only install [Download .NET Core SDK](https://dotnet.microsoft.com/download).

Some additional setup is required before test run, but it is all scripted:

- `Scripts\SetupMachine.bat` to enable Developer mode and disable UAC
- `Scripts\InstallAppium.bat` to install [Appium](http://appium.io/) and [Windows Application Driver](https://github.com/microsoft/WinAppDriver) (required to run test)

## Run Tests

Sample tests for Notepad desktop app:

```bash
cd Desktop.Tests
dotnet test
```
