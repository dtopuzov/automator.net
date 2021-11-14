Import-Module -Name Appx -UseWIndowsPowershell
$manifest = (Get-AppxPackage *WindowsCalculator*).InstallLocation + '\AppxManifest.xml'
Add-AppxPackage -DisableDevelopmentMode -Register $manifest