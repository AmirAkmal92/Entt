param(
    [Parameter(Mandatory=$True,Position=1)]
    [string]$computerName,
    [PsCredential]$localAdmin = (Get-Credential 'Administrator'),
    [PsCredential]$domainAdmin = (Get-Credential 'ORG\Administrator'),
    [string]$domainName = 'org.1mdb',
    [string]$dnsServer = "192.168.1.184",
    [CimSession]$hostCim = (New-CimSession -ComputerName S099)
)


$vm = Get-VM -CimSession $hostCim -Name $computerName
$defaultIp = $vm | Get-VMNetworkAdapter | % IPAddresses | select -First 1
$ip = $defaultIp

Get-Service WinRM | Start-Service

Get-Item WSMan:\localhost\Client\TrustedHosts
Set-item WSMAN:\Localhost\Client\TrustedHosts -value '*'

$cimsession = New-CimSession -Credential $localAdmin -ComputerName $ip

Invoke-command `
-ComputerName $ip `
-Credential $localAdmin `
-scriptblock {Rename-Computer -NewName $using:computerName -Restart}
Write-Host "Rename the computer, wait for a restart"
sleep -Seconds 10 

#TODO wait until the vm startup time to be at least 30 seconds or more
while($vm.Uptime.TotalSeconds -le 45){
    $upTime = $vm.Uptime.TotalSeconds
    Write-Host "Stil waiting $upTime"
    sleep -Seconds 5
}

#reconnect after restart
$cimsession = New-CimSession -Credential $localAdmin -ComputerName $ip

#set the DNS and JOIN domain
$netConfig = Get-NetIPConfiguration -CimSession $cimsession
Set-DnsClientServerAddress `
-CimSession $cimsession `
-InterfaceIndex $netConfig.InterfaceIndex `
-ServerAddresses @($dnsServer)

Write-Host "Setting Dns server to $dnsServer"

Invoke-command `
-ComputerName $ip `
-Credential $localAdmin `
-ScriptBlock { $env:COMPUTERNAME }

Write-Host "Setting Fixed Ip  to $ip"

Invoke-command `
-ComputerName $ip `
-Credential $localAdmin `
-ScriptBlock {Add-Computer -DomainName $using:domainName -Credential $using:domainAdmin -Restart}

Write-Host "Joining domain and waiting for restart"

#Re-Set Trusted Hosts
Get-Item WSMan:\localhost\Client\TrustedHosts
Set-item WSMAN:\Localhost\Client\TrustedHosts -value ''
