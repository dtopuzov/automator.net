REM Download and Install Fiddler
rmdir /s /q "%USERPROFILE%\AppData\Local\Programs\Fiddler"
rmdir /s /q "%USERPROFILE%\Documents\Fiddler2"
del /f FiddlerSetup.exe
call curl https://telerik-fiddler.s3.amazonaws.com/fiddler/FiddlerSetup.exe --output FiddlerSetup.exe
call FiddlerSetup.exe /S
dir "%USERPROFILE%\AppData\Local\Programs\Fiddler"
