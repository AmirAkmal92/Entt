$LocalAdmin = Get-Credential Administrator
$DomainAdmin = Get-Credential ORG\Administrator

#client
.\_create.vm.ps1 -Name es-client-01 -AdminCredential $LocalAdmin
.\_set.up.vm.ps1 -computerName es-client-01 -localAdmin $LocalAdmin -domainAdmin $DomainAdmin

#master
$Master01 = 'es-master-01'
.\_create.vm.ps1 -Name $Master01 -AdminCredential $LocalAdmin
.\_set.up.vm.ps1 -computerName $Master01 -localAdmin $LocalAdmin -domainAdmin $DomainAdmin


$Master02 = 'es-master-02'
.\_create.vm.ps1 -Name $Master02 -AdminCredential $LocalAdmin
.\_set.up.vm.ps1 -computerName $Master02 -localAdmin $LocalAdmin -domainAdmin $DomainAdmin


Get-VM -CimSession $CimSession
$Master03 = 'es-master-03'
.\_create.vm.ps1 -Name $Master03 -AdminCredential $LocalAdmin
.\_set.up.vm.ps1 -computerName $Master03 -localAdmin $LocalAdmin -domainAdmin $DomainAdmin


Get-VM -CimSession $CimSession
$Data01 = 'es-data-01'
.\_create.vm.ps1 -Name $Data01 -AdminCredential $LocalAdmin
.\_set.up.vm.ps1 -computerName $Data01 -localAdmin $LocalAdmin -domainAdmin $DomainAdmin



Get-VM -CimSession $CimSession
$Data02 = 'es-data-02'
.\_create.vm.ps1 -Name $Data02 -AdminCredential $LocalAdmin
.\_set.up.vm.ps1 -computerName $Data02 -localAdmin $LocalAdmin -domainAdmin $DomainAdmin


Get-VM -CimSession $CimSession
$Data03 = 'es-data-03'
.\_create.vm.ps1 -Name $Data03 -AdminCredential $LocalAdmin
.\_set.up.vm.ps1 -computerName $Data03 -localAdmin $LocalAdmin -domainAdmin $DomainAdmin



Get-VM -CimSession $CimSession
$Data04 = 'es-data-04'
.\_create.vm.ps1 -Name $Data04 -AdminCredential $LocalAdmin
.\_set.up.vm.ps1 -computerName $Data04 -localAdmin $LocalAdmin -domainAdmin $DomainAdmin