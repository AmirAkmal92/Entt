$cim = New-CimSession -ComputerName S099
$vm = Get-VM -CimSession $cim -Name S302
$vm.Uptime | % TotalSeconds

$S302 = New-PSSession -ComputerName S302
Copy-Item C:\project\work\sph\source\web\web.sph\SphApp -Recurse -Force -ToSession $S302 c:\apps\rx\web
Copy-Item C:\project\work\sph\bin\output -Recurse -Force -ToSession $S302 c:\apps\rx

