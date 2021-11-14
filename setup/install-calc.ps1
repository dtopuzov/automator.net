$source = 'https://github.com/microsoft/winget-cli/releases/download/v1.1.12653/Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle'
$destination = 'Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle'
Invoke-RestMethod -Uri $source -OutFile $destination
Add-AppPackage -path $destination
winget install "Windows Calculator" --force --silent --accept-package-agreements