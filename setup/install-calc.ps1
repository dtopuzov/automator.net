# Install deps
Install-Module NtObjectManager -Force
$vclibs = Invoke-WebRequest -Uri "https://store.rg-adguard.net/api/GetFiles" -Method "POST" -ContentType "application/x-www-form-urlencoded" -Body "type=PackageFamilyName&url=Microsoft.VCLibs.140.00_8wekyb3d8bbwe&ring=RP&lang=en-US" -UseBasicParsing | Foreach-Object Links | Where-Object outerHTML -match "Microsoft.VCLibs.140.00_.+_x64__8wekyb3d8bbwe.appx" | Foreach-Object href
$vclibsuwp = Invoke-WebRequest -Uri "https://store.rg-adguard.net/api/GetFiles" -Method "POST" -ContentType "application/x-www-form-urlencoded" -Body "type=PackageFamilyName&url=Microsoft.VCLibs.140.00.UWPDesktop_8wekyb3d8bbwe&ring=RP&lang=en-US" -UseBasicParsing | Foreach-Object Links | Where-Object outerHTML -match "Microsoft.VCLibs.140.00.UWPDesktop_.+_x64__8wekyb3d8bbwe.appx" | Foreach-Object href

Invoke-WebRequest $vclibsuwp -OutFile Microsoft.VCLibs.140.00.UWPDesktop_8wekyb3d8bbwe.appx
Invoke-WebRequest $vclibs -OutFile Microsoft.VCLibs.140.00_8wekyb3d8bbwe.appx

Add-AppxPackage -Path .\Microsoft.VCLibs.140.00.UWPDesktop_8wekyb3d8bbwe.appx
Add-AppxPackage -Path .\Microsoft.VCLibs.140.00_8wekyb3d8bbwe.appx

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
