# Automator.NET

[![CI Status](https://github.com/dtopuzov/automator.net/workflows/CI/badge.svg)](https://github.com/dtopuzov/automator.net)
![LICENSE](https://img.shields.io/badge/license-%20Apache--2.0-brightgreen.svg)
[![PRs Welcome](https://img.shields.io/badge/PRs-Welcome-brightgreen.svg?style=flat )](https://github.com/dtopuzov/automator.net/pulls)

## About

.NET based e2e testing framework for web, desktop and mobile.

## Technology Stack

- [NUnit](https://github.com/nunit/nunit) as unit testing framework.
- [Selenium](https://www.selenium.dev/) to driver browsers.
- [Appium](https://github.com/appium) to drive Android and iOS applications.
- [WinAppDriver](https://github.com/microsoft/WinAppDriver) to drive windows applications.

## Initial Setup

### IDE

Download and install [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/) or [Visual Studio Code](https://code.visualstudio.com/) or any other IDE of your choice.

### .NET SDK

For CI purposes you can skip Visual Studio installation and only install [Download .NET Core SDK](https://dotnet.microsoft.com/download).

### Additional Requirements

#### Web Testing

- You just need browser you want to automated to be installed.
- No other requirements.

#### Mobile Testing

- Install [Node.js](https://nodejs.org/), preferably latest LTS release.
- Install latest version of [Appium](https://www.npmjs.com/package/appium) npm package.

### Windows Desktop Application Testing

- Install [Node.js](https://nodejs.org/), preferably latest LTS release.
- Install latest version of [Appium](https://www.npmjs.com/package/appium) npm package.
- Install latest stable release of [WinAppDriver](https://github.com/microsoft/WinAppDriver/releases/tag/v1.2.1)
- Enable Windows 10 [developer mode](https://docs.microsoft.com/en-us/windows/apps/get-started/enable-your-device-for-development)

Note: Most of the steps above are scripted in `Scripts\setup.bat`.

## Test Execution

TODO.

## Contributing

Please take a look at our [contribution documentation](CONTRIBUTING.md).
