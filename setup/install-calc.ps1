Import-Module -Name Appx -UseWIndowsPowershell
get-appxpackage *Microsoft.WindowsCalculator* | remove-appxpackage   
