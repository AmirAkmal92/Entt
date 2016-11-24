param(
[Parameter(Mandatory=$True,Position=1)]
[string]$Name,
[PSCredential]$AdminCredential = (Get-Credential ORG\Administrator),
[CimSession]$CimSession = (New-CimSession -ComputerName S099),
[string]$SwitchName = 'External'
)

Get-VM -CimSession $CimSession

#Create VHD for VM
    New-VHD -CimSession $CimSession `
        -Path c:\VM\2016-$Name\$Name.vhdx `
        -ParentPath "C:\VM\2016-S300\S300\Virtual Hard Disks\S300.vhdx" `
        -Differencing


# Create Virtual Machine
    New-VM -Name $Name `
        -CimSession $CimSession `
        -VHDPath "c:\VM\2016-$Name\$Name.vhdx" `
        -MemoryStartupBytes 1024MB -Generation 2 -SwitchName $SwitchName -Path "c:\VM\2016-$Name"

Set-VMMemory -CimSession $CimSession -VMName $Name -DynamicMemoryEnabled $True -MinimumBytes 512MB -StartupBytes 1024MB -MaximumBytes 5GB -Priority 50 -Buffer 20

#Get-VM -CimSession $CimSession | Get-VMMemory | Select-Object -Property VMName,DynamicMemoryEnabled

$vm = Get-VM -CimSession $CimSession -Name $Name

#Add Network Adapter if it's not there
$vm | Get-VMNetworkAdapter
$vm | Get-VMNetworkAdapter| Connect-VMNetworkAdapter -SwitchName 'External'
$vm| Get-VMNetworkAdapter


    
$vm |  Start-VM    
