REM Download and Install WinAppDriver
del /f WindowsApplicationDriver.msi
call curl -L -O https://github.com/microsoft/WinAppDriver/releases/download/v1.2-RC/WindowsApplicationDriver.msi
call msiexec /i WindowsApplicationDriver.msi /qn

REM Install Appium
call npm i -g appium
