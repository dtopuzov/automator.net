# Download winget
$source = 'https://github.com/microsoft/winget-cli/releases/download/v1.1.12653/Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle'
$destination = 'Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle'
Invoke-RestMethod -Uri $source -OutFile $destination

# Enable Add-AppPackage
Import-Module Appx

# Install winget
Add-AppPackage -path $destination

# Install Windows Calculator
winget install "Windows Calculator" --force --silent --accept-package-agreements
Remove-Item "Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle" -Force -Recurse
