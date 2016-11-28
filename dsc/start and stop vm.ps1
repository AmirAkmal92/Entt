#22222
Get-VM -CimSession $S099 -Name S322 | Stop-VM
Get-VM -CimSession $S099 -Name S322 | Start-VM

# 3333333
Get-VM -CimSession $S099 -Name S323 | Stop-VM
Get-VM -CimSession $S099 -Name S323 | Start-VM

Get-VM -CimSession $S099 -Name S323,S322