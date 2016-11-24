$disk = Get-WmiObject Win32_LogicalDisk -ComputerName S301 -Filter "DeviceID='C:'" |
Select-Object Size,FreeSpace

$disk.Size/1GB
$disk.FreeSpace/1GB