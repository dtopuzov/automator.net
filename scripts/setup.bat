REM Enable Developer Mode and Disable UAC
call reg add "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock" /t REG_DWORD /f /v "AllowDevelopmentWithoutDevLicense" /d "1"
call C:\Windows\System32\cmd.exe /k %windir%\System32\reg.exe ADD HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System /v EnableLUA /t REG_DWORD /d 0 /f

REM Download and Install WinAppDriver
del /f WindowsApplicationDriver.msi
call curl https://github.com/microsoft/WinAppDriver/releases/download/v1.2.1/WindowsApplicationDriver_1.2.1.msi --output WindowsApplicationDriver.msi
call msiexec /i WindowsApplicationDriver.msi /qn

REM Install Appium
call npm i -g appium